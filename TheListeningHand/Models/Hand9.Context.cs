﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TheListeningHand.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class TheListeningHandEntities : DbContext
    {
        public TheListeningHandEntities()
            : base("name=TheListeningHandEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<day> days { get; set; }
        public virtual DbSet<stylist> stylists { get; set; }
        public virtual DbSet<availability> availabilities { get; set; }
    
        public virtual ObjectResult<spGetAvailabilty_Result4> spGetAvailabilty(Nullable<short> stylist)
        {
            var stylistParameter = stylist.HasValue ?
                new ObjectParameter("stylist", stylist) :
                new ObjectParameter("stylist", typeof(short));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetAvailabilty_Result4>("spGetAvailabilty", stylistParameter);
        }
    
        public virtual ObjectResult<spUpdateAvailabilty_Result1> spUpdateAvailabilty(Nullable<short> stylist, string hrs)
        {
            var stylistParameter = stylist.HasValue ?
                new ObjectParameter("stylist", stylist) :
                new ObjectParameter("stylist", typeof(short));
    
            var hrsParameter = hrs != null ?
                new ObjectParameter("hrs", hrs) :
                new ObjectParameter("hrs", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spUpdateAvailabilty_Result1>("spUpdateAvailabilty", stylistParameter, hrsParameter);
        }
    }
}
