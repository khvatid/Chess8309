using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChessLib;

namespace ChessAPI.Models
{
    public class Logic
    {
        private ModelChessDB db;

        public Logic ()
        {
            db = new ModelChessDB();
        }

        internal Game GetCurrentGame()
        {
            Game game = db.Games
                .Where(global => global.Status == "play")
                .OrderBy(global => global.ID).FirstOrDefault();
            if (game == null)
                game = CreateNewGame();
            return game;
        }

        private Game CreateNewGame()
        {
            Game game = new Game();

            ChessClass chess = new ChessClass();


            game.FEN = chess.fen;
            game.Status = "play";

            db.Games.Add(game);
            db.SaveChanges();

            return game;
        }

        public Game GetGame(int id)
        {
            return db.Games.Find(id);
        }

        public Game MakeMove(int id, string move)
        {
            Game game = GetGame(id);
            if (game == null) return game;

            if (game.Status != "play") return game;

            ChessClass chess = new ChessClass(game.FEN);
            ChessClass chessNext = chess.Move(move);

            if (chessNext.fen == game.FEN)
                return game;
            game.FEN = chessNext.fen;
            if (chessNext.IsCheck())
                game.Status = "done";
            db.Entry(game).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return game;

        }
    }
}