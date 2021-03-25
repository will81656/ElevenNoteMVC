using ElevenNote.Models.CategoryModels;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ElevenNote.WebMVC.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        // GET: Category
        [HttpGet]
        public ActionResult Index()
        {
            var service = CreateCategoryService();
            var model = service.GetCategories();
            return View(model);
        }

        // GET: Category/Create
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryCreate model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var service = CreateCategoryService();

            if (service.CreateCategory(model))
            {
                TempData["SaveResult"] = "Your category was created";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Category could not be created");
            return View(model);
        }

        // GET: Category/Details/{id}
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var service = CreateCategoryService();
            var model = service.GetCategoryById(id.Value);
            return View(model);
        }

        // GET: Category/Edit/{id}
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var service = CreateCategoryService();
            try
            {
                var detail = service.GetCategoryById(id.Value);

                var model = new CategoryEdit
                {
                    CategoryId = detail.CategoryId,
                    Name = detail.Name
                };

                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Category/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CategoryEdit model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (id != model.CategoryId)
            {
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreateCategoryService();

            if (service.UpdateCategory(model))
            {
                TempData["SaveResult"] = "Category updated";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Category could not be updated");
            return View(nameof(Index));
        }

        //GET: Category/Delete/{id}
        [HttpGet]
        [ActionName("Delete")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            try
            {
                var service = CreateCategoryService();
                var model = service.GetCategoryById(id.Value);
                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        //POST: Category/Delete/{id}
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreateCategoryService();

            if (service.DeleteCategory(id))
            {
                TempData["SaveResult"] = "Category was deleted.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Delete), id);
        }

        private CategoryService CreateCategoryService() => new CategoryService(Guid.Parse(User.Identity.GetUserId()));
    }
}