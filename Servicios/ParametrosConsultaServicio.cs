using System;
using System.Collections.Generic;
using System.Text;

namespace GestionFacturas.Servicios
{
    /// <summary>
    /// Tratamiento de los parámetros ":Nombre" de las consultas de
    /// TBL_CONSULTAS. El usuario escribe los parámetros con dos
    /// puntos (más cómodo y sin chocar con variables @ que la propia
    /// consulta pueda declarar); antes de ejecutar se convierten a
    /// parámetros de ADO.NET (@Nombre) para que los valores viajen
    /// como SqlParameter y nunca concatenados al texto SQL.
    ///
    /// Los ":" que aparecen dentro de literales ('10:30'),
    /// identificadores entre corchetes o comillas dobles y
    /// comentarios (-- y /* */) se ignoran, igual que los "::" de
    /// los métodos estáticos de SQL Server (geography::Point).
    /// </summary>
    internal static class ParametrosConsultaServicio
    {
        /// <summary>Prefijo con el que se envían los parámetros a SQL Server.</summary>
        private const string PrefijoSqlServer = "@";

        /// <summary>
        /// Devuelve los nombres (sin ":") de los parámetros distintos
        /// de la consulta, en el orden en que aparecen por primera vez.
        /// La comparación no distingue mayúsculas/minúsculas, igual
        /// que SQL Server con los nombres de parámetro.
        /// </summary>
        public static List<string> ExtraerParametros(string textoSql)
        {
            List<string> nombres = new List<string>();
            HashSet<string> vistos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            Recorrer(textoSql, nombre =>
            {
                if (vistos.Add(nombre))
                    nombres.Add(nombre);
            });

            return nombres;
        }

        /// <summary>
        /// Devuelve el texto SQL con cada ":Nombre" sustituido por
        /// "@Nombre", listo para ejecutarse con un SqlCommand al que
        /// se añaden los parámetros correspondientes.
        /// </summary>
        public static string PrepararParaEjecucion(string textoSql)
        {
            return Recorrer(textoSql, null);
        }

        /// <summary>Nombre del SqlParameter correspondiente a un parámetro ":Nombre".</summary>
        public static string NombreParametroSql(string nombre)
        {
            return PrefijoSqlServer + nombre;
        }

        /// <summary>
        /// Recorre el texto SQL saltando literales, identificadores
        /// delimitados y comentarios; por cada parámetro encontrado
        /// invoca <paramref name="alEncontrar"/> (si no es null) y
        /// devuelve el texto con los parámetros ya en formato @Nombre.
        /// </summary>
        private static string Recorrer(string textoSql, Action<string> alEncontrar)
        {
            if (string.IsNullOrEmpty(textoSql))
                return textoSql ?? string.Empty;

            StringBuilder resultado = new StringBuilder(textoSql.Length);
            int i = 0;
            int longitud = textoSql.Length;

            while (i < longitud)
            {
                char c = textoSql[i];
                char siguiente = i + 1 < longitud ? textoSql[i + 1] : '\0';

                if (c == '\'' || c == '"' || c == '[')
                {
                    int fin = FinDelimitado(textoSql, i, c == '[' ? ']' : c);
                    resultado.Append(textoSql, i, fin - i);
                    i = fin;
                }
                else if (c == '-' && siguiente == '-')
                {
                    int fin = textoSql.IndexOf('\n', i);
                    fin = fin < 0 ? longitud : fin;
                    resultado.Append(textoSql, i, fin - i);
                    i = fin;
                }
                else if (c == '/' && siguiente == '*')
                {
                    int fin = textoSql.IndexOf("*/", i + 2, StringComparison.Ordinal);
                    fin = fin < 0 ? longitud : fin + 2;
                    resultado.Append(textoSql, i, fin - i);
                    i = fin;
                }
                else if (c == ':' && EsInicioParametro(textoSql, i))
                {
                    int fin = i + 1;

                    while (fin < longitud && (char.IsLetterOrDigit(textoSql[fin]) || textoSql[fin] == '_'))
                        fin++;

                    string nombre = textoSql.Substring(i + 1, fin - i - 1);

                    alEncontrar?.Invoke(nombre);
                    resultado.Append(PrefijoSqlServer).Append(nombre);
                    i = fin;
                }
                else
                {
                    resultado.Append(c);
                    i++;
                }
            }

            return resultado.ToString();
        }

        /// <summary>
        /// Un ":" inicia parámetro si le sigue una letra o "_" y no
        /// forma parte de "::" ni va pegado a un identificador anterior.
        /// </summary>
        private static bool EsInicioParametro(string texto, int posicion)
        {
            if (posicion + 1 >= texto.Length)
                return false;

            char siguiente = texto[posicion + 1];

            if (!char.IsLetter(siguiente) && siguiente != '_')
                return false;

            if (posicion > 0)
            {
                char anterior = texto[posicion - 1];

                if (anterior == ':' || char.IsLetterOrDigit(anterior) || anterior == '_')
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Devuelve la posición siguiente al cierre de un literal o
        /// identificador delimitado que empieza en
        /// <paramref name="inicio"/>, teniendo en cuenta el escape por
        /// duplicado del delimitador ('' / "" / ]]).
        /// </summary>
        private static int FinDelimitado(string texto, int inicio, char cierre)
        {
            int i = inicio + 1;

            while (i < texto.Length)
            {
                if (texto[i] == cierre)
                {
                    if (i + 1 < texto.Length && texto[i + 1] == cierre)
                    {
                        i += 2;
                        continue;
                    }

                    return i + 1;
                }

                i++;
            }

            return texto.Length;
        }
    }
}
