using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackOfficeHYS
{
    public class Capacitacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }

        ///<Summary>
        /// Nombre de Usuario
        ///</Summary>
        ///
        public string NombreCapacitacion { get; set; }

        ///<Summary>
        /// Contraseña
        ///</Summary>
        ///
        public byte[] ArchivoDeCapacitacion { get; set; }

		///<Summary>
		/// Contraseña
		///</Summary>
		///
		public byte[] Banner{ get; set; }


		///<Summary>
		/// Contraseña
		///</Summary>
		///
		public int VigenciaCapacitacion { get; set; }

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
