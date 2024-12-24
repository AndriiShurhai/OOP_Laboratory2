using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public abstract class Piece
    {
        public char Type { get; set; }  
        public bool IsWhite { get; set; }
        public (int Row, int Col) Position { get; set; }

        public Piece(char type, bool isWhite, (int, int) position, char[,] board)
        {
            Type = type;
            IsWhite = isWhite;
            Position = position;
            board[position.Item1, position.Item2] = Type;
        }

        public abstract bool IsValidMove(int fromRow, int fromCol, int toRow, int toCol, char[,] board);
    }

    public class King : Piece
    {
        public King(bool isWhite, (int, int) position, char[,] board)
            : base(!isWhite ? '♚' : '♔', isWhite, position, board) { }

        public override bool IsValidMove(int fromRow, int fromCol, int toRow, int toCol, char[,] board)
        {
            char[] majorPiecesW = { '♖', '♘', '♗', '♕', '♔', '♗', '♘', '♖', '♙' };
            char[] majorPiecesB = { '♜', '♞', '♝', '♛', '♚', '♝', '♞', '♜', '♟' };

            int rowDiff = Math.Abs(fromRow - toRow);
            int colDiff = Math.Abs(fromCol - toCol);

            // King moves one square in any direction
            if (rowDiff > 1 || colDiff > 1)
                return false;

            // Check if destination square is occupied by a friendly piece
            char destinationPiece = board[toRow, toCol];
            if (destinationPiece != ' ')
            {
                bool isDestinationFriendly = IsWhite ?
                    majorPiecesW.Contains(destinationPiece) :
                    majorPiecesB.Contains(destinationPiece);

                if (isDestinationFriendly)
                    return false;
            }

            return true;
        }
    }


    public class Queen : Piece
    {
        public Queen(bool isWhite, (int, int) position, char[,] board)
            : base(!isWhite ? '♛' : '♕', isWhite, position, board) { }

        public override bool IsValidMove(int fromRow, int fromCol, int toRow, int toCol, char[,] board)
        {
            bool ValidRook()
            {
                if (fromRow != toRow && fromCol != toCol) return false;

                // Check for obstructions
                if (fromRow == toRow) // Horizontal move
                {
                    int step = fromCol < toCol ? 1 : -1;
                    for (int col = fromCol + step; col != toCol; col += step)
                        if (board[fromRow, col] != ' ') return false;
                }
                else // Vertical move
                {
                    int step = fromRow < toRow ? 1 : -1;
                    for (int row = fromRow + step; row != toRow; row += step)
                        if (board[row, fromCol] != ' ') return false;
                }

                return true;
            } 
            
            bool ValidBishop()
            {
                if (Math.Abs(fromRow - toRow) != Math.Abs(fromCol - toCol)) return false;

                int rowStep = fromRow < toRow ? 1 : -1;
                int colStep = fromCol < toCol ? 1 : -1;

                for (int row = fromRow + rowStep, col = fromCol + colStep; row != toRow; row += rowStep, col += colStep)
                    if (board[row, col] != ' ') return false;

                return true;
            }

            return ValidBishop() || ValidRook();
        }
    }


    public class Rook : Piece
    {
        public Rook(bool isWhite, (int, int) position, char[,] board)
            : base(!isWhite ? '♜' : '♖', isWhite, position, board) { }

        public override bool IsValidMove(int fromRow, int fromCol, int toRow, int toCol, char[,] board)
        {
            if (fromRow != toRow && fromCol != toCol) return false;

            // Check for obstructions
            if (fromRow == toRow) // Horizontal move
            {
                int step = fromCol < toCol ? 1 : -1;
                for (int col = fromCol + step; col != toCol; col += step)
                    if (board[fromRow, col] != ' ') return false;
            }
            else // Vertical move
            {
                int step = fromRow < toRow ? 1 : -1;
                for (int row = fromRow + step; row != toRow; row += step)
                    if (board[row, fromCol] != ' ') return false;
            }

            return true;
        }
    }


    public class Bishop : Piece
    {
        public Bishop(bool isWhite, (int, int) position, char[,] board)
            : base(!isWhite ? '♝' : '♗', isWhite, position, board) { }

        public override bool IsValidMove(int fromRow, int fromCol, int toRow, int toCol, char[,] board)
        {
            if (Math.Abs(fromRow - toRow) != Math.Abs(fromCol - toCol)) return false;

            int rowStep = fromRow < toRow ? 1 : -1;
            int colStep = fromCol < toCol ? 1 : -1;

            for (int row = fromRow + rowStep, col = fromCol + colStep; row != toRow; row += rowStep, col += colStep)
                if (board[row, col] != ' ') return false;

            return true;
        }
    }


    public class Knight : Piece
    {
        public Knight(bool isWhite, (int, int) position, char[,] board)
            : base(!isWhite ? '♞' : '♘', isWhite, position, board) { }

        public override bool IsValidMove(int fromRow, int fromCol, int toRow, int toCol, char[,] board)
        {
            int rowDiff = Math.Abs(fromRow - toRow);
            int colDiff = Math.Abs(fromCol - toCol);

            return (rowDiff == 2 && colDiff == 1) || (rowDiff == 1 && colDiff == 2);
        }
    }


    public class Pawn : Piece
    {
        public Pawn(bool isWhite, (int, int) position, char[,] board)
            : base(!isWhite ? '♟' : '♙', isWhite, position, board) { }

        public override bool IsValidMove(int fromRow, int fromCol, int toRow, int toCol, char[,] board)
        {
            int direction = IsWhite ? 1 : -1;
            int startRow = IsWhite ? 1 : 6;

            if (fromCol == toCol && board[toRow, toCol] == ' ' && toRow == fromRow + direction)
                return true;

            if (fromCol == toCol && board[toRow, toCol] == ' ' && fromRow == startRow && toRow == fromRow + 2 * direction)
                return board[fromRow + direction, toCol] == ' '; // Check if square in between is empty

            if (Math.Abs(fromCol - toCol) == 1 && toRow == fromRow + direction && board[toRow, toCol] != ' ')
                return true;

            return false;
        }
    }
}
