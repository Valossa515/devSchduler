using DevScheduler.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevScheduler.API.Persistence
{

    public class DevScheduleDbContent : DbContext
    {
        public DevScheduleDbContent(DbContextOptions<DevScheduleDbContent> options) : base(options)
        {

        }
        public DbSet<DevSchedule> DevSchedules { get; set; }
        public DbSet<DevScheduleSpeaker> devScheduleSpeakers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DevSchedule>(e =>
            {
                e.HasKey(de => de.Id);
                e.Property(de => de.Title).IsRequired(false);
                e.Property(de => de.Description)
                  .HasMaxLength(200)
                  .HasColumnType("varchar(200)");
                e.Property(de => de.StartDate)
                    .HasColumnName("Start_Date");
                e.Property(de => de.EndDate)
                   .HasColumnName("End_Date");
                e.HasMany(de => de.Speakers)
                    .WithOne()
                    .HasForeignKey(s => s.DevScheduleId);
            });

            builder.Entity<DevScheduleSpeaker>(e => {
                e.HasKey(de => de.Id);
            });
        }

    }
}
