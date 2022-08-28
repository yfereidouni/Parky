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

        public async Task<IActionResult> Index()
        {
            return View(new NationalPark() { });
            
            //var result = await _npRepoo.GetAllAsync(SD.NationalParkAPIPath);
            //return View(result);
        }

        public async Task<IActionResult> GetAllNationalPark()
        {
            JsonResult jR =  Json(new { data = await _npRepoo.GetAllAsync(SD.NationalParkAPIPath) });
            return jR;
        }
    }
}
