using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLib
{
	class Moves
	{
		FigureMoving figureMoving;
		Board board;

		public Moves (Board board)
		{
			this.board = board;
		}

		public bool CanMove (FigureMoving figureMoving)
		{
			this.figureMoving = figureMoving;
			return
				CanMoveFrom() &&
				CanMoveTo() &&
				CanFigureMove();
		}

		bool CanMoveFrom()
		{
			return figureMoving.from.OnBoard() &&
				board.GetFigureAt(new Square(figureMoving.from.x,figureMoving.from.y))== figureMoving.figure &&
				figureMoving.figure.GetColor() == board.moveColor;
		}
		bool CanMoveTo()
		{
			return figureMoving.to.OnBoard() &&
				figureMoving.from != figureMoving.to &&
				board.GetFigureAt(figureMoving.to).GetColor() != board.moveColor;
		}

		bool CanFigureMove ()
		{
			switch (figureMoving.figure)
			{
				
				case Figure.whiteKing:
				case Figure.blackKing:
					return CanKingMove();
				case Figure.whiteQueen:
				case Figure.blackQueen:
					return CanStraightMove();


				case Figure.whiteRook:
				case Figure.blackRook:
					//KQkq
					if (figureMoving.figure == Figure.whiteRook)
                    {
						if (figureMoving.from.x == 0 && figureMoving.from.y == 0)
							board.castling = board.castling.Remove(1,1).Insert(1,"-");
						if (figureMoving.from.x == 7 && figureMoving.from.y == 0)
							board.castling = board.castling.Remove(0, 1).Insert(1, "-");
                    }
					if (figureMoving.figure == Figure.blackRook)
                    {
						if (figureMoving.from.x == 0 && figureMoving.from.y == 7)
							board.castling = board.castling.Remove(3, 1).Insert(1, "-");
						if (figureMoving.from.x == 7 && figureMoving.from.y == 7)
							board.castling = board.castling.Remove(2, 1).Insert(1, "-");
					}
					return (figureMoving.SignX == 0 || figureMoving.SignY == 0) && CanStraightMove();

				case Figure.whiteBishop:
				case Figure.blackBishop:
					return (figureMoving.SignX != 0 && figureMoving.SignY != 0)&& CanStraightMove();

				case Figure.whiteKnight:
				case Figure.blackKnight:
					return CanKnightMove();
				case Figure.whitePawn:
				case Figure.blackPawn:
					return CanPawnMove();

				default: return false;
			}
		}

        private bool CanPawnMove()
        {
			if (figureMoving.from.y < 1 || figureMoving.from.y > 6)
				return false;
			int StepY = figureMoving.figure.GetColor() == Color.white ? 1 : -1;
			return
				CanPawnGo(StepY) ||
				CanPawnJump(StepY) ||
				CanPawnEat(StepY);
        }

        private bool CanPawnGo(int stepY)
        {	

			if (board.GetFigureAt(figureMoving.to) == Figure.none)
				if (figureMoving.AbsDeltaX == 0)
					if (figureMoving.DeltaY == stepY)
                    {
						if (figureMoving.to.y == 7 || figureMoving.to.y == 0)
							TransformPawn();
						return true;
                    }
			return false;
        }

        private bool CanPawnJump(int stepY)
        {
			if (board.GetFigureAt(figureMoving.to) == Figure.none)
				if (figureMoving.AbsDeltaX == 0)
					if (figureMoving.DeltaY == 2 * stepY)
						if (figureMoving.from.y == 1 || figureMoving.from.y == 6)
							if (board.GetFigureAt(new Square(figureMoving.from.x, figureMoving.from.y + stepY)) == Figure.none)
								return true;
			return false;
		}

        private bool CanPawnEat(int stepY)
        {
			if (board.GetFigureAt(figureMoving.to) != Figure.none)
				if (figureMoving.AbsDeltaX == 1)
					if (figureMoving.DeltaY == stepY)
					{
						if (figureMoving.to.y == 7 || figureMoving.to.y == 0)
							TransformPawn();
						return true;
					}
			return false;
        }

		private void TransformPawn()
        {
			if (figureMoving.figure == Figure.whitePawn && figureMoving.to.y == 7)
			{
				figureMoving.figure = Figure.whiteQueen;
			}
			if (figureMoving.figure == Figure.blackPawn && figureMoving.to.y == 0)
			{
				figureMoving.figure = Figure.blackQueen;
			}
		}


        private bool CanStraightMove()
        {
			Square at = figureMoving.from;
			do
			{
				at = new Square(at.x + figureMoving.SignX, at.y + figureMoving.SignY);
				if (at == figureMoving.to)
					return true;
			} while (at.OnBoard() && 
					board.GetFigureAt(at)== Figure.none);
			return false;
        }

        private bool CanKingMove()
		{
			if (figureMoving.from.x == 4 && figureMoving.from.y == 0)
            {
				if (board.castling.Contains("K") && (figureMoving.to.x == 6 && figureMoving.to.y == 0))
                {
					board.figures[7, 0] = Figure.none;
					board.figures[5, 0] = Figure.whiteRook;
					board.castling = board.castling.Remove(0, 2).Insert(0, "--");
					return true;
				}
				if (board.castling.Contains("Q") && (figureMoving.to.x == 2 && figureMoving.to.y == 0))
				{
					board.figures[0, 0] = Figure.none;
					board.figures[3, 0] = Figure.whiteRook;
					board.castling = board.castling.Remove(0, 2).Insert(0, "--");
					return true;
				}
			}
			if (figureMoving.from.x == 4 && figureMoving.from.y == 7)
            {
				if (board.castling.Contains("k") && (figureMoving.to.x == 6 && figureMoving.to.y == 7))
				{
					board.figures[7, 7] = Figure.none;
					board.figures[5, 7] = Figure.blackRook;
					board.castling = board.castling.Remove(0, 2).Insert(0, "--");
					return true;
				}
				if (board.castling.Contains("q") && (figureMoving.to.x == 2 && figureMoving.to.y == 7))
				{
					board.figures[0, 7] = Figure.none;
					board.figures[3, 7] = Figure.blackRook;
					board.castling = board.castling.Remove(2, 2).Insert(2, "--");
					return true;
				}
			}
				if (figureMoving.AbsDeltaX <= 1 && figureMoving.AbsDeltaY <= 1)
					return true;
			
			return false;
		}

		private bool CanKnightMove()
		{
			return	(figureMoving.AbsDeltaX == 1 && figureMoving.AbsDeltaY == 2) ||
					(figureMoving.AbsDeltaX == 2 && figureMoving.AbsDeltaY == 1)? true : false;
		}
	}
}
