using System;
using System.Collections.Generic;

namespace Chess
{
    public partial class Board
    {
        private char[,] board;
        private const int Size = 8;
        protected bool isWhitePerspective;
        private bool isWhiteTurn = true;


        private char[] majorPiecesW = { '♖', '♘', '♗', '♕', '♔', '♗', '♘', '♖' , '♙' };
        private char[] majorPiecesB = { '♜', '♞', '♝', '♛', '♚', '♝', '♞', '♜' , '♟' };

        private static readonly Dictionary<char, int> FileToColumn = new Dictionary<char, int>
        {
            {'a', 0}, {'b', 1}, {'c', 2}, {'d', 3},
            {'e', 4}, {'f', 5}, {'g', 6}, {'h', 7}
        };

        private static readonly Dictionary<int, char> ColumnToFile = new Dictionary<int, char>
        {
            {0, 'a'}, {1, 'b'}, {2, 'c'}, {3, 'd'},
            {4, 'e'}, {5, 'f'}, {6, 'g'}, {7, 'h'}
        };

        public Board(bool isWhitePerspective = true)
        {
            board = new char[Size, Size];
            this.isWhitePerspective = isWhitePerspective;
            InitializeBoard();
        }

        public void InitializeBoard()
        {
            setupMainPieces(true);
            setupMainPieces(false);

            for (int i = 2; i < Size - 2; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    board[i, j] = ' ';
                }
            }
        }

        private void setupMainPieces(bool isWhite)
        {
            for (int i = 0; i < Size; i++)
            {
                Pawn pawn = new Pawn(isWhite, (isWhite ? 1 : 6, i), board);
            }

            new Rook(isWhite, isWhite ? (0, 0) : (7, 0), board);
            new Rook(isWhite, isWhite ? (0, 7) : (7, 7), board);

            new Knight(isWhite, isWhite ? (0, 1) : (7, 1), board);
            new Knight(isWhite, isWhite ? (0, 6) : (7, 6), board);

            new Bishop(isWhite, isWhite ? (0, 2) : (7, 2), board);
            new Bishop(isWhite, isWhite ? (0, 5) : (7, 5), board);

            new King(isWhite, isWhite ? (0, 4) : (7, 4), board);
            new Queen(isWhite, isWhite ? (0, 3) : (7, 3), board);
        }

        private (int row, int col) ConvertNotationToIndices(string position)
        {
            if (position.Length != 2)
                throw new ArgumentException("Invalid chess notation");

            char file = char.ToLower(position[0]);
            int rank = position[1] - '1';

            if (!FileToColumn.ContainsKey(file) || rank < 0 || rank >= Size)
                throw new ArgumentException("Invalid chess notation");

            int col = FileToColumn[file];
            int row = rank;


            return (row, col);
        }


        private string ConvertIndicesToNotation(int row, int col)
        {
            if (row < 0 || row >= Size || col < 0 || col >= Size)
                throw new ArgumentException("Indices out of bounds");

            int rank = isWhitePerspective ? Size - 1 - row : row;
            char file = ColumnToFile[col];

            return $"{file}{rank + 1}";
        }

        public void MovePiece(string fromPosition, string toPosition)
        {
            var (fromRow, fromCol) = ConvertNotationToIndices(fromPosition);
            var (toRow, toCol) = ConvertNotationToIndices(toPosition);

            char piece = board[fromRow, fromCol];

            if (piece == ' ')
                throw new InvalidOperationException("No piece at the selected position.");

            // Validate turn color
            bool isPieceWhite = majorPiecesW.Contains(piece);
            if ((isWhiteTurn && !isPieceWhite) || (!isWhiteTurn && isPieceWhite))
                throw new InvalidOperationException("It's not your turn.");

            if (!IsValidMove(fromRow, fromCol, toRow, toCol))
                throw new InvalidOperationException("Invalid move for this piece.");

            if (WouldMoveResultInCheck(fromPosition, toPosition))
            {
                throw new InvalidOperationException("This move would put your king in check.");
            }

            board[toRow, toCol] = board[fromRow, fromCol];
            board[fromRow, fromCol] = ' ';
            
            isWhitePerspective = !isWhitePerspective;
            isWhiteTurn = !isWhiteTurn;
        }


