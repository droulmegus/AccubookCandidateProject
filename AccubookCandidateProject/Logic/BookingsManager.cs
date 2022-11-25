using AccubookCandidateProject.Data;
using AccubookCandidateProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AccubookCandidateProject.Logic

{
   public class BookingsManager
   {
      private BookingsRepository _BookingsRepository;
      private HotelsRepository _HotelsRepository;

      public BookingsManager()
      {
         _BookingsRepository = new BookingsRepository();
         _HotelsRepository = new HotelsRepository();
      }
      /// <summary>
      /// Returns the list of bookings with their hotels data
      /// </summary>
      /// <returns>List of bookings or null if an error</returns>
      public async Task<List<BookingDTO>> List()
      {
         List<BookingDTO> result = null;
         try
         {
            var bookings = await _BookingsRepository.ListWithHotel();
            result = bookings.Select(b => GetBookingDTO(b)).ToList();
         }
         catch (Exception)
         {
            //Logging here
         }
         return result;
      }

      /// <summary>
      /// Adds a booking and return the new booking
      /// </summary>
      /// <returns>Added booking or null if an error</returns>
      public async Task<BookingDTO> Add(BookingDTO booking)
      {
         BookingDTO result = null;
         try
         {
            //On a bigger project I would handle a way to warn the user of the wrong data.
            var bookingToAdd = GetBooking(booking);
            await ValidateBooking(bookingToAdd);

            var addedBooking = await _BookingsRepository.Add(bookingToAdd);
            result = GetBookingDTO(addedBooking);
         }
         catch (Exception)
         {
            //Logging here
         }
         return result;
      }

      #region Private Methods

      /*I didn't use automapper for performances issues*/

      /// <summary>
      /// Convert Booking to BookingDTO
      /// </summary>
      /// <param name="booking">booking to convert</param>
      /// <returns>Converted Booking</returns>
      private BookingDTO GetBookingDTO(Booking booking)
      {
         return new BookingDTO
         {
            Arrival = booking.Arrival,
            Departure = booking.Departure,
            HotelId = booking.HotelId,
            HotelName = booking.Hotel?.Name,
            Id = booking.Id,
            Name = booking.Name,
            Rate = booking.Rate
         };
      }
      /// <summary>
      /// Convert BookingDTO to Booking
      /// </summary>
      /// <param name="booking">bookingDTO to convert</param>
      /// <returns>Converted BookingDTO</returns>

      private Booking GetBooking(BookingDTO booking)
      {
         return new Booking
         {
            Arrival = booking.Arrival,
            Departure = booking.Departure,
            HotelId = booking.HotelId,
            Id = booking.Id,
            Name = booking.Name,
            Rate = booking.Rate
         };
      }

      /// <summary>
      /// Checks if the booking has valid data. Also checks the associated hotel exists.
      /// </summary>
      /// <param name="booking">booking to verify</param>
      /// <exception cref="Exception">Throws an exception if the booking is invalid</exception>
      private async Task ValidateBooking(Booking booking)
      {
         if (booking.Arrival == DateTime.MinValue)
         {
            throw new Exception("Invalid Arrival Date");
         }
         if (booking.Departure == DateTime.MinValue)
         {
            throw new Exception("Invalid Departure Date");
         }

         if (booking.Departure <= booking.Arrival)
         {
            throw new Exception("Invalid Departure Date");
         }

         var hotel = await _HotelsRepository.Get(booking.HotelId);
         if (hotel == null)
         {
            throw new Exception("Invalid Hotel");
         }

         if (string.IsNullOrEmpty(booking.Name))
         {
            throw new Exception("Invalid name");
         }

         if (booking.Rate < 0)
         {
            throw new Exception("Invalid rate");
         }
      }

      #endregion Private Methods
   }
}