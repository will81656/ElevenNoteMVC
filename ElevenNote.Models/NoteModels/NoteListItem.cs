using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Models.NoteModels
{
    public class NoteListItem
    {
        public int NoteId { get; set; }

        public string Title { get; set; }

        [UIHint("Starred")]
        [Display(Name = "Important")]
        public bool IsStarred { get; set; }

        [Display(Name = "Created")]
        public DateTimeOffset CreatedUtc { get; set; }

        public int CategoryId { get; set; }

        [Display(Name ="Category")]
        public string CategoryName { get; set; }
    }
}
