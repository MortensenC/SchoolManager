//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace SchoolManager.Domain.Entities
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(Post))]
    [KnownType(typeof(Reply))]
    [KnownType(typeof(Classroom))]
    [KnownType(typeof(Role))]
    [KnownType(typeof(Book))]
    public partial class User
    {
        public User()
        {
            this.SharePhone = true;
            this.ShareEmail = true;
            this.Teaches = "null";
            this.Posts = new HashSet<Post>();
            this.Replies = new HashSet<Reply>();
            this.Roles = new HashSet<Role>();
            this.Book = new HashSet<Book>();
        }
    
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Surname { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Notes { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public bool SharePhone { get; set; }
        [DataMember]
        public bool ShareEmail { get; set; }
        [DataMember]
        public Nullable<int> DadId { get; set; }
        [DataMember]
        public Nullable<int> MomId { get; set; }
        [DataMember]
        public string Birthdate { get; set; }
        [DataMember]
        public string Teaches { get; set; }
        [DataMember]
        public Nullable<int> Subject { get; set; }
        [DataMember]
        public Nullable<int> YearOfPromotion { get; set; }
    
        [DataMember]
        public virtual ICollection<Post> Posts { get; set; }
        [DataMember]
        public virtual ICollection<Reply> Replies { get; set; }
        [DataMember]
        public virtual Classroom Classroom { get; set; }
        [DataMember]
        public virtual ICollection<Role> Roles { get; set; }
        [DataMember]
        public virtual ICollection<Book> Book { get; set; }
    }
    
}
