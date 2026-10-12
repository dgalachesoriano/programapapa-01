using System;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace GestionFacturas.Servicios
{
    /// <summary>
    /// Exporta una DataTable a una hoja nueva de Microsoft Excel con
    /// formato de Tabla (ListObject: cabecera con filtros, filas
    /// bandeadas), y deja Excel abierto para que el usuario la revise
    /// y la guarde donde quiera.
    ///
    /// Se automatiza el Excel instalado en el equipo mediante COM con
    /// enlace tardío (dynamic), en vez de referenciar los ensamblados
    /// de interoperabilidad de Office: así el proyecto no depende de
    /// una versión concreta de Office ni de paquetes adicionales. A
    /// cambio, el equipo necesita tener Excel instalado; si no lo
    /// está se lanza <see cref="InvalidOperationException"/> con un
    /// mensaje claro.
    /// </summary>
    internal static class ExportadorExcel
    {
        // Constantes de la API de Excel (enumeraciones XlListObjectSourceType y XlYesNoGuess).
        private const int XlSrcRange = 1;
        private const int XlYes = 1;

        /// <summary>Máximo de filas de datos de una hoja (1.048.576 menos la cabecera).</summary>
        private const int MaximoFilasDatos = 1048575;

        private const string FormatoTexto = "@";
        private const string FormatoFecha = "dd/mm/yyyy";
        private const string FormatoFechaHora = "dd/mm/yyyy hh:mm:ss";
        private const string FormatoDecimal = "#,##0.00";

        /// <summary>
        /// Abre Excel, crea un libro nuevo y vuelca
        /// <paramref name="tabla"/> como Tabla en su primera hoja,
        /// que se llamará <paramref name="nombreHoja"/> (ajustado a
        /// las restricciones de nombre de Excel).
        /// </summary>
        public static void Exportar(DataTable tabla, string nombreHoja)
        {
            if (tabla.Columns.Count == 0)
                throw new InvalidOperationException("La consulta no ha devuelto ninguna columna que exportar.");

            if (tabla.Rows.Count > MaximoFilasDatos)
            {
                throw new InvalidOperationException(
                    "La consulta devuelve " + tabla.Rows.Count.ToString("N0") +
                    " filas y una hoja de Excel admite como máximo " +
                    MaximoFilasDatos.ToString("N0") + ".");
            }

            Type tipoExcel = Type.GetTypeFromProgID("Excel.Application");

            if (tipoExcel == null)
            {
                throw new InvalidOperationException(
                    "No se ha encontrado Microsoft Excel en este equipo. " +
                    "Es necesario tenerlo instalado para poder exportar.");
            }

            dynamic excel = Activator.CreateInstance(tipoExcel);
            bool entregadoAlUsuario = false;

            try
            {
                excel.ScreenUpdating = false;

                VolcarEnLibroNuevo(excel, tabla, nombreHoja);

                excel.ScreenUpdating = true;
                excel.Visible = true;
                excel.UserControl = true;

                entregadoAlUsuario = true;
            }
            finally
            {
                if (!entregadoAlUsuario)
                {
                    try
                    {
                        excel.DisplayAlerts = false;
                        excel.Quit();
                    }
                    catch (COMException)
                    {
                        // Excel ya no responde: no hay nada más que cerrar.
                    }
                }

                // Suelta las referencias COM que quedan en este hilo para
                // que el proceso EXCEL.EXE termine cuando el usuario lo
                // cierre (o ahora mismo, si la exportación ha fallado).
                Marshal.FinalReleaseComObject(excel);
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// Crea el libro y vuelca la tabla. Va en un método aparte para
        /// que las referencias COM intermedias (libro, hoja, rango...)
        /// queden fuera de ámbito antes de liberar Excel en
        /// <see cref="Exportar"/>.
        /// </summary>
        private static void VolcarEnLibroNuevo(dynamic excel, DataTable tabla, string nombreHoja)
        {
            dynamic libro = excel.Workbooks.Add();
            dynamic hoja = libro.Worksheets[1];

            hoja.Name = NormalizarNombreHoja(nombreHoja);

            int numFilas = tabla.Rows.Count + 1;
            int numColumnas = tabla.Columns.Count;

            AplicarFormatosColumnas(hoja, tabla, numFilas);

            dynamic rango = hoja.Range[hoja.Cells[1, 1], hoja.Cells[numFilas, numColumnas]];
            rango.Value2 = ConstruirMatrizValores(tabla);

            dynamic tablaExcel = hoja.ListObjects.Add(XlSrcRange, rango, Missing.Value, XlYes);
            tablaExcel.TableStyle = "TableStyleMedium2";

            hoja.Columns.AutoFit();
        }

        /// <summary>
        /// Da formato a cada columna según su tipo de datos, antes de
        /// volcar los valores: texto ("@", para que Excel no convierta
        /// "000123" en 123 ni interprete como fórmula un valor que
        /// empiece por "="), fecha y decimal. La fila de cabecera se
        /// fuerza a texto por el mismo motivo.
        /// </summary>
        private static void AplicarFormatosColumnas(dynamic hoja, DataTable tabla, int numFilas)
        {
            hoja.Range[hoja.Cells[1, 1], hoja.Cells[1, tabla.Columns.Count]].NumberFormat = FormatoTexto;

            if (numFilas < 2)
                return;

            for (int c = 0; c < tabla.Columns.Count; c++)
            {
                string formato = FormatoParaColumna(tabla, tabla.Columns[c]);

                if (formato != null)
                    hoja.Range[hoja.Cells[2, c + 1], hoja.Cells[numFilas, c + 1]].NumberFormat = formato;
            }
        }

        private static string FormatoParaColumna(DataTable tabla, DataColumn columna)
        {
            Type tipo = columna.DataType;

            if (tipo == typeof(string) || tipo == typeof(Guid))
                return FormatoTexto;

            if (tipo == typeof(DateTime) || tipo == typeof(DateTimeOffset))
            {
                bool tieneHora = tabla.Rows
                    .Cast<DataRow>()
                    .Where(f => f[columna] != DBNull.Value)
                    .Any(f => ADateTime(f[columna]).TimeOfDay != TimeSpan.Zero);

                return tieneHora ? FormatoFechaHora : FormatoFecha;
            }

            if (tipo == typeof(decimal) || tipo == typeof(double) || tipo == typeof(float))
                return FormatoDecimal;

            return null;
        }

        /// <summary>
        /// Construye la matriz (base 1, como espera Excel) con la
        /// cabecera y todos los valores, para volcarla de una sola
        /// vez: escribir celda a celda por COM es órdenes de magnitud
        /// más lento.
        /// </summary>
        private static object[,] ConstruirMatrizValores(DataTable tabla)
        {
            int numFilas = tabla.Rows.Count + 1;
            int numColumnas = tabla.Columns.Count;

            object[,] valores = (object[,])Array.CreateInstance(
                typeof(object), new[] { numFilas, numColumnas }, new[] { 1, 1 });

            for (int c = 0; c < numColumnas; c++)
                valores[1, c + 1] = tabla.Columns[c].ColumnName;

            for (int f = 0; f < tabla.Rows.Count; f++)
            {
                DataRow fila = tabla.Rows[f];

                for (int c = 0; c < numColumnas; c++)
                    valores[f + 2, c + 1] = ConvertirValor(fila[c]);
            }

            return valores;
        }

        /// <summary>
        /// Convierte un valor de la DataTable a uno que Excel entienda
        /// vía COM: fechas como número de serie OLE (con el formato de
        /// fecha ya aplicado a la columna), numéricos como double y el
        /// resto como texto.
        /// </summary>
        private static object ConvertirValor(object valor)
        {
            if (valor == null || valor == DBNull.Value)
                return null;

            if (valor is DateTime || valor is DateTimeOffset)
                return ADateTime(valor).ToOADate();

            if (valor is TimeSpan)
                return ((TimeSpan)valor).ToString();

            if (valor is bool)
                return valor;

            if (valor is byte[])
                return "(binario)";

            if (valor is byte || valor is short || valor is int || valor is long ||
                valor is decimal || valor is double || valor is float)
            {
                return Convert.ToDouble(valor);
            }

            return valor.ToString();
        }

        private static DateTime ADateTime(object valor)
        {
            return valor is DateTimeOffset ? ((DateTimeOffset)valor).DateTime : (DateTime)valor;
        }

        /// <summary>
        /// Ajusta un texto a las reglas de nombre de hoja de Excel:
        /// máximo 31 caracteres y sin \ / ? * [ ] :
        /// </summary>
        private static string NormalizarNombreHoja(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return "Consulta";

            char[] prohibidos = { '\\', '/', '?', '*', '[', ']', ':' };

            string limpio = new string(nombre.Select(c => prohibidos.Contains(c) ? '_' : c).ToArray()).Trim('\'', ' ');

            if (limpio.Length > 31)
                limpio = limpio.Substring(0, 31);

            return limpio.Length == 0 ? "Consulta" : limpio;
        }
    }
}
