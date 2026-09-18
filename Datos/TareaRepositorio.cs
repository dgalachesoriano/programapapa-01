using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GestionFacturas.Modelos;

namespace GestionFacturas.Datos
{
    /// <summary>
    /// Acceso a datos para las tareas y sus líneas de detalle (tablas
    /// TBL_TAREAS y TBL_DETALLETAREA), y para los eventos del flujo
    /// de una tarea (tabla TBL_CONTROL: alta, asignación, bloqueo y
    /// facturación). Concentra el SQL y la gestión de transacciones,
    /// igual que hacía FacturaRepositorio en el modelo anterior.
    /// </summary>
    internal class TareaRepositorio
    {
        private readonly EstadoTareaRepositorio estadoRepositorio = new EstadoTareaRepositorio();
        private readonly FacturadoRepositorio facturadoRepositorio = new FacturadoRepositorio();

        /// <summary>Nombre del estado inicial que recibe toda tarea nueva.</summary>
        private const string EstadoRegistrado = "REGISTRADO";

        /// <summary>Nombre del estado tras asignar usuario.</summary>
        private const string EstadoEnProceso = "EN_PROCESO";

        /// <summary>Nombre del estado tras bloquear la tarea.</summary>
        private const string EstadoBloqueado = "BLOQUEADO";

        /// <summary>Nombre del estado tras facturar la tarea.</summary>
        private const string EstadoFacturado = "FACTURADO";

        /// <summary>
        /// Busca tareas aplicando el filtro indicado sobre
        /// VW_TAREAS_ESTADO_ACTUAL (estado/usuario/fechas actuales de
        /// cada tarea) y devuelve el resultado como DataTable, listo
        /// para enlazar directamente a un DataGridView. Incluye
        /// ID_TAREA para que la pantalla pueda identificar la fila
        /// seleccionada, aunque esa columna se oculta siempre en el
        /// grid (ver Servicios/FormateadorGridTareas).
        /// </summary>
        public DataTable BuscarPorFiltro(FiltroBusquedaTareas filtro)
        {
            DataTable tabla = new DataTable();

            const string sql = @"
                SELECT
                    ID_TAREA,
                    DES_DOC,
                    FEC_ENT_CAL,
                    FEC_REG,
                    DES_ORG_VENTAS,
                    DES_PROYECTO,
                    DES_SEGMENTO,
                    IMP_ESTIMADO,
                    DES_ESTADO,
                    NOMBRE_USUARIO,
                    DES_MOTIVO

                FROM dbo.VW_TAREAS_ESTADO_ACTUAL

                WHERE
                    (@COD_SEQ_EST = 0 OR COD_SEQ_EST = @COD_SEQ_EST)

                    AND

                    (@COD_SEQ_USER = 0 OR COD_SEQ_USER = @COD_SEQ_USER)

                    AND

                    (@DES_DOC = '' OR DES_DOC LIKE '%' + @DES_DOC + '%')

                    AND

                    (@FEC_DESDE IS NULL OR FEC_REG >= @FEC_DESDE)

                    AND

                    (@FEC_HASTA IS NULL OR FEC_REG < DATEADD(day, 1, @FEC_HASTA))

                ORDER BY
                    ID_TAREA DESC;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@COD_SEQ_EST", SqlDbType.Int).Value = filtro.IdEstado;

                comando.Parameters.Add("@COD_SEQ_USER", SqlDbType.Int).Value = filtro.IdUsuario;

                comando.Parameters.Add("@DES_DOC", SqlDbType.NVarChar).Value = filtro.Documento ?? string.Empty;

                comando.Parameters.Add("@FEC_DESDE", SqlDbType.Date).Value =
                    filtro.FechaDesde.HasValue ? (object)filtro.FechaDesde.Value.Date : DBNull.Value;

                comando.Parameters.Add("@FEC_HASTA", SqlDbType.Date).Value =
                    filtro.FechaHasta.HasValue ? (object)filtro.FechaHasta.Value.Date : DBNull.Value;

                using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }

