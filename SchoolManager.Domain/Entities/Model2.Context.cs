﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;

namespace SchoolManager.Domain.Entities
{
    public partial class SchoolManagerEntities : DbContext
    {
        public SchoolManagerEntities()
            : this(false) { }
    
        public SchoolManagerEntities(bool proxyCreationEnabled)	    
            : base("name=SchoolManagerEntities")
        {
            this.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
        }
    
        public SchoolManagerEntities(string connectionString)
          : this(connectionString, false) { }
    
        public SchoolManagerEntities(string connectionString, bool proxyCreationEnabled)
            : base(connectionString)
        {
            this.Configuration.ProxyCreationEnabled = proxyCreationEnabled;
        }	
    
        public ObjectContext ObjectContext
        {
          get { return ((IObjectContextAdapter)this).ObjectContext; }
        }
    
    	protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    	
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<RegistrationRequest> RegistrationRequest { get; set; }
        public DbSet<ASPStateTempApplications> ASPStateTempApplications { get; set; }
        public DbSet<ASPStateTempSessions> ASPStateTempSessions { get; set; }
        public DbSet<SystemConfigurations> SystemConfigurations { get; set; }
    }
}
