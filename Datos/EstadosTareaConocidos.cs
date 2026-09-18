namespace GestionFacturas.Datos
{
    /// <summary>
    /// Identificadores de TBL_ESTADOS_TAREA que el código necesita
    /// conocer para escribir/leer eventos concretos del flujo
    /// (alta, asignación, bloqueo, desbloqueo, facturación,
    /// cancelación).
    ///
    /// Se referencian por ID y no por su descripción (DES_ESTADO)
    /// porque esta última es un texto pensado para mostrarse en
    /// pantalla y editarse libremente desde el negocio -de hecho,
    /// ya se renombró una vez ("Registrado"/"En proceso"/"Pendiente"/
    /// "Facturado"/"Cancelado" en vez de los nombres con los que se
    /// sembró originalmente la tabla, "REGISTRADO"/"EN_PROCESO"/
    /// "BLOQUEADO"/"FACTURADO"), lo que rompió en silencio todas las
    /// comparaciones de texto que hacía la aplicación. El ID, al ser
    /// la clave primaria, no cambia aunque se retoque el texto.
    ///
    /// IMPORTANTE: estos valores deben coincidir con el orden de
    /// inserción de ScriptsBBDD/v2/02_DatosIniciales.sql (que crea
    /// las cinco filas en este mismo orden, por lo que sus IDENTITY
    /// resultan 1..5). Si se añade, elimina o reordena algún estado
    /// en ese script, hay que actualizar también estas constantes.
    /// </summary>
    internal static class EstadosTareaConocidos
    {
        /// <summary>Estado inicial de toda tarea nueva ("Registrado").</summary>
        public const int Registrado = 1;

        /// <summary>Tras asignar un usuario ("En proceso").</summary>
        public const int EnProceso = 2;

        /// <summary>
        /// Tras bloquear la tarea. El texto actual en base de datos
        /// es "Pendiente" (no "Bloqueado"); conceptualmente sigue
        /// siendo el mismo estado que usan Bloquear/Desbloquear
        /// tarea en el Pool de Tareas.
        /// </summary>
        public const int Pendiente = 3;

        /// <summary>Tras registrar la facturación ("Facturado").</summary>
        public const int Facturado = 4;

        /// <summary>Tras cancelar la tarea ("Cancelado").</summary>
        public const int Cancelado = 5;
    }
}
