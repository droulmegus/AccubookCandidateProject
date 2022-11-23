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
            HotelContext db = new HotelContext();
            List<HotelDTO> hotelDTOs = new List<HotelDTO>();

            foreach (Hotel h in db.Hotels)
            {
                hotelDTOs.Add(new HotelDTO
                {
                    Id = h.Id,
                    Name = h.Name,
                    Address = h.Address,
                    TimesBooked = db.Bookings.Where(b => b.HotelId == h.Id).Count(),
                });
            }

            return View();

            // your code here - the above code contains a performance pitfall
            // identity/document the issue and rewrite the code with performance in mind
        }
    }
}