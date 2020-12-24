using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLib
{
    class Board
    {
        public string fen { get; private set; }
        public Figure[,] figures { get; set; }
        public Color moveColor { get; private set; }
        public int moveNumber { get; private set; }
        public string castling {get; set;}

        public Board (string fen)
        {
            this.fen = fen;
            figures = new Figure[8, 8];
            Init();
        }

        void Init()
        {
            //rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1
            //                 0                          1  2   3 4 5
            // 0 - поле битвы; 1 - чей ход; 2 - рокировка; 3 - битое поле; 4 - правило 50 ходов; 5 - количество ходов; 
            string[] parts = fen.Split();
            if (parts.Length != 6) return;
            InitFigures(parts[0]);
            moveColor = (parts[1] == "b") ? Color.black : Color.white;
            castling = parts[2];
            moveNumber = int.Parse(parts[5]);
        }

        void InitFigures (string data)
        {
            for (int j = 8; j >= 2; j--)
                data = data.Replace(j.ToString(), (j - 1).ToString()+"1");

            string[] lines = data.Split('/');
            for (int y = 7; y >= 0; y--)
                for (int x = 0; x < 8; x++)
                    figures[x, y] =lines[7-y][x] == '1'? Figure.none : (Figure)lines[7 - y][x];

        }

        

        void GenerateFEN()
        {
            fen = FenFigure() + //0
                (moveColor == Color.white? " w " : " b ") +//1
                castling + " - 0 " + // 2 - 3 - 4
                moveNumber.ToString();//5
        }

        public IEnumerable<FigureOnSquare> YieldFigures()
        {
            foreach (Square square in Square.YieldSquare())
                if (GetFigureAt(square).GetColor() == moveColor)
                    yield return new FigureOnSquare(GetFigureAt(square),square);
        }

        string FenFigure()
        {
            StringBuilder sb = new StringBuilder();
            for (int y = 7; y >= 0; y--)
            {
                for (int x = 0; x < 8; x++)
                    sb.Append(figures[x, y] == Figure.none ? '1' : (char)figures[x, y]);
                if(y>0) 
                    sb.Append('/');
            }
            string eight = "11111111";
            for (int j = 8; j >= 2; j--)
                sb.Replace(eight.Substring(0, j), j.ToString());
            return sb.ToString();
        }


        public Figure GetFigureAt(Square square) => square.OnBoard() ? 
            figures[square.x, square.y] : Figure.none;

        void SetFigureAt(Square square,Figure figure)
        {
            if (square.OnBoard())
                figures[square.x, square.y] = figure;
        }

        public Board Move (FigureMoving figureMoving)
        {

            Board next = new Board(fen);
            next.castling = this.castling;
            next.figures = this.figures;
            next.SetFigureAt(figureMoving.from, Figure.none);
            next.SetFigureAt(figureMoving.to, figureMoving.promotion == Figure.none ? figureMoving.figure : figureMoving.promotion);
            if (moveColor == Color.black)
                next.moveNumber++;
            next.moveColor = moveColor.FlipColor();
            next.GenerateFEN();
            return next;
        }

        bool CanEatKing()
        {
            Square badKing = FindBadKing();
            Moves moves = new Moves(this);
            foreach (FigureOnSquare fs in YieldFigures())
            {
                FigureMoving figureMoving = new FigureMoving(fs, badKing);
                if (moves.CanMove(figureMoving))
                    return true;
            }
            return false;
        }

        private Square FindBadKing()
        {
            Figure badKing = moveColor == Color.black ? Figure.whiteKing : Figure.blackKing;
            foreach (Square square in Square.YieldSquare())
                if (GetFigureAt(square) == badKing)
                    return square;
            return Square.none;
        }

        public bool IsCheck ()
        {
            Board after = new Board(fen);
            after.moveColor = moveColor.FlipColor();
            return after.CanEatKing();
        }
        public bool IsCheckAfterMove (FigureMoving figureMoving)
        {
            Board after = Move(figureMoving);
            return after.CanEatKing();
        }
     
    }
}
