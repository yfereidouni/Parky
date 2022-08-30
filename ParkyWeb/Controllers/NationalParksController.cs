using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Models.DTOs;
using ParkyWeb.Repository.IRepository;
using System.Reflection;

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


        public async Task<IActionResult> Upsert(int? id)
        {
            NationalParkDTO obj = new NationalParkDTO();

            if (id == null)
            {
                //This will be true for Insert/Create
                return View(obj);
            }

            //Flow will come here for update
            var nationalParkObj = await _npRepoo.GetAsync(SD.NationalParkAPIPath, id.GetValueOrDefault());
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
                    var objFromDb = await _npRepoo.GetAsync(SD.NationalParkAPIPath, obj.Id);
                    nationalPark.Picture = objFromDb.Picture;
                }

                if (obj.Id == 0) // create
                {
                    await _npRepoo.CreateAsync(SD.NationalParkAPIPath, nationalPark);
                }
                else //update
                {
                    await _npRepoo.UpdateAsync(SD.NationalParkAPIPath + obj.Id, nationalPark);
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
            JsonResult jR = Json(new { data = await _npRepoo.GetAllAsync(SD.NationalParkAPIPath) });
            return jR;
        }
    }
}
