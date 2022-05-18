using Microsoft.EntityFrameworkCore;

namespace CompleteExample.Entities
{
    public class CompleteExampleDBContext : DbContext
    {
        public CompleteExampleDBContext(DbContextOptions<CompleteExampleDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .ToTable("Students", schema: "dbo");

            modelBuilder.Entity<Course>()
                .ToTable("Courses", schema: "dbo");

            modelBuilder.Entity<Enrollment>()
                .ToTable("Enrollment", schema: "dbo");

            modelBuilder.Entity<Enrollment>()
                .Property(e => e.Grade).HasPrecision(5, 2);

            modelBuilder.Entity<Instructor>()
                .ToTable("Instructors", schema: "dbo");

            modelBuilder.Entity<HistoricalStudentGrade>()
                .ToTable("HistoricalStudentGrades", schema: "dbo");

            modelBuilder.Entity<HistoricalStudentGrade>()
                .Property(e => e.Grade).HasPrecision(5, 2);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<HistoricalStudentGrade> HistoricalStudentGrades { get; set; }
    }
}
