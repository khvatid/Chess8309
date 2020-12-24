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
						return true;
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
						return true;
			return false;
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
			if (board.castling == true && (figureMoving.AbsDeltaX == 2 && figureMoving.AbsDeltaY == 0))
			{
				
				
				return true;
			}
			else
			{
				if (figureMoving.AbsDeltaX <= 1 && figureMoving.AbsDeltaY <= 1)
					return true;
			}
			return false;
		}

		private bool CanKnightMove()
		{
			return	(figureMoving.AbsDeltaX == 1 && figureMoving.AbsDeltaY == 2) ||
					(figureMoving.AbsDeltaX == 2 && figureMoving.AbsDeltaY == 1)? true : false;
		}
	}
}
