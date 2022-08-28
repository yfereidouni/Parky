using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers
{
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _npRepoo;

        public NationalParksController(INationalParkRepository npRepoo)
        {
            _npRepoo = npRepoo;
        }

        public IActionResult Index()
        {
            return View(new NationalPark() { });
            //return View();
        }

        public async Task<IActionResult> GetAllNationalPark()
        {
            JsonResult jR =  Json(new { data = await _npRepoo.GetAllAsync(SD.NationalParkAPIPath) });
            return jR;
        }
    }
}
