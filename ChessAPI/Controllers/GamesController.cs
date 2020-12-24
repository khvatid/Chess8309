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
using ChessAPI.Models;

namespace ChessAPI.Controllers
{
    public class GamesController : ApiController
    {
        private ModelChessDB db = new ModelChessDB();

        // GET: api/Games
        public Game GetGames()
        {
            Logic logic = new Logic();
            Game game = logic.GetCurrentGame();
            return game;
        }

        // GET: api/Games/5
        [ResponseType(typeof(Game))]
        public Game GetGame(int id)
        {
            Logic logic = new Logic();
            Game game = logic.GetGame(id);
            return game;
        }


        public Game GetMove (int id, string move)
        {
            Logic logic = new Logic();
            Game game = logic.MakeMove(id,move);
            return game;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GameExists(int id)
        {
            return db.Games.Count(e => e.ID == id) > 0;
        }
    }
}