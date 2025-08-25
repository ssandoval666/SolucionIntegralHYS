using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackOfficeHYS
{
    public class UsuarioCapacitacion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int Id { get; set; }

        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }

        [ForeignKey("Capacitacion")]
        public int CapacitacionId { get; set; }

        public bool RealizoCurso { get; set; }

        public DateTime FeachadeRealizacion { get; set; }

        public bool Activo { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
