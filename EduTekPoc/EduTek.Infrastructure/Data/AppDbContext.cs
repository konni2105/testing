using System;
using System.Collections.Generic;
using System.Text;
using EduTek.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace EduTek.Infrastructure.Data
{
    public class AppDbContext : DbContext  //inherit by EFc
    {
        //constructor
        //: base(options) -> Pass options to the constructor of the parent class (DbContext).
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }


        //table name Students
        public DbSet<Student> Students { get; set; }
    }

}
