using AccubookCandidateProject.Data;
using AccubookCandidateProject.Logic;
using AccubookCandidateProject.Models;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace AccubookCandidateProject.Controllers
{
   public class BookingsController : Controller
   {
      public ActionResult Index()
      {
         return View();
      }
   }

   public class BookingsApiController : ApiController
   {
      public BookingsApiController() : base()
      {
      }

      [System.Web.Http.HttpGet]
      [System.Web.Http.Route("api/bookings")]
      public async Task<IHttpActionResult> GetBookings()
      {
         var manager = new BookingsManager();
         var bookings = await manager.List();
         //Would need a different Status Code depending the value / if an error occured while getting the data
         return Ok(bookings);
      }

      [System.Web.Http.HttpPost]
      [System.Web.Http.Route("api/booking")]
      public async Task<IHttpActionResult> PostBooking(BookingDTO booking)
      {
         var manager = new BookingsManager();
         var addedBooking = await manager.Add(booking);
         //Would need a different Status Code depending the value / if there was an error adding a booking
         if (addedBooking != null)
            return Ok(addedBooking);
         else
            return BadRequest();
      }
   }
}