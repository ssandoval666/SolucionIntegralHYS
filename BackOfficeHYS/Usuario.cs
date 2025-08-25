using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackOfficeHYS
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public string UserName { get; set; }

        ///<Summary>
        /// Contraseña
        ///</Summary>
        ///
        public string Password { get; set; }

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string? Foto { get; set; }

        ///<Summary>
        /// Fecha de Ingreso
        ///</Summary>
        ///
        public DateTime FechaIngreso { get; set; }

        ///<Summary>
        /// Fecha de Egreso
        ///</Summary>
        ///
        public DateTime FechaEgreso { get; set; }


        public List<UsuarioCapacitacion>? Capacitaciones { get; set; }

		[ForeignKey("Perfil")]
		public int  IdPerfil { get; set; }

		///<Summary>
		/// Empleado Activo
		///</Summary>
		///
		public bool Activo { get; set; }

        ///<Summary>
        /// Fecha de Creacion
        ///</Summary>
        ///
        public DateTime CreatedDate { get; set; }
    }
}
