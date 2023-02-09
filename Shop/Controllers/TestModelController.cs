using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Shop_Models;
using Shop_DataAccess;
using Microsoft.AspNetCore.Authorization;
using Shop_Utility;

namespace Shop.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class TestModelController : Controller
    {
        private readonly AppDbContext _db;

        public TestModelController(AppDbContext db) { _db = db; }
        public IActionResult Index()
        {
            IEnumerable<TestModel> objList = _db.TestModel;
            return View(objList);
        }
        public IActionResult Create()
        {
            return View();
        }
        // post-create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TestModel obj)
        {
            _db.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        //get-delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.TestModel.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        //Post - delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public IActionResult PostDelete(int? id)
        {
            var obj = _db.TestModel.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.TestModel.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        //GET - edit
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.TestModel.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //Post - edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TestModel obj)
        {
            _db.TestModel.Update(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
