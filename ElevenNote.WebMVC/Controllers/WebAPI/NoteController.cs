using ElevenNote.Models.NoteModels;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ElevenNote.WebMVC.Controllers.WebAPI
{
    [Authorize]
    [RoutePrefix("api/Note")]
    public class NoteController : ApiController
    {
        private bool SetStarState(int noteId, bool newState)
        {
            // Create the Service
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new NoteService(userId);

            // Get the Note from the Service
            var detail = service.GetNoteById(noteId);

            // Create the NoteEdit model instance with the new star state
            var updatedNote =
                new NoteEdit
                {
                    NoteId = detail.NoteId,
                    Title = detail.Title,
                    Content = detail.Content,
                    IsStarred = newState
                };

            // Return a value indicating whether the update succeeded
            return service.UpdateNote(updatedNote);
        }

        [HttpPut]
        [Route("{id}/Star")]
        public bool ToggleStarOn(int id) => SetStarState(id, true);

        [HttpDelete]
        [Route("{id}/Star")]
        public bool ToggleStarOff(int id) => SetStarState(id, false);
    }
}
