using AccubookCandidateProject.Data;
using AccubookCandidateProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AccubookCandidateProject.Logic

{
   public class HotelsManager
   {
      private HotelsRepository _HotelsRepository;

      public HotelsManager()
      {
         _HotelsRepository = new HotelsRepository();
      }

      public async Task<HotelDTO> Get(int id)
      {
         HotelDTO result = null;
         try
         {
            var hotel = await _HotelsRepository.Get(id);
            result = GetHotelDTO(hotel);
         }
         catch (Exception)
         {
            //Logging here
         }
         return result;
      }

      public async Task<List<HotelDTO>> List()
      {
         List<HotelDTO> result = null;
         try
         {
            var hotels = await _HotelsRepository.ListWithBookings();
            result = hotels.Select(h => GetHotelDTO(h)).ToList();
         }
         catch (Exception)
         {
            //Logging here
         }
         return result;
      }

      #region Private Methods
      /// <summary>
      /// Converts Hotel to HotelDTO
      /// </summary>
      /// <param name="hotel">Hotel to convert</param>
      /// <returns>Converted hotel</returns>
      private HotelDTO GetHotelDTO(Hotel hotel)
      {
         var dto = new HotelDTO
         {
            Address = hotel.Address,
            Id = hotel.Id,
            Name = hotel.Name
         };

         if (hotel.Bookings?.Count > 0)
         {
            decimal totalRate = 0;
            decimal totalNights = 0;
            dto.TimesBooked = hotel.Bookings.Count;
            //Using a loop instead of LINQ to avoid looping through the list 3 times.
            foreach (var booking in hotel.Bookings)
            {
               var numberNights = (booking.Departure - booking.Arrival).Days;
               totalRate += booking.Rate;
               totalNights += numberNights;
               dto.TotalBookingsValue = booking.Rate * numberNights;
            }

            dto.AverageBookingRate = Math.Round(totalRate / dto.TimesBooked, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
            //If rounding is prefered over cutting the decimals (otherwise I would use (int))
            dto.AverageNightsStayed = Convert.ToInt32(totalNights / dto.TimesBooked);
         }
         return dto;
      }

      #endregion Private Methods
   }
}