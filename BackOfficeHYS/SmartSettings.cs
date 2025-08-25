using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackOfficeHYS
{
    public class SmartSettings
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int id { get; set; }

		public string Version { get; set; }
		public string App { get; set; }
		public string AppName { get; set; }
		public string AppFlavor { get; set; }
		public string AppFlavorSubscript { get; set; }
		public string Logo { get; set; }

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
