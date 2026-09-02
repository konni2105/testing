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

        //table name Teachers
        public DbSet<Teacher> Teachers{ get; set; }

        //table name Subjects
        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Department>Departments { get; set; }

        public DbSet<Class> Classes { get; set; }

        public DbSet<ClassSubject> ClassSubjects { get; set; }

        public DbSet<TeacherSubjectClass> TeacherSubjectClasses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Department → Subject
            modelBuilder.Entity<Subject>()
                .HasOne(s => s.Department)
                .WithMany(d => d.Subjects)
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Class → Student
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Class)
                .WithMany(c => c.Students)
                .HasForeignKey(s => s.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            // Class ↔ Subject
            modelBuilder.Entity<ClassSubject>()
                .HasKey(cs => new { cs.ClassId, cs.SubjectId });

            modelBuilder.Entity<ClassSubject>()
                .HasOne(cs => cs.Class)
                .WithMany(c => c.ClassSubjects)
                .HasForeignKey(cs => cs.ClassId);

            modelBuilder.Entity<ClassSubject>()
                .HasOne(cs => cs.Subject)
                .WithMany(s => s.ClassSubjects)
                .HasForeignKey(cs => cs.SubjectId);

            // Teacher ↔ Subject ↔ Class
            modelBuilder.Entity<TeacherSubjectClass>()
                .HasKey(tsc => new
                {
                    tsc.TeacherId,
                    tsc.SubjectId,
                    tsc.ClassId
                });

            modelBuilder.Entity<TeacherSubjectClass>()
                .HasOne(tsc => tsc.Teacher)
                .WithMany(t => t.TeacherSubjectClasses)
                .HasForeignKey(tsc => tsc.TeacherId);

            modelBuilder.Entity<TeacherSubjectClass>()
                .HasOne(tsc => tsc.Subject)
                .WithMany(s => s.TeacherSubjectClasses)
                .HasForeignKey(tsc => tsc.SubjectId);

            modelBuilder.Entity<TeacherSubjectClass>()
                .HasOne(tsc => tsc.Class)
                .WithMany(c => c.TeacherSubjectClasses)
                .HasForeignKey(tsc => tsc.ClassId);
        }


    }

}
