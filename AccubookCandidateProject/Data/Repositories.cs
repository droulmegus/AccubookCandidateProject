using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

/*All repositories are using asynchronous to match the design of the API using Tasks. It would allow th*/

namespace AccubookCandidateProject.Data
{
   /// <summary>
   /// Repository to handle SQL requests for Bookings DB Set
   /// </summary>
   public class BookingsRepository
   {
      /// <summary>
      /// Loads all bookings with the Hotel data
      /// </summary>
      /// <returns>All existing bookings with their hotel data or an empty list</returns>
      public async Task<List<Booking>> ListWithHotel()
      {
         using (var ctx = new HotelContext())
         {
            return await ctx.Bookings.Include("Hotel").ToListAsync();
         }
      }

      /// <summary>
      /// Create a new booking
      /// </summary>
      /// <param name="booking">Booking to add</param>
      /// <returns>Created Booking</returns>
      public async Task<Booking> Add(Booking booking)
      {
         using (var ctx = new HotelContext())
         {
            ctx.Entry(booking).State = EntityState.Added;
            await ctx.SaveChangesAsync();
            return booking;
         }
      }
   }

   /// <summary>
   /// Repository to handle SQL requests for Hotels DB Set
   /// </summary>
   public class HotelsRepository
   {
      /// <summary>
      /// Get an hotel by its ID
      /// </summary>
      /// <param name="id">Id of the hotel to get</param>
      /// <returns>Hotel mathcing the id or null</returns>
      public async Task<Hotel> Get(int id)
      {
         using (var ctx = new HotelContext())
         {
            return await ctx.Hotels.Where(h => h.Id == id).FirstOrDefaultAsync(); ;
         }
      }

      /// <summary>
      /// Get all Hotels in a list with their bookings
      /// </summary>
      /// <returns>All Hotels loaded with their bookings or an empty list.</returns>
      public async Task<List<Hotel>> ListWithBookings()
      {
         using (var ctx = new HotelContext())
         {
            return await ctx.Hotels.Include("Bookings").ToListAsync(); ;
         }
      }
   }
}