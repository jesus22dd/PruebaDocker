using System.ComponentModel.DataAnnotations;

namespace AppCompleta.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingresa un correo valido")]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string Clave { get; set; } = null!;
    }
}
