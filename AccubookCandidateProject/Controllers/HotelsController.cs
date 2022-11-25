using AccubookCandidateProject.Logic;
using AccubookCandidateProject.Data;
using AccubookCandidateProject.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace AccubookCandidateProject.Controllers
{
   public class HotelsController : Controller
   {
      public ActionResult Index()
      {
         var manager = new HotelsManager();
         /**
          * A join on Bookings should be used instead of calling two distinct DBSets when one has an FK on the other.
          * The Model is also missing from the page.
          * */
         var hotels = manager.List().Result;
         var model = new HotelsViewModel
         {
            Hotels = hotels,
            TotalBookingsValueForAllHotels = 0
         };
         foreach (var hotel in hotels)
         {
            model.TotalBookingsValueForAllHotels += hotel.TotalBookingsValue;
         }

         return View(model);
      }
   }
}