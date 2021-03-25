using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Models.CategoryModels
{
    public class CategoryListItem
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public bool IsUserOwned { get; set; }
    }
}
