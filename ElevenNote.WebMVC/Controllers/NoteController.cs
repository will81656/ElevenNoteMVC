using ElevenNote.Models.NoteModels;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElevenNote.WebMVC.Controllers
{
    [Authorize]
    public class NoteController : Controller
    {
        // GET: Note
        public ActionResult Index()
        {
            var service = CreateNoteService();
            var model = service.GetNotes();

            return View(model);
        }

        // GET: Note/Create
        public ActionResult Create()
        {
            PopulateCategories();

            return View();
        }

        // POST: Note/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteCreate model)
        {
            if (!ModelState.IsValid)
            {
                PopulateCategories();
                return View(model);
            }

            var service = CreateNoteService();

            if (service.CreateNote(model))
            {
                TempData["SaveResult"] = "Your note was created.";
                return RedirectToAction("Index");
            }

            PopulateCategories();
            ModelState.AddModelError("", "Note could not be created");
            return View(model);
        }

        // GET: Note/Details/{id}
        public ActionResult Details(int id)
        {
            var service = CreateNoteService();
            var model = service.GetNoteById(id);
            return View(model);
        }

        // GET: Note/Edit/{id}
        public ActionResult Edit(int id)
        {

            var service = CreateNoteService();
            var detail = service.GetNoteById(id);
            var model =
                new NoteEdit
                {
                    NoteId = detail.NoteId,
                    Title = detail.Title,
                    Content = detail.Content
                };
            PopulateCategories(detail.CategoryId);
            return View(model);
        }

        // POST: Note/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, NoteEdit model)
        {
            if (!ModelState.IsValid)
            {
                PopulateCategories(model.CategoryId);

                return View(model);
            }

            if (model.NoteId != id)
            {
                PopulateCategories(model.CategoryId);
                ModelState.AddModelError("", "Id Mismatch");
                return View(model);
            }

            var service = CreateNoteService();

            if (service.UpdateNote(model))
            {
                TempData["SaveResult"] = "Your note was updated.";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Your note could not be updated.");
            return View();
        }

        // GET: Note/Delete/{id}
        [ActionName("Delete")]
        public ActionResult Delete(int id)
        {
            var service = CreateNoteService();
            var model = service.GetNoteById(id);

            return View(model);
        }

        // POST: Note/Delete/{id}
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            var service = CreateNoteService();
            service.DeleteNote(id);

            TempData["SaveResult"] = "Your note was deleted.";
            return RedirectToAction("Index");
        }

        private NoteService CreateNoteService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new NoteService(userId);
            return service;
        }


        private void PopulateCategories()
        {
            ViewBag.CategoryId = new SelectList(new CategoryService(Guid.Parse(User.Identity.GetUserId())).GetCategories(), "CategoryId", "Name");
        }
        private void PopulateCategories(int id)
        {
            ViewBag.CategoryId = new SelectList(new CategoryService(Guid.Parse(User.Identity.GetUserId())).GetCategories(), "CategoryId", "Name", id);
        }
    }
}