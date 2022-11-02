using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace EnvironmentCrime.Models
{
    public class Errand
    {
        [Key]
        public int ErrandId { get; set; }

        public string RefNumber { get; set; }
        
        [Display(Name = "Var har brottet skett någonstans?")]
        [Required(ErrorMessage = "Du måste ange en plats")]
        public string Place { get; set; }

        [Display(Name = "Vilken typ av brott?")]
        [Required(ErrorMessage = "Du måste ange typ av brott")]
        public string TypeOfCrime { get; set; }

        [Display(Name = "När skedde brottet?")]
        [Required(ErrorMessage = "Vi behöver ett datum för observationen")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString ="{0:yyyy - MM - dd}")]
        public DateTime DateOfObservation { get; set; }

        [Display(Name = "Ditt namn (för- och efternamn):")]
        [Required(ErrorMessage = "Du måste fylla i ditt namn")]
        public string InformerName { get; set; }

        [Display(Name = "Din telefon:")]
        [Required(ErrorMessage = "Du måste fylla i ett telefonnummer")]
        [RegularExpression(@"^\d{2,4}-\d{5,9}$", ErrorMessage = "Formatet för telefonnummer måste vara riktnummer-telefonnummer")]
        public string InformerPhone { get; set; }

        public string Observation { get; set; }

        public string InvestigatorInfo { get; set; }
        public string InvestigatorAction { get; set; }
        public string StatusId { get; set; }
        public string DepartmentId { get; set; }
        public string EmployeeId { get; set; }
    

        //Includes lists of object types Samples and Pictures:
        public ICollection<Picture> Pictures { get; set; }
        public ICollection<Sample> Samples { get; set; }


    }
}
