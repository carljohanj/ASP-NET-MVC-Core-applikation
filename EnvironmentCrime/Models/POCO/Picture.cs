using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnvironmentCrime.Models
{
    public class Picture
    {
        [Key]
        public int PictureId { get; set; }

        public string PictureName { get; set; }

        public int ErrandId { get; set; }

    }
}