        public bool IsValidMove(int fromRow, int fromCol, int toRow, int toCol)
        {
            char pieceChar = board[fromRow, fromCol];

            Piece piece = pieceChar switch
            {
                '♔' or '♚' => new King(pieceChar == '♔', (fromRow, fromCol), board),
                '♕' or '♛' => new Queen(pieceChar == '♕', (fromRow, fromCol), board),
                '♖' or '♜' => new Rook(pieceChar == '♖', (fromRow, fromCol), board),
                '♗' or '♝' => new Bishop(pieceChar == '♗', (fromRow, fromCol), board),
                '♘' or '♞' => new Knight(pieceChar == '♘', (fromRow, fromCol), board),
                '♙' or '♟' => new Pawn(pieceChar == '♙', (fromRow, fromCol), board),
                _ => throw new ArgumentException("Unknown piece type")
            };

            // Check if the destination square is occupied by a piece of the same color
            if (board[toRow, toCol] != ' ')
            {
                if ((piece.IsWhite && majorPiecesW.Contains(board[toRow, toCol])) ||
                    (!piece.IsWhite && majorPiecesB.Contains(board[toRow, toCol])))
                    return false;
            }

            return piece.IsValidMove(fromRow, fromCol, toRow, toCol, board);
        }

        public bool WouldMoveResultInCheck(string fromPosition, string toPosition)
        {
            char[,] tempBoard = (char[,])board.Clone();
            bool currentTurn = isWhiteTurn;

            var (fromRow, fromCol) = ConvertNotationToIndices(fromPosition);
            var (toRow, toCol) = ConvertNotationToIndices(toPosition);

            tempBoard[toRow, toCol] = tempBoard[fromRow, fromCol];
            tempBoard[fromRow, fromCol] = ' ';

            return IsInCheck(currentTurn, tempBoard);
        }


        public void DisplayBoard()
        {
            Console.WriteLine("\n   Chess Board");
            Console.WriteLine("    " + string.Join("  ", isWhitePerspective ?
                new[] { "a", "b", "c", "d", "e", "f", "g", "h" } :
                new[] { "h", "g", "f", "e", "d", "c", "b", "a" }));

            for (int i = 0; i < Size; i++)
            {
                int row = isWhitePerspective ? Size - 1 - i : i;
                Console.Write($" {row + 1} "); // Display rank numbers

                for (int j = 0; j < Size; j++)
                {
                    int col = isWhitePerspective ? j : Size - 1 - j;

                    // Alternate background colors to create a chess board effect
                    if ((row + col) % 2 == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.Cyan;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                    }

                    char piece = board[row, col];
                    if (majorPiecesW.Contains(piece))
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (majorPiecesB.Contains(piece))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }

                    Console.Write(piece == ' ' ? " · " : $" {piece} ");

                    Console.ResetColor();
                }
                Console.WriteLine(); 
            }

            Console.WriteLine("\n   Current Turn: " + (isWhiteTurn ? "White" : "Black"));
            Console.WriteLine("   Perspective: " + (isWhitePerspective ? "White" : "Black"));
        }


        public char GetPieceAt(string position)
        {
            var (row, col) = ConvertNotationToIndices(position);
            return board[row, col];
        }

        public bool IsPositionEmpty(string position)
        {
            var (row, col) = ConvertNotationToIndices(position);
            return board[row, col] == ' ';
        }

        public bool IsCheckmate(bool isWhiteKing)
        {
            if (!IsInCheck(isWhiteKing, board))
                return false;

            // Try all possible moves for pieces of the king's color
            for (int fromRow = 0; fromRow < Size; fromRow++)
            {
                for (int fromCol = 0; fromCol < Size; fromCol++)
                {
                    char piece = board[fromRow, fromCol];
                    bool isPieceOfColor = isWhiteKing ?
                        majorPiecesW.Contains(piece) :
                        majorPiecesB.Contains(piece);

                    if (isPieceOfColor)
                    {
                        // Try moving to all possible squares
                        for (int toRow = 0; toRow < Size; toRow++)
                        {
                            for (int toCol = 0; toCol < Size; toCol++)
                            {
                                try
                                {
                                    MovePiece(
                                        ConvertIndicesToNotation(fromRow, fromCol),
                                        ConvertIndicesToNotation(toRow, toCol)
                                    );
                                    if (!IsInCheck(isWhiteKing, board)) return false;
                                    else
                                    {
                                        continue;
                                    }
                                }
                                catch
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}
