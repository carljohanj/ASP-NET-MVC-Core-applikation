using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnvironmentCrime.Models
{
    public class Sample
    {

        [Key]
        public int SampleId { get; set; }

        public string SampleName { get; set; }

        public int ErrandId { get; set; }

    }
}
