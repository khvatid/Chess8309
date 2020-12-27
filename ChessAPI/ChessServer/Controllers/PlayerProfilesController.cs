using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ChessServer.Models;

namespace ChessServer.Controllers
{
    public class PlayerProfilesController : ApiController
    {
        private ModelChessDB db = new ModelChessDB();

        // GET: api/PlayerProfiles
        public IQueryable<PlayerProfile> GetPlayerProfiles()
        {
            return db.PlayerProfiles;
        }

        // GET: api/PlayerProfiles/5
        [ResponseType(typeof(PlayerProfile))]
        public IHttpActionResult GetPlayerProfile(string id)
        {
            PlayerProfile playerProfile = db.PlayerProfiles.Find(id);
            if (playerProfile == null)
            {
                return NotFound();
            }

            return Ok(playerProfile);
        }

        // PUT: api/PlayerProfiles/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPlayerProfile(string id, PlayerProfile playerProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != playerProfile.emailPlayer)
            {
                return BadRequest();
            }

            db.Entry(playerProfile).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerProfileExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PlayerProfiles
        [ResponseType(typeof(PlayerProfile))]
        public IHttpActionResult PostPlayerProfile(PlayerProfile playerProfile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PlayerProfiles.Add(playerProfile);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PlayerProfileExists(playerProfile.emailPlayer))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = playerProfile.emailPlayer }, playerProfile);
        }

        // DELETE: api/PlayerProfiles/5
        [ResponseType(typeof(PlayerProfile))]
        public IHttpActionResult DeletePlayerProfile(string id)
        {
            PlayerProfile playerProfile = db.PlayerProfiles.Find(id);
            if (playerProfile == null)
            {
                return NotFound();
            }

            db.PlayerProfiles.Remove(playerProfile);
            db.SaveChanges();

            return Ok(playerProfile);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PlayerProfileExists(string id)
        {
            return db.PlayerProfiles.Count(e => e.emailPlayer == id) > 0;
        }
    }
}