using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GestionFacturas.Modelos;
using GestionFacturas.Servicios;

namespace GestionFacturas.Datos
{
    /// <summary>
    /// Acceso a datos para las consultas SQL predefinidas (tabla
    /// TBL_CONSULTAS) y para la ejecución de las mismas desde la
    /// pantalla Datos > Consultas SQL.
    /// </summary>
    internal class ConsultaRepositorio
    {
        /// <summary>
        /// Tiempo máximo (segundos) de ejecución de una consulta. Más
        /// alto que el valor por defecto de ADO.NET (30 s) porque la
        /// exportación lanza la consulta completa, sin límite de filas.
        /// </summary>
        private const int TiempoMaximoEjecucion = 300;

        /// <summary>
        /// Obtiene las consultas activas (XTI_ACTIVO = 'S'), ordenadas
        /// alfabéticamente por nombre.
        /// </summary>
        public List<Consulta> ObtenerActivas()
        {
            List<Consulta> consultas = new List<Consulta>();

            const string sql = @"
                SELECT
                    ID,
                    NOM_CONSULTA,
                    DES_CONSULTA,
                    TXT_CONSULTA
                FROM dbo.TBL_CONSULTAS
                WHERE XTI_ACTIVO = 'S'
                ORDER BY NOM_CONSULTA;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            using (SqlDataReader lector = comando.ExecuteReader())
            {
                while (lector.Read())
                {
                    consultas.Add(new Consulta
                    {
                        Id = Convert.ToInt32(lector["ID"]),
                        Nombre = lector["NOM_CONSULTA"].ToString(),
                        Descripcion = lector["DES_CONSULTA"] == DBNull.Value
                            ? string.Empty
                            : lector["DES_CONSULTA"].ToString(),
                        TextoSql = lector["TXT_CONSULTA"].ToString(),
                        Activo = true
                    });
                }
            }

            return consultas;
        }

        /// <summary>
        /// Ejecuta la consulta y devuelve como mucho
        /// <paramref name="maximoFilas"/> filas (null = todas).
        /// <paramref name="valoresParametros"/> asocia cada nombre de
        /// parámetro (sin ":") con el valor tecleado; un valor vacío
        /// se envía como NULL. Al alcanzar el máximo se cancela la
        /// ejecución en el servidor, para que la previsualización de
        /// una consulta grande no tenga que esperar a leerla entera.
        /// </summary>
        public DataTable Ejecutar(Consulta consulta, IDictionary<string, string> valoresParametros, int? maximoFilas)
        {
            string sql = ParametrosConsultaServicio.PrepararParaEjecucion(consulta.TextoSql);

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.CommandTimeout = TiempoMaximoEjecucion;

                foreach (string nombre in ParametrosConsultaServicio.ExtraerParametros(consulta.TextoSql))
                {
                    string valor;
                    valoresParametros.TryGetValue(nombre, out valor);

                    comando.Parameters.Add(ParametrosConsultaServicio.NombreParametroSql(nombre), SqlDbType.NVarChar, 4000).Value =
                        string.IsNullOrEmpty(valor) ? (object)DBNull.Value : valor;
                }

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    DataTable tabla = CrearTablaDesdeEsquema(lector);

                    object[] valores = new object[lector.FieldCount];

                    while ((!maximoFilas.HasValue || tabla.Rows.Count < maximoFilas.Value) && lector.Read())
                    {
                        lector.GetValues(valores);
                        tabla.Rows.Add(valores);
                    }

                    if (maximoFilas.HasValue)
                        comando.Cancel();

                    return tabla;
                }
            }
        }

        /// <summary>
        /// Crea una DataTable vacía con una columna por cada columna
        /// del resultado. Las columnas sin nombre (p. ej. COUNT(*) sin
        /// alias) o con nombre repetido (SELECT A.ID, B.ID) se
        /// renombran para que la DataTable las admita.
        /// </summary>
        private static DataTable CrearTablaDesdeEsquema(SqlDataReader lector)
        {
            DataTable tabla = new DataTable();

            for (int i = 0; i < lector.FieldCount; i++)
            {
                string nombreBase = lector.GetName(i);

                if (string.IsNullOrWhiteSpace(nombreBase))
                    nombreBase = "Columna" + (i + 1);

                string nombre = nombreBase;
                int sufijo = 2;

                while (tabla.Columns.Contains(nombre))
                {
                    nombre = nombreBase + "_" + sufijo;
                    sufijo++;
                }

                tabla.Columns.Add(nombre, lector.GetFieldType(i));
            }

            return tabla;
        }
    }
}
