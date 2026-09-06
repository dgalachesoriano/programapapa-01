using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GestionFacturas.Servicios
{
    /// <summary>
    /// Servicio encargado de interpretar texto tabular (por ejemplo,
    /// copiado desde una hoja de Excel externa) y volcarlo sobre un
    /// DataGridView, fila a fila y columna a columna, añadiendo filas
    /// nuevas si hacen falta. No depende de ningún formulario
    /// concreto: cualquier pantalla que necesite esta misma función
    /// de "pegado especial" puede reutilizarlo, pasándole el grid
    /// destino en lugar de estar acoplado a uno en particular.
    /// </summary>
    internal class PegadoPortapapelesServicio
    {
        /// <summary>
        /// Pega el texto indicado sobre <paramref name="grid"/>,
        /// comenzando en su celda actualmente seleccionada. No
        /// accede al portapapeles del sistema: recibe el texto ya
        /// obtenido, para no acoplar el servicio a la API de UI
        /// <c>Clipboard</c> y poder probarlo de forma aislada.
        /// </summary>
        /// <param name="grid">Rejilla destino del pegado.</param>
        /// <param name="texto">Texto a interpretar (habitualmente el contenido del portapapeles).</param>
        /// <param name="columnaDecimal">
        /// Índice de columna cuyo contenido debe interpretarse como
        /// número decimal en lugar de texto libre; -1 si ninguna
        /// columna debe tratarse como numérica.
        /// </param>
        /// <param name="culturaDecimal">
        /// Cultura usada para interpretar los valores numéricos de
        /// <paramref name="columnaDecimal"/>.
        /// </param>
        public ResultadoPegadoPortapapeles Pegar(
            DataGridView grid,
            string texto,
            int columnaDecimal,
            CultureInfo culturaDecimal)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return ResultadoPegadoPortapapeles.SinTexto;

            if (grid.CurrentCell == null)
                return ResultadoPegadoPortapapeles.SinCeldaSeleccionada;

            string[] filas = DividirEnFilas(texto);

            if (filas.Length == 0)
                return ResultadoPegadoPortapapeles.SinTexto;

            int filaInicial = grid.CurrentCell.RowIndex;
            int columnaInicial = grid.CurrentCell.ColumnIndex;

            for (int f = 0; f < filas.Length; f++)
            {
                string[] valores = DividirEnColumnas(filas[f]);

                int filaDestino = filaInicial + f;

                // Si necesitamos una nueva fila, la añadimos.
                if (filaDestino >= grid.Rows.Count - 1)
                {
                    grid.Rows.Add();
                }

                for (int c = 0; c < valores.Length; c++)
                {
                    int columnaDestino = columnaInicial + c;

                    // No nos salimos de las columnas.
                    if (columnaDestino >= grid.ColumnCount)
                        break;

                    AsignarCelda(
                        grid,
                        filaDestino,
                        columnaDestino,
                        valores[c].Trim(),
                        columnaDecimal,
                        culturaDecimal);
                }
            }

            return ResultadoPegadoPortapapeles.Correcto;
        }

        /// <summary>
        /// Normaliza los distintos tipos de salto de línea (\r\n,
        /// \r, \n) y separa el texto en filas, descartando las
        /// líneas vacías.
        /// </summary>
        private string[] DividirEnFilas(string texto)
        {
            string normalizado = texto
                .Replace("\r\n", "\n")
                .Replace("\r", "\n");

            return normalizado.Split(
                new[] { '\n' },
                StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Separa una fila de texto en columnas: por tabuladores si
        /// los contiene (formato típico al copiar desde Excel), o
        /// por bloques de dos o más espacios en caso contrario.
        /// </summary>
        private string[] DividirEnColumnas(string fila)
        {
            if (fila.Contains("\t"))
                return fila.Split('\t');

            return Regex.Split(fila.Trim(), @"\s{2,}");
        }

        /// <summary>
        /// Asigna un valor a una celda del grid: lo interpreta como
        /// decimal si la columna destino coincide con la columna
        /// numérica configurada y el texto es un número válido; en
        /// cualquier otro caso, lo asigna tal cual como texto.
        /// </summary>
        private void AsignarCelda(
            DataGridView grid,
            int fila,
            int columna,
            string valor,
            int columnaDecimal,
            CultureInfo culturaDecimal)
        {
            if (columna == columnaDecimal)
            {
                decimal valorDecimal;

                if (decimal.TryParse(
                    valor,
                    NumberStyles.Number,
                    culturaDecimal,
                    out valorDecimal))
                {
                    grid.Rows[fila].Cells[columna].Value = valorDecimal;
                    return;
                }
            }

            grid.Rows[fila].Cells[columna].Value = valor;
        }
    }
}
