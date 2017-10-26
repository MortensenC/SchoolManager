// -----------------------------------------------------------------------
// <copyright file="PostValidation.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManager.Domain.Entities
{
    [MetadataType(typeof(PostValidation))]
    public partial class Post
    {
        [NotMapped]
        public string ClassroomId { get; set; }

        [NotMapped]
        public string ShortText
        {
            get
            {
                if (this.Text.Length > 500)
                {
                    return this.Text.Substring(0, 500) + "...";
                }

                return this.Text;
            }
        }

        [NotMapped]
        public string CategoryText
        {
            get { return Enum.GetName(typeof(Category), this.Category); }
        }

        [NotMapped]
        public bool Selected { get; set; }

        /// <summary>
        /// Returns the "nombredeusuario" string from a URL like https://www.flickr.com/photos/nombredeusuario/sets/721...083
        /// </summary>
        [NotMapped]
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
        [NotMapped]
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

        [NotMapped]
        public string Photo { get; set; }

        [NotMapped]
        public List<string> Files { get; set; }
    }

    public class PostValidation
    {
        [DataType(DataType.Html)]
        public string Text;
    }
}