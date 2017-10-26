using SchoolManager.Resources;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolManager.Domain.Entities
{
    [MetadataType(typeof(GalleryValidation))]
    public partial class Gallery
    {
        /// <summary>
        /// Returns the "nombredeusuario" string from a URL like https://www.flickr.com/photos/nombredeusuario/sets/721...083
        /// </summary>
        public string GetFlickrUser
        {
            get
            {
                if (this.FlickrAlbum != null)
                {
                    var split = this.FlickrAlbum.Split('/');
                    if (split.Length >= 4)
                    {
                        return split[4];
                    }
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// Returns the "721...083" string from a URL like https://www.flickr.com/photos/nombredeusuario/sets/721...083
        /// </summary>
        public string GetFlickrAlbum
        {
            get
            {
                if (this.FlickrAlbum != null)
                {
                    var split = this.FlickrAlbum.Split('/');
                    if (split.Length >= 6)
                    {
                        return split[6];
                    }
                }

                return string.Empty;
            }
        }
    }

    public class GalleryValidation
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Título")]
        [Required(ErrorMessageResourceName = "MEN002", ErrorMessageResourceType = typeof(Messages))]
        public string Title { get; set; }

        [DisplayName("Descripción")]
        public string Description { get; set; }

        [DisplayName("Album en Flickr")]
        public string FlickrAlbum { get; set; }
    }
}