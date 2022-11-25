using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;
using System.IO;
using System.Reflection;

namespace AccubookCandidateProject.Data
{
   public class HotelContext : DbContext
   {
      private static readonly string binPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);

      public HotelContext()
          : base(new SQLiteConnection($"Data Source={Path.Combine(binPath, "app.sqlite")}"), true)
      {
         Database.SetInitializer<HotelContext>(null);
      }

      public DbSet<Booking> Bookings { get; set; }
      public DbSet<Hotel> Hotels { get; set; }

      protected override void OnModelCreating(DbModelBuilder modelBuilder)
      {
         modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
      }
   }
}