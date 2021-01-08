using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLib
{
    public class ChessClass
    {
        public string fen {get;private set;}
        private bool CHECK;
        Board board;
        Moves moves;
        List<FigureMoving> allMoves;
        public ChessClass (string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {
            this.fen = fen;
            this.CHECK = false;
            board = new Board(fen);
            moves = new Moves(board);
        }

        ChessClass (Board board)
        {
            this.board = board;
            this.fen = board.fen;
            CHECK = false;
            moves = new Moves(board);
        }


        //на вход подается значение в виде Pa2a4. И возвращается новая доска для дальнейшей работы
        public ChessClass Move (string move)
        {
            FigureMoving figureMoving = new FigureMoving(move);
            if (!moves.CanMove(figureMoving))
                return this;
            if (board.IsCheckAfterMove(figureMoving))
            {
                CHECK = true;
                return this;
            }
                
            Board nextBoard = board.Move(figureMoving);
            ChessClass nextChess = new ChessClass(nextBoard);
            
            return nextChess;
        }
        
        //Проверка фигуры по координатам
        public char GetFigureAt(int x, int y)
        {
            Square square = new Square(x, y);
            Figure figure = board.GetFigureAt(square);
            return figure == Figure.none ? '1' : (char) figure;
        }

        void FindAllMoves()
        {
            allMoves = new List<FigureMoving>();
            foreach ( FigureOnSquare fs in board.YieldFigures())
                foreach (Square to in Square.YieldSquare())
                {
                    FigureMoving figureMoving = new FigureMoving(fs, to);
                    if (moves.CanMove(figureMoving))
                       // if(!board.IsCheckAfterMove(figureMoving))
                            allMoves.Add(figureMoving);
                }
        }

        public string GetColor()
        {
            return board.moveColor == Color.white ? "white" : "black";
        }

        public List<string> GetAllMoves ()
        {
            FindAllMoves();
            List<string> list = new List<string>();
            foreach (FigureMoving figureMoving in allMoves)
                list.Add(figureMoving.ToString());
            return list;
        }

        public bool IsCheck()
        {
            return board.IsCheck();
        }

        public bool IsCheckAfter()
        {
            return CHECK;
        }
    }
}
