namespace GestionFacturas.Modelos
{
    /// <summary>
    /// Representa un usuario al que se le pueden asignar facturas
    /// (tabla Usuarios_Facturas).
    /// </summary>
    internal class Usuario
    {
        /// <summary>Identificador del usuario en base de datos.</summary>
        public int IdUsuario { get; set; }

        /// <summary>Login/usuario de acceso al sistema.</summary>
        public string UsuarioLogin { get; set; }

        /// <summary>Nombre completo mostrado en pantalla.</summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Devuelve el nombre del usuario. Los controles ComboBox
        /// enlazados a esta clase usan este valor como texto mostrado
        /// (DisplayMember).
        /// </summary>
        public override string ToString()
        {
            return Nombre;
        }
    }
}
