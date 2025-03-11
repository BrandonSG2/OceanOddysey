using System.ComponentModel.DataAnnotations;

namespace OceanOdyssey.Web.ViewModels
{
    public class ViewModelInput
    {


    
        [Range(0, 999999999, ErrorMessage = "Precio mínimo es {0}")]
        [Display(Name = "Precio")]
        public decimal Precio { get; set; }
        [Display(Name = "Fecha de inicio")]
        public DateTime FechaInicio { get; set; }
        [Display(Name = "Fecha Limite Pago")]
        public DateTime FechaLimitePago { get; set; }


        [Display(Name = "Habitacion")]
        public int IdHabitacion { get; set; }
    }
}
