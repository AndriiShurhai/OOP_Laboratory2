using System;
using System.Collections.Generic;

namespace Chess
{
    public class ChessGame
    {
        private Board board;
        private bool isGameOver;
        private bool isWhiteTurn;

        public ChessGame()
        {
            board = new Board();
            isGameOver = false;
            isWhiteTurn = true;
        }

        public void Start()
        {
            while (!isGameOver)
            {
                Console.Clear();
                board.DisplayBoard();
                Console.WriteLine($"{(isWhiteTurn ? "White" : "Black")} to move");

                try
                {
                    string fromPosition = GetPlayerInput("Enter the position of the piece to move (e.g., e2): ");
                    if (fromPosition == "o-o" || fromPosition == "o-o-o")
                    {
                        board.Castle(isWhiteTurn, fromPosition);
                        continue;
                    }
                    string toPosition = GetPlayerInput("Enter the destination position (e.g., e4): ");

                    board.MovePiece(fromPosition, toPosition);
                    if (CheckForCheckmate())
                    {
                        Console.Clear();
                        board.DisplayBoard();

                        Console.WriteLine("Game over");
                        if (isWhiteTurn)
                        {
                            Console.WriteLine("White won");
                        }
                        else
                        {
                            Console.WriteLine("Black won");
                        }

                        return;
                    }

                    isWhiteTurn = !isWhiteTurn;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Invalid move: {ex.Message}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        private string GetPlayerInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine().Trim().ToLower();

                if (input == "o-o" || input == "o-o-o")
                {
                    return input;
                }

                if (input.Length == 2 &&
                    input[0] >= 'a' && input[0] <= 'h' &&
                    input[1] >= '1' && input[1] <= '8')
                {
                    return input;
                }

                Console.WriteLine("Invalid input. Please use algebraic notation (e.g., e2).");
            }
        }

        private bool CheckForCheckmate()
        {
            if (board.IsCheckmate(!(isWhiteTurn)))
            {
                return true;
            }
            return false;
        }

        private void CheckForStalemate()
        {
            // доробить
        }
    }

    // Main program to run the game
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Chess!");
            ChessGame game = new ChessGame();
            game.Start();
        }
    }
}