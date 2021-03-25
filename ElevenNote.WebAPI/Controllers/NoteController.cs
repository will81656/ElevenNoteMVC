using ElevenNote.Models.NoteModels;
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
    [RoutePrefix("api/Note")]
    public class NoteController : ApiController
    {
        public IHttpActionResult GetAll()
        {
            NoteService noteService = CreateNoteService();
            var notes = noteService.GetNotes();
            return Ok(notes);
        }

        public IHttpActionResult Get(int id)
        {
            var service = CreateNoteService();
            var note = service.GetNoteById(id);
            return Ok(note);
        }

        public IHttpActionResult Post(NoteCreate note)
        {
            if (note == null)
                return BadRequest("Received model was null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = CreateNoteService();

            if (!service.CreateNote(note))
                return InternalServerError();

            return Ok();
        }

        public IHttpActionResult Put(NoteEdit note)
        {
            if (note == null)
                return BadRequest("Received model was null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = CreateNoteService();

            if (!service.UpdateNote(note))
                return InternalServerError();

            return Ok();
        }

        public IHttpActionResult Delete(int id)
        {
            var service = CreateNoteService();

            if (!service.DeleteNote(id))
                return InternalServerError();

            return Ok();
        }

        [HttpPut]
        [Route("{id}/Star")]
        public bool ToggleStar(int id)
        {
            var service = CreateNoteService();

            var detail = service.GetNoteById(id);

            var updatedNote =
                new NoteEdit
                {
                    NoteId = detail.NoteId,
                    Title = detail.Title,
                    Content = detail.Content,
                    IsStarred = !detail.IsStarred
                };

            return service.UpdateNote(updatedNote);
        }

        private NoteService CreateNoteService() => new NoteService(Guid.Parse(User.Identity.GetUserId()));

    }
}
