using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using WebApplication112.Entities;
using WebApplication112.Models;


namespace WebApplication112.Controllers
{
    public class ArabaGaleriController : Controller
    {
       
        private readonly GaleryContext _context;
        private readonly IFileProvider fileProvider1;
        private readonly IHostingEnvironment hostingEnvironment;
        public ArabaGaleriController(GaleryContext context, IFileProvider fileprovider, IHostingEnvironment env)
        {
            _context = context;
            fileProvider1 = fileprovider;
            hostingEnvironment = env;
        }              
        public ActionResult Index()
        {

            return View(_context.Arabalar.ToList());

        }
        public ActionResult Filter1()
        {
            var kazadurumu = (TempData["kontrol"] as string);
            var model = (TempData["model"] as string);
            
            if (model != null && kazadurumu != null)
            {                        
                return View(_context.Arabalar.Where(z => z.Model == model && z.Aciklama == kazadurumu));               
                
            }
            else if (model != null && kazadurumu == null)
            {
                  return View(_context.Arabalar.Where(z => z.Model == model));
            }
            else if (model == null && kazadurumu != null)
            {
               
                return View(_context.Arabalar.Where(z => z.Aciklama == kazadurumu));
            }
                else
                {
                return View(_context.Arabalar.ToList());

            }

        }

        [HttpPost]
        public ActionResult Filter1(string model , string kontrol)
        {
            TempData["model"]  = model;
            TempData["kontrol"] = kontrol;
            return RedirectToAction("Filter1");

        }
      
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var arabaGaleri = await _context.Arabalar
                .FirstOrDefaultAsync(m => m.Id == id);
            if (arabaGaleri == null)
            {
                return NotFound();
            }

            return View(arabaGaleri);
        }
       
        public IActionResult Create()
        {
            return View();
        }       

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fiyat,Aciklama,Model,Resim")] ArabaGaleri arabaGaleri, IFormFile file)
        {

            if (ModelState.IsValid)
            {
                if (file != null || file.Length != 0)
                {
                    // Create a File Info 
                    FileInfo fi = new FileInfo(file.FileName);

                    // This code creates a unique file name to prevent duplications 
                    // stored at the file location
                    var newFilename = arabaGaleri.Id + "_" + String.Format("{0:d}",
                                      (DateTime.Now.Ticks / 10) % 100000000) + fi.Extension;
                    var webPath = hostingEnvironment.WebRootPath;
                    var path = Path.Combine("", webPath + @"\images\" + newFilename);

                    // IMPORTANT: The pathToSave variable will be save on the column in the database
                    var pathToSave =  newFilename;

                    // This stream the physical file to the allocate wwwroot/ImageFiles folder
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // This save the path to the record
                    arabaGaleri.Resim = pathToSave;
                    _context.Update(arabaGaleri);

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
               
            }
            return View(arabaGaleri);
        }

        private bool ArabaGaleriExists(int id)
        {
            return _context.Arabalar.Any(e => e.Id == id);
        }
    }
}
