using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EnvironmentCrime.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Fyll i användarnamn")]
        [Display(Name = "Användarnamn")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Skriv in lösenord")]
        [Display(Name = "Lösenord")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        public string ReturnUrl { get; set; }

    }
}
