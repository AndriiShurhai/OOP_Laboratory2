using System;
using System.Collections.Generic;

namespace Chess
{
    public partial class Board
    {
        private bool whiteKingMoved = false;
        private bool blackKingMoved = false;
        private bool[] whiteRooksMoved = new bool[2] { false, false };
        private bool[] blackRooksMoved = new bool[2] { false, false };

        private (int row, int col)? lastPawnDoubleMove = null;

        public void PromotePawn(string position, char promotionPiece)
        {
            var (row, col) = ConvertNotationToIndices(position);

            bool isValidPromotionRow = (isWhiteTurn && row == Size - 1) ||
                                       (!isWhiteTurn && row == 0);

            if (!isValidPromotionRow)
                throw new InvalidOperationException("Pawn can only be promoted at the last rank");

            char[] validPromotionPieces = isWhiteTurn ?
                new char[] { '♕', '♖', '♗', '♘' } :
                new char[] { '♛', '♜', '♝', '♞' };

            if (!Array.Exists(validPromotionPieces, p => p == promotionPiece))
                throw new InvalidOperationException("Invalid promotion piece");

            board[row, col] = promotionPiece;
        }

        public void Castle(bool isKingSide, string castleSide)
        {
            int row = isWhiteTurn ? 0 : Size - 1;

            // Check if king and appropriate rook have moved
            if ((isWhiteTurn && (whiteKingMoved ||
                 (isKingSide ? whiteRooksMoved[1] : whiteRooksMoved[0]))) ||
                (!isWhiteTurn && (blackKingMoved ||
                 (isKingSide ? blackRooksMoved[1] : blackRooksMoved[0]))))
            {
                throw new InvalidOperationException("Castling is not allowed");
            }

            // Check if there is no pieces beetween them
            bool p1 = true;
            bool p2 = true;
            if (isWhiteTurn)
            {
                for (int i = 1; i < 3; i++)
                {
                    if (!(board[0, i] == ' '))
                    {
                        p2 = false;
                    }
                }

                for (int i = 6; i > 4; i--)
                {
                    if (!(board[0, i] == ' '))
                    {
                        p1 = false;
                    }
                }

                if (!p1 && !p2)
                {
                    throw new InvalidOperationException("Castling is not allowed");
                }
            }
            else
            {
                for (int i = 1; i < 3; i++)
                {
                    if (!(board[7, i] == ' '))
                    {
                        p2 = false;
                    }
                }

                for (int i = 6; i > 4; i--)
                {
                    if (!(board[7, i] == ' '))
                    {
                        p1 = false;
                    }
                }

                if (!p1 && !p2)
                {
                    throw new InvalidOperationException("Castling is not allowed");
                }
            }

            if (p1 && castleSide == "o-o")
            {
                // King-side castling
                board[row, 6] = board[row, 4]; // Move king
                board[row, 5] = board[row, 7]; // Move rook
                board[row, 4] = ' ';
                board[row, 7] = ' ';
            }
            else if(p2 &&  castleSide == "o-o-o")
            {
                // Queen-side castling
                board[row, 2] = board[row, 4]; // Move king
                board[row, 3] = board[row, 0]; // Move rook
                board[row, 4] = ' ';
                board[row, 0] = ' ';
            }

            if (isWhiteTurn)
            {
                whiteKingMoved = true;
            }
            else
            {
                blackKingMoved = true;
            }
            isWhitePerspective = !isWhitePerspective;
            isWhiteTurn = !isWhiteTurn;

        }

        public bool IsInCheck(bool isWhiteKing, char [,] board)
        {
            (int kingRow, int kingCol) = FindKingPosition(isWhiteKing);

            // Check if any opponent piece can attack the king
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    char piece = board[row, col];

                    // Skip empty squares and pieces of the same color
                    if (piece == ' ' ||
                        (isWhiteKing && majorPiecesW.Contains(piece)) ||
                        (!isWhiteKing && majorPiecesB.Contains(piece)))
                        continue;

                    Piece attackingPiece = piece switch
                    {
                        '♕' or '♛' => new Queen(!isWhiteKing, (row, col), board),
                        '♖' or '♜' => new Rook(!isWhiteKing, (row, col), board),
                        '♗' or '♝' => new Bishop(!isWhiteKing, (row, col), board),
                        '♘' or '♞' => new Knight(!isWhiteKing, (row, col), board),
                        '♙' or '♟' => new Pawn(!isWhiteKing, (row, col), board),
                        '♔' or '♚' => new King(!isWhiteKing, (row, col), board),
                        _ => null
                    };

                    if (attackingPiece != null &&
                        attackingPiece.IsValidMove(row, col, kingRow, kingCol, board))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private (int, int) FindKingPosition(bool isWhiteKing)
        {
            char kingPiece = isWhiteKing ? '♔' : '♚';

            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    if (board[row, col] == kingPiece)
                    {
                        return (row, col);
                    }
                }
            }

            throw new InvalidOperationException("King not found on the board");
        }
    }
}