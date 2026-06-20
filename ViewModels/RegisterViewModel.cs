using System.ComponentModel.DataAnnotations;

namespace AppCompleta.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [StringLength(100,ErrorMessage ="El nombre no puede superar los 100 caracteres")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage ="Ingresa un correo valido")]
        [StringLength(100,ErrorMessage = "El correo no puede superar los 100 caracteres")]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [MinLength(8,ErrorMessage ="La contraseña debe tener al menos 8 caracteres")]
        public string Clave { get; set; } = null!;
    }
}
