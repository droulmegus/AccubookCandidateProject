using System.Collections.Generic;

namespace AccubookCandidateProject.Models
{
   public class HotelsViewModel
   {
      public List<HotelDTO> Hotels { get; set; }

      /// <summary>
      /// The total gross revenue taken in by the hotels.
      /// </summary>
      public decimal TotalBookingsValueForAllHotels { get; set; }
   }
}