        /// <summary>
        /// Obtiene la cabecera de una tarea por su identificador, o
        /// null si no existe.
        /// </summary>
        public Tarea ObtenerPorId(int id)
        {
            const string sql = @"
                SELECT
                    ID,
                    DES_DOC,
                    FEC_ENT_CAL,
                    FEC_REG,
                    DES_ORG_VENTAS,
                    COD_SEQ_PRY,
                    COD_SEQ_SEG,
                    IMP_ESTIMADO

                FROM dbo.TBL_TAREAS

                WHERE ID = @ID;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@ID", SqlDbType.Int).Value = id;

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    if (!lector.Read())
                        return null;

                    return new Tarea
                    {
                        Id = Convert.ToInt32(lector["ID"]),
                        Documento = lector["DES_DOC"].ToString(),
                        FechaEntradaCalidad = Convert.ToDateTime(lector["FEC_ENT_CAL"]),
                        FechaRegistro = Convert.ToDateTime(lector["FEC_REG"]),
                        OrganizacionVentas = lector["DES_ORG_VENTAS"].ToString(),
                        IdProyecto = Convert.ToInt32(lector["COD_SEQ_PRY"]),
                        IdSegmento = Convert.ToInt32(lector["COD_SEQ_SEG"]),

                        ImporteEstimado = lector["IMP_ESTIMADO"] == DBNull.Value
                            ? (decimal?)null
                            : Convert.ToDecimal(lector["IMP_ESTIMADO"])
                    };
                }
            }
        }

        /// <summary>
        /// Obtiene las líneas de detalle de una tarea, en el mismo
        /// orden en que fueron registradas.
        /// </summary>
        public List<DetalleTarea> ObtenerDetalle(int idTarea)
        {
            List<DetalleTarea> detalles = new List<DetalleTarea>();

            const string sql = @"
                SELECT
                    COD_PED_VENTA,
                    COD_PED_COMPRA,
                    COD_PED_INSPEC,
                    COD_ENT_ENTR,
                    NBR_UNIDADES

                FROM dbo.TBL_DETALLETAREA

                WHERE COD_SEQ_TAR = @COD_SEQ_TAR

                ORDER BY ID;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@COD_SEQ_TAR", SqlDbType.Int).Value = idTarea;

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        detalles.Add(new DetalleTarea
                        {
                            PedidoVenta = ObtenerTextoONulo(lector, "COD_PED_VENTA"),
                            PedidoCompra = ObtenerTextoONulo(lector, "COD_PED_COMPRA"),
                            PedidoInspeccion = ObtenerTextoONulo(lector, "COD_PED_INSPEC"),
                            EntidadEntrega = ObtenerTextoONulo(lector, "COD_ENT_ENTR"),

                            Unidades = lector["NBR_UNIDADES"] == DBNull.Value
                                ? (decimal?)null
                                : Convert.ToDecimal(lector["NBR_UNIDADES"])
                        });
                    }
                }
            }

            return detalles;
        }

        /// <summary>
        /// Obtiene el detalle de varias tareas a la vez (p. ej. una
        /// selección múltiple en el Pool de Tareas), como DataTable
        /// listo para enlazar a un DataGridView. Incluye el Documento
        /// de cada tarea para poder distinguir a qué tarea pertenece
        /// cada línea cuando se mezclan detalles de varias. Devuelve
        /// una tabla vacía si <paramref name="idsTarea"/> no tiene
        /// elementos.
        /// </summary>
        public DataTable ObtenerDetalle(IEnumerable<int> idsTarea)
        {
            DataTable tabla = new DataTable();

            List<int> ids = new List<int>(idsTarea);

            if (ids.Count == 0)
                return tabla;

            List<string> nombresParametros = new List<string>();

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand())
            {
                comando.Connection = conexion;

                for (int i = 0; i < ids.Count; i++)
                {
                    string nombreParametro = "@ID" + i;

                    nombresParametros.Add(nombreParametro);

                    comando.Parameters.Add(nombreParametro, SqlDbType.Int).Value = ids[i];
                }

                comando.CommandText = @"
                    SELECT
                        T.DES_DOC,
                        D.COD_PED_VENTA,
                        D.COD_PED_COMPRA,
                        D.COD_PED_INSPEC,
                        D.COD_ENT_ENTR,
                        D.NBR_UNIDADES

                    FROM dbo.TBL_DETALLETAREA AS D

                    INNER JOIN dbo.TBL_TAREAS AS T
                        ON T.ID = D.COD_SEQ_TAR

                    WHERE D.COD_SEQ_TAR IN (" + string.Join(",", nombresParametros) + @")

                    ORDER BY T.DES_DOC, D.ID;";

                using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }

        /// <summary>
        /// Inserta una nueva tarea junto con sus líneas de detalle y
        /// el evento inicial de TBL_CONTROL (estado "REGISTRADO", sin
        /// usuario asignado), todo en una única transacción. Devuelve
        /// el identificador de tarea generado.
        /// </summary>
        public int Insertar(Tarea tarea, List<DetalleTarea> detalles)
        {
            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlTransaction transaccion = conexion.BeginTransaction())
            {
                try
                {
                    int idTarea = InsertarCabecera(tarea, conexion, transaccion);

                    InsertarDetalles(idTarea, detalles, conexion, transaccion);

                    int idEstadoRegistrado = estadoRepositorio.ObtenerIdPorDescripcion(
                        EstadoRegistrado, conexion, transaccion);

                    InsertarEventoControl(
                        idTarea, idUsuario: null, idFacturado: null, idEstado: idEstadoRegistrado,
                        idMotivo: null, conexion, transaccion);

                    transaccion.Commit();

                    return idTarea;
                }
                catch
                {
                    RevertirSinPropagar(transaccion);
                    throw;
                }
            }
        }

        /// <summary>
        /// Actualiza la cabecera de una tarea existente y sustituye
        /// por completo sus líneas de detalle, en una única
        /// transacción. No modifica TBL_CONTROL: editar los datos de
        /// una tarea no cambia su estado en el flujo.
        /// </summary>
        public void Actualizar(Tarea tarea, List<DetalleTarea> detalles)
        {
            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlTransaction transaccion = conexion.BeginTransaction())
            {
                try
                {
                    ActualizarCabecera(tarea, conexion, transaccion);

                    EliminarDetalle(tarea.Id, conexion, transaccion);

                    InsertarDetalles(tarea.Id, detalles, conexion, transaccion);

                    transaccion.Commit();
                }
                catch
                {
                    RevertirSinPropagar(transaccion);
                    throw;
                }
            }
        }

        /// <summary>
        /// Asigna de una sola vez el usuario indicado a todas las
        /// tareas cuyo identificador esté en <paramref name="idsTarea"/>,
        /// insertando un nuevo evento en TBL_CONTROL (estado
        /// "EN_PROCESO") por cada una. No hace nada si la lista de
        /// identificadores está vacía.
        /// </summary>
        public void AsignarUsuario(IEnumerable<int> idsTarea, int idUsuario)
        {
            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlTransaction transaccion = conexion.BeginTransaction())
            {
                try
                {
                    int idEstadoEnProceso = estadoRepositorio.ObtenerIdPorDescripcion(
                        EstadoEnProceso, conexion, transaccion);

                    foreach (int idTarea in idsTarea)
                    {
                        InsertarEventoControl(
                            idTarea, idUsuario, idFacturado: null, idEstado: idEstadoEnProceso,
                            idMotivo: null, conexion, transaccion);
                    }

                    transaccion.Commit();
                }
                catch
                {
                    RevertirSinPropagar(transaccion);
                    throw;
                }
            }
        }

        /// <summary>
        /// Bloquea de una sola vez todas las tareas cuyo identificador
        /// esté en <paramref name="idsTarea"/>, con el motivo indicado
        /// (obligatorio), insertando un nuevo evento en TBL_CONTROL
        /// (estado "BLOQUEADO") por cada una. El usuario asignado se
        /// mantiene: se copia del evento más reciente de cada tarea.
        /// No hace nada si la lista de identificadores está vacía.
        /// </summary>
        public void Bloquear(IEnumerable<int> idsTarea, int idMotivo)
        {
            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlTransaction transaccion = conexion.BeginTransaction())
            {
                try
                {
                    int idEstadoBloqueado = estadoRepositorio.ObtenerIdPorDescripcion(
                        EstadoBloqueado, conexion, transaccion);

                    foreach (int idTarea in idsTarea)
                    {
                        InsertarEventoControlConUsuarioActual(
                            idTarea, idFacturado: null, idEstado: idEstadoBloqueado,
                            idMotivo: idMotivo, conexion, transaccion);
                    }

                    transaccion.Commit();
                }
                catch
                {
                    RevertirSinPropagar(transaccion);
                    throw;
                }
            }
        }

        /// <summary>
        /// Desbloquea de una sola vez todas las tareas cuyo
        /// identificador esté en <paramref name="idsTarea"/>,
        /// devolviendo cada una a su estado anterior al bloqueo
        /// (usuario y estado del evento previo al más reciente de
        /// TBL_CONTROL), insertando un nuevo evento que lo repite. No
        /// comprueba que la tarea esté realmente bloqueada: es
        /// responsabilidad de quien llama asegurarse de que solo se
        /// invoque sobre tareas en ese estado (ver
        /// FrmPoolTareas.btnDesbloquearTarea_Click). No hace nada si
        /// la lista de identificadores está vacía.
        /// </summary>
        public void Desbloquear(IEnumerable<int> idsTarea)
        {
            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlTransaction transaccion = conexion.BeginTransaction())
            {
                try
                {
                    foreach (int idTarea in idsTarea)
                    {
                        InsertarEventoControlRevirtiendoAlAnterior(idTarea, conexion, transaccion);
                    }

                    transaccion.Commit();
                }
                catch
                {
                    RevertirSinPropagar(transaccion);
                    throw;
                }
            }
        }

        /// <summary>
        /// Registra la facturación de una tarea: inserta el registro
        /// de TBL_FACTURADO y, en la misma transacción, el evento de
        /// TBL_CONTROL (estado "FACTURADO") que la referencia. El
        /// usuario asignado se mantiene: se copia del evento más
        /// reciente de la tarea.
        /// </summary>
        public void Facturar(int idTarea, Facturado facturado)
        {
            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlTransaction transaccion = conexion.BeginTransaction())
            {
                try
                {
                    int idFacturado = facturadoRepositorio.Insertar(facturado, conexion, transaccion);

                    int idEstadoFacturado = estadoRepositorio.ObtenerIdPorDescripcion(
                        EstadoFacturado, conexion, transaccion);

                    InsertarEventoControlConUsuarioActual(
                        idTarea, idFacturado, idEstado: idEstadoFacturado,
                        idMotivo: null, conexion, transaccion);

                    transaccion.Commit();
                }
                catch
                {
                    RevertirSinPropagar(transaccion);
                    throw;
                }
            }
        }

        /// <summary>
        /// Inserta la cabecera de una tarea nueva y devuelve el
        /// identificador (identity) generado por la base de datos.
        /// </summary>
        private int InsertarCabecera(Tarea tarea, SqlConnection conexion, SqlTransaction transaccion)
        {
            const string sql = @"
                INSERT INTO dbo.TBL_TAREAS
                (
                    DES_DOC,
                    FEC_ENT_CAL,
                    FEC_REG,
                    DES_ORG_VENTAS,
                    COD_SEQ_PRY,
                    COD_SEQ_SEG,
                    IMP_ESTIMADO
                )
                VALUES
                (
                    @DES_DOC,
                    @FEC_ENT_CAL,
                    @FEC_REG,
                    @DES_ORG_VENTAS,
                    @COD_SEQ_PRY,
                    @COD_SEQ_SEG,
                    @IMP_ESTIMADO
                );

                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlCommand comando = new SqlCommand(sql, conexion, transaccion))
            {
                AgregarParametrosCabecera(comando, tarea);

                return Convert.ToInt32(comando.ExecuteScalar());
            }
        }

        /// <summary>
        /// Actualiza los campos de cabecera de una tarea ya
        /// existente. Lanza excepción si el identificador no existe.
        /// </summary>
        private void ActualizarCabecera(Tarea tarea, SqlConnection conexion, SqlTransaction transaccion)
        {
            const string sql = @"
                UPDATE dbo.TBL_TAREAS
                SET
                    DES_DOC = @DES_DOC,
                    FEC_ENT_CAL = @FEC_ENT_CAL,
                    FEC_REG = @FEC_REG,
                    DES_ORG_VENTAS = @DES_ORG_VENTAS,
                    COD_SEQ_PRY = @COD_SEQ_PRY,
                    COD_SEQ_SEG = @COD_SEQ_SEG,
                    IMP_ESTIMADO = @IMP_ESTIMADO
                WHERE ID = @ID;";

            using (SqlCommand comando = new SqlCommand(sql, conexion, transaccion))
            {
                AgregarParametrosCabecera(comando, tarea);

                comando.Parameters.Add("@ID", SqlDbType.Int).Value = tarea.Id;

                int filasAfectadas = comando.ExecuteNonQuery();

                if (filasAfectadas == 0)
                {
                    throw new InvalidOperationException(
                        "No se encontró la tarea " + tarea.Id + " para actualizar.");
                }
            }
        }

        /// <summary>
        /// Agrega al comando los parámetros comunes a INSERT y UPDATE
        /// de la cabecera de la tarea (todo salvo ID, que depende del
        /// modo insertar/actualizar).
        /// </summary>
        private void AgregarParametrosCabecera(SqlCommand comando, Tarea tarea)
        {
            comando.Parameters.AddWithValue("@DES_DOC", tarea.Documento ?? string.Empty);
            comando.Parameters.AddWithValue("@FEC_ENT_CAL", tarea.FechaEntradaCalidad);
            comando.Parameters.AddWithValue("@FEC_REG", tarea.FechaRegistro);
            comando.Parameters.AddWithValue("@DES_ORG_VENTAS", tarea.OrganizacionVentas ?? string.Empty);
            comando.Parameters.Add("@COD_SEQ_PRY", SqlDbType.Int).Value = tarea.IdProyecto;
            comando.Parameters.Add("@COD_SEQ_SEG", SqlDbType.Int).Value = tarea.IdSegmento;

            comando.Parameters.Add("@IMP_ESTIMADO", SqlDbType.Decimal).Value =
                tarea.ImporteEstimado.HasValue ? (object)tarea.ImporteEstimado.Value : DBNull.Value;
        }

        /// <summary>
        /// Elimina todas las líneas de detalle existentes de una
        /// tarea, como paso previo a volver a insertarlas.
        /// </summary>
        private void EliminarDetalle(int idTarea, SqlConnection conexion, SqlTransaction transaccion)
        {
            const string sql = @"
                DELETE FROM dbo.TBL_DETALLETAREA
                WHERE COD_SEQ_TAR = @COD_SEQ_TAR;";

            using (SqlCommand comando = new SqlCommand(sql, conexion, transaccion))
            {
                comando.Parameters.Add("@COD_SEQ_TAR", SqlDbType.Int).Value = idTarea;

                comando.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Inserta las líneas de detalle indicadas para una tarea,
        /// omitiendo las que estén completamente vacías.
        /// </summary>
        private void InsertarDetalles(
            int idTarea,
            List<DetalleTarea> detalles,
            SqlConnection conexion,
            SqlTransaction transaccion)
        {
            const string sql = @"
                INSERT INTO dbo.TBL_DETALLETAREA
                (
                    COD_SEQ_TAR,
                    COD_PED_VENTA,
                    COD_PED_COMPRA,
                    COD_PED_INSPEC,
                    COD_ENT_ENTR,
                    NBR_UNIDADES
                )
                VALUES
                (
                    @COD_SEQ_TAR,
                    @COD_PED_VENTA,
                    @COD_PED_COMPRA,
                    @COD_PED_INSPEC,
                    @COD_ENT_ENTR,
                    @NBR_UNIDADES
                );";

            foreach (DetalleTarea detalle in detalles)
            {
                if (detalle.EstaVacia())
                    continue;

                using (SqlCommand comando = new SqlCommand(sql, conexion, transaccion))
                {
                    comando.Parameters.Add("@COD_SEQ_TAR", SqlDbType.Int).Value = idTarea;

                    comando.Parameters.Add("@COD_PED_VENTA", SqlDbType.NVarChar).Value =
                        ValorOTextoVacio(detalle.PedidoVenta);

                    comando.Parameters.Add("@COD_PED_COMPRA", SqlDbType.NVarChar).Value =
                        ValorOTextoVacio(detalle.PedidoCompra);

                    comando.Parameters.Add("@COD_PED_INSPEC", SqlDbType.NVarChar).Value =
                        ValorOTextoVacio(detalle.PedidoInspeccion);

                    comando.Parameters.Add("@COD_ENT_ENTR", SqlDbType.NVarChar).Value =
                        ValorOTextoVacio(detalle.EntidadEntrega);

                    comando.Parameters.Add("@NBR_UNIDADES", SqlDbType.Decimal).Value =
                        detalle.Unidades.HasValue ? (object)detalle.Unidades.Value : DBNull.Value;

                    comando.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Inserta un nuevo evento de TBL_CONTROL con los valores
        /// indicados explícitamente.
        /// </summary>
        private void InsertarEventoControl(
            int idTarea,
            int? idUsuario,
            int? idFacturado,
            int idEstado,
            int? idMotivo,
            SqlConnection conexion,
            SqlTransaction transaccion)
        {
            const string sql = @"
                INSERT INTO dbo.TBL_CONTROL
                (
                    COD_SEQ_TAR,
                    COD_SEQ_USER,
                    COD_SEQ_FAC,
                    COD_SEQ_EST,
                    COD_SEQ_MOTIVO
                )
                VALUES
                (
                    @COD_SEQ_TAR,
                    @COD_SEQ_USER,
                    @COD_SEQ_FAC,
                    @COD_SEQ_EST,
                    @COD_SEQ_MOTIVO
                );";

            using (SqlCommand comando = new SqlCommand(sql, conexion, transaccion))
            {
                comando.Parameters.Add("@COD_SEQ_TAR", SqlDbType.Int).Value = idTarea;

                comando.Parameters.Add("@COD_SEQ_USER", SqlDbType.Int).Value =
                    idUsuario.HasValue ? (object)idUsuario.Value : DBNull.Value;

                comando.Parameters.Add("@COD_SEQ_FAC", SqlDbType.Int).Value =
                    idFacturado.HasValue ? (object)idFacturado.Value : DBNull.Value;

                comando.Parameters.Add("@COD_SEQ_EST", SqlDbType.Int).Value = idEstado;

                comando.Parameters.Add("@COD_SEQ_MOTIVO", SqlDbType.Int).Value =
                    idMotivo.HasValue ? (object)idMotivo.Value : DBNull.Value;

                comando.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Inserta un nuevo evento de TBL_CONTROL que repite el
        /// penúltimo evento de la tarea (usuario, factura y estado),
        /// es decir, el que estaba vigente inmediatamente antes del
        /// más reciente. Usado para "deshacer" el bloqueo de una
        /// tarea devolviéndola a su estado anterior. Lanza
        /// <see cref="InvalidOperationException"/> si la tarea no
        /// tiene un evento previo al que volver (no debería ocurrir
        /// en uso normal: toda tarea nace con un evento "Registrado").
        /// </summary>
        private void InsertarEventoControlRevirtiendoAlAnterior(
            int idTarea,
            SqlConnection conexion,
            SqlTransaction transaccion)
        {
            const string sql = @"
                INSERT INTO dbo.TBL_CONTROL
                (
                    COD_SEQ_TAR,
                    COD_SEQ_USER,
                    COD_SEQ_FAC,
                    COD_SEQ_EST,
                    COD_SEQ_MOTIVO
                )
                SELECT
                    @COD_SEQ_TAR,
                    ANTERIOR.COD_SEQ_USER,
                    ANTERIOR.COD_SEQ_FAC,
                    ANTERIOR.COD_SEQ_EST,
                    NULL

                FROM dbo.TBL_CONTROL AS ANTERIOR

                WHERE ANTERIOR.COD_SEQ_TAR = @COD_SEQ_TAR

                ORDER BY ANTERIOR.FEC_ALTA DESC, ANTERIOR.ID DESC
                OFFSET 1 ROWS FETCH NEXT 1 ROWS ONLY;";

            using (SqlCommand comando = new SqlCommand(sql, conexion, transaccion))
            {
                comando.Parameters.Add("@COD_SEQ_TAR", SqlDbType.Int).Value = idTarea;

                int filasAfectadas = comando.ExecuteNonQuery();

                if (filasAfectadas == 0)
                {
                    throw new InvalidOperationException(
                        "No se ha podido determinar el estado anterior de la tarea " + idTarea + ".");
                }
            }
        }

        /// <summary>
        /// Inserta un nuevo evento de TBL_CONTROL copiando el usuario
        /// asignado del evento más reciente de la tarea (si lo hay),
        /// para no perder esa información al bloquear o facturar.
        /// </summary>
        private void InsertarEventoControlConUsuarioActual(
            int idTarea,
            int? idFacturado,
            int idEstado,
            int? idMotivo,
            SqlConnection conexion,
            SqlTransaction transaccion)
        {
            const string sql = @"
                INSERT INTO dbo.TBL_CONTROL
                (
                    COD_SEQ_TAR,
                    COD_SEQ_USER,
                    COD_SEQ_FAC,
                    COD_SEQ_EST,
                    COD_SEQ_MOTIVO
                )
                VALUES
                (
                    @COD_SEQ_TAR,
                    (
                        SELECT TOP (1) COD_SEQ_USER
                        FROM dbo.TBL_CONTROL
                        WHERE COD_SEQ_TAR = @COD_SEQ_TAR
                        ORDER BY FEC_ALTA DESC, ID DESC
                    ),
                    @COD_SEQ_FAC,
                    @COD_SEQ_EST,
                    @COD_SEQ_MOTIVO
                );";

            using (SqlCommand comando = new SqlCommand(sql, conexion, transaccion))
            {
                comando.Parameters.Add("@COD_SEQ_TAR", SqlDbType.Int).Value = idTarea;

                comando.Parameters.Add("@COD_SEQ_FAC", SqlDbType.Int).Value =
                    idFacturado.HasValue ? (object)idFacturado.Value : DBNull.Value;

                comando.Parameters.Add("@COD_SEQ_EST", SqlDbType.Int).Value = idEstado;

                comando.Parameters.Add("@COD_SEQ_MOTIVO", SqlDbType.Int).Value =
                    idMotivo.HasValue ? (object)idMotivo.Value : DBNull.Value;

                comando.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Convierte una cadena nula o en blanco en DBNull para
        /// enviarla a base de datos como NULL; en caso contrario
        /// devuelve el propio texto.
        /// </summary>
        private object ValorOTextoVacio(string texto)
        {
            return string.IsNullOrWhiteSpace(texto) ? (object)DBNull.Value : texto;
        }

        /// <summary>
        /// Lee una columna de texto de un SqlDataReader, devolviendo
        /// null si el valor es DBNull.
        /// </summary>
        private string ObtenerTextoONulo(SqlDataReader lector, string nombreColumna)
        {
            return lector[nombreColumna] == DBNull.Value ? null : lector[nombreColumna].ToString();
        }

        /// <summary>
        /// Intenta deshacer la transacción sin propagar un posible
        /// error del propio rollback, para no ocultar la excepción
        /// original que provocó el fallo.
        /// </summary>
        private void RevertirSinPropagar(SqlTransaction transaccion)
        {
            try
            {
                transaccion.Rollback();
            }
            catch
            {
                // Se ignora: ya se va a relanzar la excepción original.
            }
        }
    }
}
