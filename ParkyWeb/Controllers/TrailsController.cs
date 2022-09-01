using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkyWeb.Models;
using ParkyWeb.Models.DTOs;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.IRepository;
using System.Reflection;
using static ParkyWeb.Models.ViewModel.TrailsVM;

namespace ParkyWeb.Controllers
{
    public class TrailsController : Controller
    {
        private readonly INationalParkRepository _npRepo;
        private readonly ITrailRepository _trailRepo;

        public TrailsController(INationalParkRepository npRepo, ITrailRepository trailRepo)
        {
            _npRepo = npRepo;
            _trailRepo = trailRepo;
        }

        public async Task<IActionResult> Index()
        {
            return View(new Trail() { });
        }


        public async Task<IActionResult> Upsert(int? id)
        {
            // Fill Combobox
            IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(SD.NationalParkAPIPath);
            ViewBag.NationalParks = new SelectList(npList, "Id", "Name");

            TrailsVM objVM = new TrailsVM();

            if (id == null)
            {
                //This will be true for Insert/Create
                return View(objVM);
            }

            //Flow will come here for update
            var trailObj = await _trailRepo.GetAsync(SD.TrailAPIPath, id.GetValueOrDefault());
            objVM.Id = trailObj.Id;
            objVM.Name = trailObj.Name;
            objVM.Distance = trailObj.Distance;
            objVM.Elevation = trailObj.Elevation;
            objVM.Difficulty = (DifficultyType)trailObj.Difficulty;
            objVM.NationalParkId = trailObj.NationalParkId;

            if (objVM == null)
            {
                return NotFound();
            }

            return View(objVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsVM obj)
        {
            if (ModelState.IsValid)
            {
                Trail newTrail = new Trail
                {
                    Id = obj.Id,
                    Name = obj.Name,
                    Distance = obj.Distance,
                    Elevation = obj.Elevation,
                    NationalParkId = obj.NationalParkId,
                    Difficulty = (Trail.DifficultyType)obj.Difficulty,
                };

                if (obj.Id == 0)
                {
                    await _trailRepo.CreateAsync(SD.TrailAPIPath, newTrail);
                }
                else
                {
                    await _trailRepo.UpdateAsync(SD.TrailAPIPath + newTrail.Id, newTrail);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(SD.NationalParkAPIPath);
                ViewBag.NationalParks = new SelectList(npList, "Id", "Name");

                return View(obj);
            }
        }

        public async Task<IActionResult> GetAllTrail()
        {
            JsonResult jR = Json(new { data = await _trailRepo.GetAllAsync(SD.TrailAPIPath) });
            return jR;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepo.DeleteAsync(SD.TrailAPIPath, id);

            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = true, message = "Delete Not Successful" });
        }
    }
}
