using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GestionFacturas.Modelos;

namespace GestionFacturas.Datos
{
    /// <summary>
    /// Acceso a datos para las facturas/tareas y sus líneas de
    /// detalle (tablas Facturas y Detalle_Facturas). Concentra el
    /// SQL y la gestión de transacciones que antes vivía repartida
    /// en los formularios.
    /// </summary>
    internal class FacturaRepositorio
    {
        private readonly EstadoFacturaRepositorio estadoRepositorio =
            new EstadoFacturaRepositorio();

        /// <summary>
        /// Nombre del estado inicial que recibe toda factura nueva.
        /// </summary>
        private const string EstadoInicial = "Registrado";

        /// <summary>
        /// Busca facturas aplicando el filtro indicado y devuelve el
        /// resultado como DataTable, listo para enlazar directamente
        /// al DataGridView de resultados (columnas ya con los alias
        /// usados por FrmTratarTarea.FormatearGrid).
        /// </summary>
        public DataTable BuscarPorFiltro(FiltroBusquedaFacturas filtro)
        {
            DataTable tabla = new DataTable();

            const string sql = @"
                SELECT
                    F.Registro,
                    F.Documento,
                    F.F_Ent_Calidad,
                    F.F_Registro,
                    F.Sociedad,
                    F.Proyecto,
                    SN.Segmento,
                    E.Estado,
                    U.Nombre AS Usuario,

                    F.Importe_Estimado,
                    F.Num_Factura,
                    F.F_Factura,
                    F.Importe_Total

                FROM Facturas AS F

                LEFT JOIN Estados_Facturas AS E
                    ON F.IdEstado = E.IdEstado

                LEFT JOIN Usuarios_Facturas AS U
                    ON F.IdUsuarioAsignado = U.IdUsuario

                LEFT JOIN Segmento_Negocio AS SN
                    ON F.IdSegmento = SN.IdSegmento

                WHERE
                    (@IdEstado = 0
                     OR F.IdEstado = @IdEstado)

                    AND

                    (
                        @Documento = ''
                        OR F.Documento LIKE '%' + @Documento + '%'
                    )

                    AND

                    (
                        @Proyecto = ''
                        OR F.Proyecto LIKE '%' + @Proyecto + '%'
                    )

                    AND

                    (
                        @IdUsuario = 0
                        OR F.IdUsuarioAsignado = @IdUsuario
                    )

                    AND

                    (
                        @FechaDesde IS NULL
                        OR F.F_Registro >= @FechaDesde
                    )

                    AND

                    (
                        @FechaHasta IS NULL
                        OR F.F_Registro < DATEADD(day, 1, @FechaHasta)
                    )

                ORDER BY
                    F.Registro DESC;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@IdEstado", SqlDbType.Int).Value =
                    filtro.IdEstado;

                comando.Parameters.Add("@IdUsuario", SqlDbType.Int).Value =
                    filtro.IdUsuario;

                comando.Parameters.Add("@Documento", SqlDbType.NVarChar).Value =
                    filtro.Documento ?? string.Empty;

                comando.Parameters.Add("@Proyecto", SqlDbType.NVarChar).Value =
                    filtro.Proyecto ?? string.Empty;

                comando.Parameters.Add("@FechaDesde", SqlDbType.Date).Value =
                    filtro.FechaDesde.HasValue
                        ? (object)filtro.FechaDesde.Value.Date
                        : DBNull.Value;

                comando.Parameters.Add("@FechaHasta", SqlDbType.Date).Value =
                    filtro.FechaHasta.HasValue
                        ? (object)filtro.FechaHasta.Value.Date
                        : DBNull.Value;

                using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }

