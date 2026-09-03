using EduTek.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace EduTek.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<Class> Classes { get; set; }

        public DbSet<ClassSubject> ClassSubjects { get; set; }

        public DbSet<TeacherSubjectClass> TeacherSubjectClasses { get; set; }


        public DbSet<Attendance> Attendances { get; set; }

        public DbSet<Exam> Exams { get; set; }

        public DbSet<Mark> Marks { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

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

            // Student → Attendance
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Student)
                .WithMany(s => s.Attendances)
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Subject → Attendance
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Subject)
                .WithMany(s => s.Attendances)
                .HasForeignKey(a => a.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Subject → Exam
            modelBuilder.Entity<Exam>()
                .HasOne(e => e.Subject)
                .WithMany(s => s.Exams)
                .HasForeignKey(e => e.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // Class → Exam
            modelBuilder.Entity<Exam>()
                .HasOne(e => e.Class)
                .WithMany(c => c.Exams)
                .HasForeignKey(e => e.ClassId)
                .OnDelete(DeleteBehavior.Restrict);

            // Exam → Mark
            modelBuilder.Entity<Mark>()
                .HasOne(m => m.Exam)
                .WithMany(e => e.Marks)
                .HasForeignKey(m => m.ExamId)
                .OnDelete(DeleteBehavior.Restrict);

            // Student → Mark
            modelBuilder.Entity<Mark>()
                .HasOne(m => m.Student)
                .WithMany(s => s.Marks)
                .HasForeignKey(m => m.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Mark>()
                .HasIndex(m => new
                {
                    m.ExamId,
                    m.StudentId
                })
                .IsUnique();

            // Teacher → Feedback
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Teacher)
                .WithMany(t => t.Feedbacks)
                .HasForeignKey(f => f.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);

            // Student → Feedback
            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.Student)
                .WithMany(s => s.Feedbacks)
                .HasForeignKey(f => f.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

        }



    }
}