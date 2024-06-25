using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web2.Models;

namespace Web2.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
		public DbSet<patient_group> patient_groups { get; set; }
        public DbSet<patient_archive> patient_archive { get; set; }
        public DbSet<Analyzes> analyzes { get; set;}
        public DbSet<Examination> examination { get; set;}
        public DbSet<Role> role { get; set; }
        public DbSet<User> user { get; set; }
        public DbSet<Archive_Group> archive_group { get; set; }
        public DbSet<Patient_Analyzes> patient_analyzes { get; set; }
        public DbSet<Patient_Examination> patient_examinations { get; set; }
        public DbSet<User_Patients> user_patients { get; set; }
    }
}
