using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Constraints;
using System;
using Shop_DataAccess;
using Shop_Models;
using Shop_Models.ViewModels;
using NuGet.Packaging.Signing;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Shop_Utility;

namespace Shop.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ProductController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;   

        public ProductController(AppDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;   
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objList = _db.Product.Include(a => a.Category).Include(a => a.TestModel);
            //OR
            //foreach (var obj in objList)
            //{
            //    obj.Category = _db.Category.FirstOrDefault(o => o.Id == obj.CategoryId);
            //    obj.TestModel = _db.TestModel.FirstOrDefault(o => o.Id == obj.TestModelId);
            //}
            return View(objList);
        }

        //GET - upsert
        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.Id.ToString()
            //});

            //ViewBag.CategoryDropDown = CategoryDropDown;

            //var product = new Product();
            ProductVM productVM = new ProductVM
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                TestModelSelectList = _db.TestModel.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })


            };
            if (id == null)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _db.Product.Find(id);
                if(productVM.Product == null) 
                {
                    return NotFound();
                }
                else
                {
                    return View(productVM);
                }
            }
        }
        //Post - upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {

            var files = HttpContext.Request.Form.Files;
            string webRootPath = _webHostEnvironment.WebRootPath;
            if (productVM.Product.Id == 0)
            {
                //Creating
                string upload = webRootPath + WC.ImagePath;
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(files[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                productVM.Product.Image = fileName + extension;

                _db.Product.Add(productVM.Product);
            }
            else
            {
                var ObjFromDb = _db.Product.AsNoTracking().FirstOrDefault(o => o.Id == productVM.Product.Id);

                if (files.Count > 0)
                {
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    var oldFile = Path.Combine(upload, ObjFromDb.Image);
                    if (System.IO.File.Exists(oldFile))
                    {
                        System.IO.File.Delete(oldFile);
                    }
                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }                  
                }
                else
                {
                    productVM.Product.Image = ObjFromDb.Image;                    
                }
                _db.Product.Update(productVM.Product);
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET - delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //Product product = _db.Product.Find(id);
            //product.Category = _db.Category.Find(product.CategoryId);
            //OR
            Product product = _db.Product.Include(a => a.Category).Include(a => a.TestModel).FirstOrDefault(b => b.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        //Post - delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult PostDelete(int? id)
        {
            var product = _db.Product.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;

            var oldFile = Path.Combine(upload, product.Image);
            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }
            _db.Product.Remove(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