        /// <summary>
        /// Obtiene la cabecera de una factura por su número de
        /// registro, o null si no existe.
        /// </summary>
        public Factura ObtenerPorRegistro(int registro)
        {
            const string sql = @"
                SELECT
                    F.Registro,
                    F.Documento,
                    F.F_Ent_Calidad,
                    F.F_Registro,
                    F.Sociedad,
                    F.Proyecto,
                    F.Importe_Estimado,
                    F.IdUsuarioAsignado,
                    F.IdSegmento

                FROM Facturas AS F

                WHERE F.Registro = @Registro;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@Registro", SqlDbType.Int).Value = registro;

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    if (!lector.Read())
                        return null;

                    return new Factura
                    {
                        Registro = Convert.ToInt32(lector["Registro"]),
                        Documento = lector["Documento"].ToString(),

                        FechaEntradaCalidad =
                            lector["F_Ent_Calidad"] == DBNull.Value
                                ? default(DateTime)
                                : Convert.ToDateTime(lector["F_Ent_Calidad"]),

                        FechaRegistro =
                            lector["F_Registro"] == DBNull.Value
                                ? default(DateTime)
                                : Convert.ToDateTime(lector["F_Registro"]),

                        Sociedad = lector["Sociedad"].ToString(),
                        Proyecto = lector["Proyecto"].ToString(),

                        ImporteEstimado =
                            lector["Importe_Estimado"] == DBNull.Value
                                ? (decimal?)null
                                : Convert.ToDecimal(lector["Importe_Estimado"]),

                        IdUsuarioAsignado =
                            lector["IdUsuarioAsignado"] == DBNull.Value
                                ? (int?)null
                                : Convert.ToInt32(lector["IdUsuarioAsignado"]),

                        IdSegmento =
                            lector["IdSegmento"] == DBNull.Value
                                ? 0
                                : Convert.ToInt32(lector["IdSegmento"])
                    };
                }
            }
        }

        /// <summary>
        /// Obtiene las líneas de detalle de una factura, en el
        /// mismo orden en que fueron registradas.
        /// </summary>
        public List<DetalleFactura> ObtenerDetalle(int registro)
        {
            List<DetalleFactura> detalles = new List<DetalleFactura>();

            const string sql = @"
                SELECT
                    RC,
                    Unidades,
                    PInspeccion,
                    PCompra,
                    PVenta,
                    Albaran

                FROM Detalle_Facturas

                WHERE RegistroRN = @Registro

                ORDER BY RegistroDetalle;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@Registro", SqlDbType.Int).Value = registro;

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        detalles.Add(new DetalleFactura
                        {
                            RC = lector["RC"] == DBNull.Value
                                ? null
                                : lector["RC"].ToString(),

                            Unidades = lector["Unidades"] == DBNull.Value
                                ? (decimal?)null
                                : Convert.ToDecimal(lector["Unidades"]),

                            PInspeccion = lector["PInspeccion"] == DBNull.Value
                                ? null
                                : lector["PInspeccion"].ToString(),

                            PCompra = lector["PCompra"] == DBNull.Value
                                ? null
                                : lector["PCompra"].ToString(),

                            PVenta = lector["PVenta"] == DBNull.Value
                                ? null
                                : lector["PVenta"].ToString(),

                            Albaran = lector["Albaran"] == DBNull.Value
                                ? null
                                : lector["Albaran"].ToString()
                        });
                    }
                }
            }

            return detalles;
        }

        /// <summary>
        /// Obtiene, ordenados alfabéticamente, los nombres de proyecto
        /// distintos que ya tienen alguna factura registrada. Se usa
        /// para poblar el combo de filtro por proyecto en el pool de
        /// tareas (no existe una tabla de proyectos independiente).
        /// </summary>
        public List<string> ObtenerProyectosDistintos()
        {
            List<string> proyectos = new List<string>();

            const string sql = @"
                SELECT DISTINCT Proyecto
                FROM Facturas
                WHERE Proyecto IS NOT NULL AND Proyecto <> ''
                ORDER BY Proyecto;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            using (SqlDataReader lector = comando.ExecuteReader())
            {
                while (lector.Read())
                {
                    proyectos.Add(lector.GetString(0));
                }
            }

            return proyectos;
        }

        /// <summary>
        /// Asigna (o desasigna, si <paramref name="idUsuario"/> es
        /// null) de una sola vez el usuario indicado a todas las
        /// facturas cuyo registro esté en <paramref name="registros"/>.
        /// Pensado para la asignación masiva desde el pool de tareas.
        /// No hace nada si la lista de registros está vacía.
        /// </summary>
        public void AsignarUsuario(IEnumerable<int> registros, int? idUsuario)
        {
            const string sql = @"
                UPDATE Facturas
                SET IdUsuarioAsignado = @IdUsuarioAsignado
                WHERE Registro = @Registro;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlTransaction transaccion = conexion.BeginTransaction())
            {
                try
                {
                    foreach (int registro in registros)
                    {
                        using (SqlCommand comando = new SqlCommand(sql, conexion, transaccion))
                        {
                            comando.Parameters.Add("@IdUsuarioAsignado", SqlDbType.Int).Value =
                                idUsuario.HasValue ? (object)idUsuario.Value : DBNull.Value;

                            comando.Parameters.Add("@Registro", SqlDbType.Int).Value = registro;

                            comando.ExecuteNonQuery();
                        }
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
        /// Inserta una nueva factura junto con sus líneas de detalle,
        /// en una única transacción. La factura se crea con el
        /// estado inicial "Registrado". Devuelve el número de
        /// registro generado.
        /// </summary>
        public int Insertar(Factura factura, List<DetalleFactura> detalles)
        {
            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            {
                using (SqlTransaction transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        int idEstadoRegistrado = estadoRepositorio.ObtenerIdPorNombre(
                            EstadoInicial,
                            conexion,
                            transaccion);

                        int nuevoRegistro = InsertarCabecera(
                            factura,
                            idEstadoRegistrado,
                            conexion,
                            transaccion);

                        InsertarDetalles(nuevoRegistro, detalles, conexion, transaccion);

                        transaccion.Commit();

                        return nuevoRegistro;
                    }
                    catch
                    {
                        RevertirSinPropagar(transaccion);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Actualiza la cabecera de una factura existente y sustituye
        /// por completo sus líneas de detalle, en una única
        /// transacción.
        /// </summary>
        public void Actualizar(Factura factura, List<DetalleFactura> detalles)
        {
            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            {
                using (SqlTransaction transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        ActualizarCabecera(factura, conexion, transaccion);

                        EliminarDetalle(factura.Registro, conexion, transaccion);

                        InsertarDetalles(factura.Registro, detalles, conexion, transaccion);

                        transaccion.Commit();
                    }
                    catch
                    {
                        RevertirSinPropagar(transaccion);
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Inserta la cabecera de una factura nueva y devuelve el
        /// número de registro (identidad) generado por la base de datos.
        /// </summary>
        private int InsertarCabecera(
            Factura factura,
            int idEstado,
            SqlConnection conexion,
            SqlTransaction transaccion)
        {
            const string sql = @"
                INSERT INTO Facturas
                (
                    Documento,
                    F_Ent_Calidad,
                    F_Registro,
                    Sociedad,
                    Proyecto,
                    Importe_Estimado,
                    IdUsuarioAsignado,
                    IdEstado,
                    IdSegmento
                )
                VALUES
                (
                    @Documento,
                    @F_Ent_Calidad,
                    @F_Registro,
                    @Sociedad,
                    @Proyecto,
                    @Importe_Estimado,
                    @IdUsuarioAsignado,
                    @IdEstado,
                    @IdSegmento
                );

                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlCommand comando = new SqlCommand(sql, conexion, transaccion))
            {
                AgregarParametrosCabecera(comando, factura);

                comando.Parameters.Add("@IdEstado", SqlDbType.Int).Value = idEstado;

                return Convert.ToInt32(comando.ExecuteScalar());
            }
        }

        /// <summary>
        /// Actualiza los campos de cabecera de una factura ya
        /// existente. Lanza excepción si el registro no existe.
        /// </summary>
        private void ActualizarCabecera(
            Factura factura,
            SqlConnection conexion,
            SqlTransaction transaccion)
        {
            const string sql = @"
                UPDATE Facturas
                SET
                    Documento = @Documento,
                    F_Ent_Calidad = @F_Ent_Calidad,
                    F_Registro = @F_Registro,
                    Sociedad = @Sociedad,
                    Proyecto = @Proyecto,
                    Importe_Estimado = @Importe_Estimado,
                    IdUsuarioAsignado = @IdUsuarioAsignado,
                    IdSegmento = @IdSegmento
                WHERE Registro = @Registro;";

            using (SqlCommand comando = new SqlCommand(sql, conexion, transaccion))
            {
                AgregarParametrosCabecera(comando, factura);

                comando.Parameters.Add("@Registro", SqlDbType.Int).Value =
                    factura.Registro;

                int filasAfectadas = comando.ExecuteNonQuery();

                if (filasAfectadas == 0)
                {
                    throw new InvalidOperationException(
                        "No se encontró el registro " + factura.Registro +
                        " para actualizar.");
                }
            }
        }

        /// <summary>
        /// Agrega al comando los parámetros comunes a INSERT y UPDATE
        /// de la cabecera de la factura (todo salvo IdEstado/Registro,
        /// que dependen del modo insertar/actualizar).
        /// </summary>
        private void AgregarParametrosCabecera(SqlCommand comando, Factura factura)
        {
            comando.Parameters.AddWithValue("@Documento", factura.Documento ?? string.Empty);
            comando.Parameters.AddWithValue("@F_Ent_Calidad", factura.FechaEntradaCalidad);
            comando.Parameters.AddWithValue("@F_Registro", factura.FechaRegistro);
            comando.Parameters.AddWithValue("@Sociedad", factura.Sociedad ?? string.Empty);
            comando.Parameters.AddWithValue("@Proyecto", factura.Proyecto ?? string.Empty);

            comando.Parameters.Add("@Importe_Estimado", SqlDbType.Decimal).Value =
                factura.ImporteEstimado.HasValue
                    ? (object)factura.ImporteEstimado.Value
                    : DBNull.Value;

            comando.Parameters.Add("@IdUsuarioAsignado", SqlDbType.Int).Value =
                factura.IdUsuarioAsignado.HasValue
                    ? (object)factura.IdUsuarioAsignado.Value
                    : DBNull.Value;

            comando.Parameters.Add("@IdSegmento", SqlDbType.Int).Value =
                factura.IdSegmento;
        }

        /// <summary>
        /// Elimina todas las líneas de detalle existentes de una
        /// factura, como paso previo a volver a insertarlas.
        /// </summary>
        private void EliminarDetalle(
            int registro,
            SqlConnection conexion,
            SqlTransaction transaccion)
        {
            const string sql = @"
                DELETE FROM Detalle_Facturas
                WHERE RegistroRN = @RegistroRN;";

            using (SqlCommand comando = new SqlCommand(sql, conexion, transaccion))
            {
                comando.Parameters.Add("@RegistroRN", SqlDbType.Int).Value = registro;

                comando.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Inserta las líneas de detalle indicadas para una factura,
        /// omitiendo las que estén completamente vacías.
        /// </summary>
        private void InsertarDetalles(
            int registro,
            List<DetalleFactura> detalles,
            SqlConnection conexion,
            SqlTransaction transaccion)
        {
            const string sql = @"
                INSERT INTO Detalle_Facturas
                (
                    RegistroRN,
                    RC,
                    Unidades,
                    PInspeccion,
                    PCompra,
                    PVenta,
                    Albaran
                )
                VALUES
                (
                    @RegistroRN,
                    @RC,
                    @Unidades,
                    @PInspeccion,
                    @PCompra,
                    @PVenta,
                    @Albaran
                );";

            foreach (DetalleFactura detalle in detalles)
            {
                if (detalle.EstaVacia())
                    continue;

                using (SqlCommand comando = new SqlCommand(sql, conexion, transaccion))
                {
                    comando.Parameters.Add("@RegistroRN", SqlDbType.Int).Value = registro;

                    comando.Parameters.Add("@RC", SqlDbType.NVarChar).Value =
                        ValorOTextoVacio(detalle.RC);

                    comando.Parameters.Add("@Unidades", SqlDbType.Decimal).Value =
                        detalle.Unidades.HasValue
                            ? (object)detalle.Unidades.Value
                            : DBNull.Value;

                    comando.Parameters.Add("@PInspeccion", SqlDbType.NVarChar).Value =
                        ValorOTextoVacio(detalle.PInspeccion);

                    comando.Parameters.Add("@PCompra", SqlDbType.NVarChar).Value =
                        ValorOTextoVacio(detalle.PCompra);

                    comando.Parameters.Add("@PVenta", SqlDbType.NVarChar).Value =
                        ValorOTextoVacio(detalle.PVenta);

                    comando.Parameters.Add("@Albaran", SqlDbType.NVarChar).Value =
                        ValorOTextoVacio(detalle.Albaran);

                    comando.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Convierte una cadena nula o en blanco en DBNull para
        /// enviarla a base de datos como NULL; en caso contrario
        /// devuelve el propio texto.
        /// </summary>
        private object ValorOTextoVacio(string texto)
        {
            return string.IsNullOrWhiteSpace(texto)
                ? (object)DBNull.Value
                : texto;
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
