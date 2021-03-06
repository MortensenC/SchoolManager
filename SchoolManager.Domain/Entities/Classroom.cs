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
    [KnownType(typeof(User))]
    [KnownType(typeof(Post))]
    [KnownType(typeof(Event))]
    public partial class Classroom
    {
        public Classroom()
        {
            this.Users = new HashSet<User>();
            this.Posts = new HashSet<Post>();
            this.Events = new HashSet<Event>();
        }
    
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
    
        [DataMember]
        public virtual ICollection<User> Users { get; set; }
        [DataMember]
        public virtual ICollection<Post> Posts { get; set; }
        [DataMember]
        public virtual ICollection<Event> Events { get; set; }
    }
    
}
