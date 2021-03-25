using ElevenNote.Data;
using ElevenNote.Models.CategoryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    public class CategoryService
    {
        private readonly Guid _userId;
        public CategoryService(Guid userId)
        {
            _userId = userId;
        }

        public IEnumerable<CategoryListItem> GetCategories()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var categoryQuery =
                    ctx
                        .Categories
                        .Select(e => new CategoryListItem
                        {
                            CategoryId = e.CategoryId,
                            Name = e.Name,
                            IsUserOwned = e.CreatorId == _userId
                        });

                return categoryQuery.ToArray();
            }
        }

        public bool CreateCategory(CategoryCreate model)
        {
            var entity = new Category
            {
                Name = model.Name,
                CreatorId = _userId
            };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Categories.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public CategoryDetail GetCategoryById(int categoryId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Categories.SingleOrDefault(e => e.CategoryId == categoryId);

                return
                    new CategoryDetail
                    {
                        CategoryId = entity.CategoryId,
                        Name = entity.Name,
                        IsUserOwned = entity.CreatorId == _userId
                    };
            }
        }

        public bool UpdateCategory(CategoryEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Categories
                        .SingleOrDefault(e => e.CategoryId == model.CategoryId && e.CreatorId == _userId);

                entity.CategoryId = model.CategoryId;
                entity.Name = model.Name;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeleteCategory(int categoryId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Categories
                        .SingleOrDefault(e => e.CategoryId == categoryId && e.CreatorId == _userId);

                ctx.Categories.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
