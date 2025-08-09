using CalenderEventProject.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CalenderEventProject.Context
{
    public class ProjectContext : DbContext
    {
        public ProjectContext() : base("name= ProjectContext")
        {
        } 
        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}