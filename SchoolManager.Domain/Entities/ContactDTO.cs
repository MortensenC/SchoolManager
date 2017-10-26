using System.ComponentModel.DataAnnotations;

namespace SchoolManager.Domain.Entities
{
    public class ContactDTO
    {
        [Required(ErrorMessage = "Ingrese su nombre")]
        public string Name { get; set; }

        public string Subject { get; set; }

        [Required(ErrorMessage = "Ingrese su correo así podremos responderle")]
        public string Email { get; set; }

        public string Phone { get; set; }

        [Required(ErrorMessage = "Ingrese su consulta")]
        public string Comment { get; set; }

        public bool SendMeCopy { get; set; }
    }
}