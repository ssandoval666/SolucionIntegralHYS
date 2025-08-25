using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackOfficeHYS
{
    public class Perfil
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }

        ///<Summary>
        /// Nombre de Usuario
        ///</Summary>
        ///
        public string NombrePerfil { get; set; }


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
