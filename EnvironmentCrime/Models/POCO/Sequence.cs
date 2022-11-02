using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnvironmentCrime.Models
{
    public class Sequence
    {
        [Key]
        public int Id { get; set; }

        public int CurrentValue { get; set; }

    }
}
