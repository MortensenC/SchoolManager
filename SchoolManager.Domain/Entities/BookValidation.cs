// -----------------------------------------------------------------------
// <copyright file="BookValidation.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using SchoolManager.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManager.Domain.Entities
{
    [MetadataType(typeof(BookValidation))]
    public partial class Book
    {
        [NotMapped]
        public string UserId { get; set; }
    }

    public class BookValidation
    {
        [Required(ErrorMessageResourceName = "MEN003", ErrorMessageResourceType = typeof(Messages))]
        [DisplayName("Título")]
        public int Title { get; set; }

        [DisplayName("Descripción")]
        public string Description { get; set; }
        [DisplayName("Autor")]
        public string Author { get; set; }

        [DisplayName("Destacado")]
        public bool Highlighted { get; set; }
    }
}