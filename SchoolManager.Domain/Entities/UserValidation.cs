using SchoolManager.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SchoolManager.Domain.Entities
{
    [MetadataType(typeof(UserValidation))]
    public partial class User
    {
        [NotMapped]
        public string FullName
        {
            get { return string.Format("{0}, {1}", this.Surname, this.Name); }
        }

        public bool IsInRole(string roleName)
        {
            return this.Roles.Where(role => role.Name.Equals(roleName)).Count() > 0;
        }

        [NotMapped]
        public string ClassroomId { get; set; }

        [NotMapped]
        public string ClassroomText
        {
            get
            {
                return this.Classroom != null ? this.Classroom.Name : string.Empty;
            }
        }

        [NotMapped]
        public int Promotion
        {
            get { return this.YearOfPromotion.HasValue ? this.YearOfPromotion.Value : 0; }
        }

        [NotMapped]
        public string UserType { get; set; }

        [NotMapped]
        public User Mom { get; set; }

        [NotMapped]
        public User Dad { get; set; }
    }

    public class UserValidation
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessageResourceName = "MEN001", ErrorMessageResourceType = typeof(Messages))]
        [DisplayName("Documento")]
        public int Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Introduzca el nombre")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Introduzca el apellido")]
        [Display(Name = "Apellido")]
        public string Surname { get; set; }

        [Display(Name = "Email")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Introduzca una dirección de correo electrónico válida")]
        public string Email { get; set; }

        [Display(Name = "Teléfono")]
        public string Phone { get; set; }

        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Display(Name = "Notas")]
        public string Notes { get; set; }

        [Display(Name = "Comparte teléfono")]
        public bool SharePhone { get; set; }

        [Display(Name = "Comparte Email")]
        public bool ShareEmail { get; set; }

        [Display(Name = "Papá")]
        public int DadId { get; set; }

        [Display(Name = "Mamá")]
        public int MomId { get; set; }

        [Display(Name = "Grado")]
        public Classroom Classroom { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        public string Birthdate { get; set; }

        [Display(Name = "Año de egreso")]
        public int YearOfPromotion { get; set; }
    }
}