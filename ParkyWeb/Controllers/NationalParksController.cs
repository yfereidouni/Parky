using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Models.DTOs;
using ParkyWeb.Repository.IRepository;
using System.Reflection;

namespace ParkyWeb.Controllers
{
    [Authorize]
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _npRepo;

        public NationalParksController(INationalParkRepository npRepo)
        {
            _npRepo = npRepo;
        }

        public async Task<IActionResult> Index()
        {
            return View(new NationalPark() { });

            //var result = await _npRepo.GetAllAsync(SD.NationalParkAPIPath);
            //return View(result);
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Upsert(int? id)
        {
            NationalParkDTO obj = new NationalParkDTO();

            if (id == null)
            {
                //This will be true for Insert/Create
                return View(obj);
            }

            //Flow will come here for update
            var nationalParkObj = await _npRepo.GetAsync(SD.NationalParkAPIPath, id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            obj.Id = nationalParkObj.Id;
            obj.Name = nationalParkObj.Name;
            obj.State = nationalParkObj.State;
            obj.Created = nationalParkObj.Created;
            obj.Established = nationalParkObj.Established;
            obj.CurrentPicture = nationalParkObj.Picture;

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalParkDTO obj)
        {
            ModelState.Remove("CurrentPicture"); // to omit CurrentImage Validation Error

            if (ModelState.IsValid)
            {
                NationalPark nationalPark = new NationalPark
                {
                    Id = obj.Id,
                    Name = obj.Name,
                    State = obj.State,
                    Created = obj.Created,
                    Established = obj.Established,
                };
                if (obj.Picture != null)
                {
                    if (obj.Picture.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            obj.Picture.CopyTo(ms);
                            var fileBytes = ms.ToArray();
                            nationalPark.Picture = Convert.ToBase64String(fileBytes);
                        }
                    }
                }
                else
                {
                    var objFromDb = await _npRepo.GetAsync(SD.NationalParkAPIPath, obj.Id, HttpContext.Session.GetString("JWToken"));
                    nationalPark.Picture = objFromDb.Picture;
                }

                if (obj.Id == 0) // create
                {
                    await _npRepo.CreateAsync(SD.NationalParkAPIPath, nationalPark, HttpContext.Session.GetString("JWToken"));
                }
                else //update
                {
                    await _npRepo.UpdateAsync(SD.NationalParkAPIPath + obj.Id, nationalPark, HttpContext.Session.GetString("JWToken"));
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(obj);
            }
        }

        public async Task<IActionResult> GetAllNationalPark()
        {
            JsonResult jR = Json(new { data = await _npRepo.GetAllAsync(SD.NationalParkAPIPath, HttpContext.Session.GetString("JWToken")) });
            return jR;
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _npRepo.DeleteAsync(SD.NationalParkAPIPath, id, HttpContext.Session.GetString("JWToken"));

            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = true, message = "Delete Not Successful" });
        }
    }
}
