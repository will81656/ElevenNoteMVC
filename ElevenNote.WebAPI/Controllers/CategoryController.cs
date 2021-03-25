using ElevenNote.Models.CategoryModels;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ElevenNote.WebAPI.Controllers
{
    [Authorize]
    public class CategoryController : ApiController
    {
        public IHttpActionResult GetAll()
        {
            var categoryService = CreateCategoryService();
            var categories = categoryService.GetCategories();
            return Ok(categories);
        }

        public IHttpActionResult Get(int id)
        {
            var service = CreateCategoryService();
            var category = service.GetCategoryById(id);
            return Ok(category);
        }

        public IHttpActionResult Post(CategoryCreate category)
        {
            if (category == null)
                return BadRequest("Received model was null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = CreateCategoryService();

            if (!service.CreateCategory(category))
                return InternalServerError();

            return Ok();
        }

        public IHttpActionResult Put(CategoryEdit category)
        {
            if (category == null)
                return BadRequest("Received model was null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = CreateCategoryService();

            if (!service.UpdateCategory(category))
                return InternalServerError();

            return Ok();
        }

        public IHttpActionResult Delete(int id)
        {
            var service = CreateCategoryService();

            if (!service.DeleteCategory(id))
                return InternalServerError();

            return Ok();
        }

        private CategoryService CreateCategoryService() => new CategoryService(Guid.Parse(User.Identity.GetUserId()));
    }
}
