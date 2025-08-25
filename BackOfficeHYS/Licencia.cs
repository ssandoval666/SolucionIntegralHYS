using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackOfficeHYS
{
    public class Licencia
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }

        ///<Summary>
        /// Nombre de Usuario
        ///</Summary>
        ///
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }

		[ForeignKey("Usuario")]
		public int UsuarioId { get; set; }

        public byte[]? Comprobante { get; set; }


		///<Summary>
		/// Perfil Activo
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
