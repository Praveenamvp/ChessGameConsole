namespace ChessGame
{
    public enum Color
    {
        White,
        Black
    }

    public enum PieceType
    {
        King,
        Queen,
        Bishop,
        Knight,
        Rook,
        Pawn
    }

    public class Piece
    {
        public Color Color { get; }
        public PieceType Type { get; }

        public Piece(Color color, PieceType type)
        {
            Color = color;
            Type = type;
        }
    }

    public class Board
    {
        private const int BoardSize = 8;
        private Piece[,] pieces;

        public Board()
        {
            pieces = new Piece[BoardSize, BoardSize];
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            pieces[0, 0] = new Piece(Color.White, PieceType.Rook);
            pieces[0, 1] = new Piece(Color.White, PieceType.Knight);
            pieces[0, 2] = new Piece(Color.White, PieceType.Bishop);
            pieces[0, 3] = new Piece(Color.White, PieceType.Queen);
            pieces[0, 4] = new Piece(Color.White, PieceType.King);
            pieces[0, 5] = new Piece(Color.White, PieceType.Bishop);
            pieces[0, 6] = new Piece(Color.White, PieceType.Knight);
            pieces[0, 7] = new Piece(Color.White, PieceType.Rook);

            for (int i = 0; i < BoardSize; i++)
            {
                pieces[1, i] = new Piece(Color.White, PieceType.Pawn);
            }

            pieces[7, 0] = new Piece(Color.Black, PieceType.Rook);
            pieces[7, 1] = new Piece(Color.Black, PieceType.Knight);
            pieces[7, 2] = new Piece(Color.Black, PieceType.Bishop);
            pieces[7, 3] = new Piece(Color.Black, PieceType.Queen);
            pieces[7, 4] = new Piece(Color.Black, PieceType.King);
            pieces[7, 5] = new Piece(Color.Black, PieceType.Bishop);
            pieces[7, 6] = new Piece(Color.Black, PieceType.Knight);
            pieces[7, 7] = new Piece(Color.Black, PieceType.Rook);

            for (int i = 0; i < BoardSize; i++)
            {
                pieces[6, i] = new Piece(Color.Black, PieceType.Pawn);
            }
        }

        public Piece GetPiece(int row, int col)
        {
            if (row < 0 || row >= BoardSize || col < 0 || col >= BoardSize)
            {
                return null;
            }

            return pieces[row, col];
        }

        public void MovePiece(int fromRow, int fromCol, int toRow, int toCol)
        {
            pieces[toRow, toCol] = pieces[fromRow, fromCol];
            pieces[fromRow, fromCol] = null;
        }
    }

    public class ChessGame
    {
        private Board board;
        private Color currentPlayer;

        private Dictionary<PieceType, List<Tuple<int, int>>> possibleMoves;

        private string recordFilePath = "chess_record.txt";

        public ChessGame()
        {
            board = new Board();
            currentPlayer = Color.White;

            InitializePossibleMoves();
        }

        private void InitializePossibleMoves()
        {
            possibleMoves = new Dictionary<PieceType, List<Tuple<int, int>>>
            {
                { PieceType.King, new List<Tuple<int, int>> { Tuple.Create(1, 0), Tuple.Create(-1, 0), Tuple.Create(0, 1), Tuple.Create(0, -1) } },
                { PieceType.Queen, new List<Tuple<int, int>> { Tuple.Create(1, 1), Tuple.Create(1, -1), Tuple.Create(-1, 1), Tuple.Create(-1, -1), Tuple.Create(1, 0), Tuple.Create(-1, 0), Tuple.Create(0, 1), Tuple.Create(0, -1) } },
                { PieceType.Bishop, new List<Tuple<int, int>> { Tuple.Create(1, 1), Tuple.Create(1, -1), Tuple.Create(-1, 1), Tuple.Create(-1, -1) } },
                { PieceType.Knight, new List<Tuple<int, int>> { Tuple.Create(2, 1), Tuple.Create(2, -1), Tuple.Create(-2, 1), Tuple.Create(-2, -1), Tuple.Create(1, 2), Tuple.Create(1, -2), Tuple.Create(-1, 2), Tuple.Create(-1, -2) } },
                { PieceType.Rook, new List<Tuple<int, int>> { Tuple.Create(1, 0), Tuple.Create(-1, 0), Tuple.Create(0, 1), Tuple.Create(0, -1) } },
                { PieceType.Pawn, new List<Tuple<int, int>> { Tuple.Create(1, 0) } } // Pawn moves only forward one step
            };
        }

        public void StartGame()
        {
            while (true)
            {
                Console.Clear();
                PrintBoard();
                Console.WriteLine();

                Console.WriteLine($"Current player: {(currentPlayer == Color.White ? "White" : "Black")}");
                Console.WriteLine();

                Console.WriteLine("Enter 'exit' to quit ");
                Console.WriteLine("Enter 'Print' to show the current state");
                Console.WriteLine("Enter your coin (e.g., 'b1')");



                string input = Console.ReadLine().Trim().ToLower();

                if (input == "exit")
                {
                    Console.WriteLine("Game ended. Thanks for playing!");
                    break;
                }

                if (input == "print")
                {
                    continue;
                }

                if (input.EndsWith("--help"))
                {
                    HandleHelp(input);
                    continue;
                }

                if (IsValidInput(input))
                {
                    int fromRow = input[1] - '1';
                    int fromCol = input[0] - 'a';

                    if (board.GetPiece(fromRow, fromCol) == null || board.GetPiece(fromRow, fromCol).Color != currentPlayer)
                    {
                        Console.WriteLine("Invalid move. Choose your own piece.");
                        continue;
                    }

                    PieceType pieceType = board.GetPiece(fromRow, fromCol).Type;
                    List<Tuple<int, int>> moves = GetPossibleMoves(fromRow, fromCol, pieceType);
                    PrintPossibleMoves(moves);

                    Console.Write("Enter the new position to move the piece: ");
                    string newPosition = Console.ReadLine().Trim().ToLower();

                    if (IsValidInput(newPosition))
                    {
                        int toRow = newPosition[1] - '1';
                        int toCol = newPosition[0] - 'a';

                        if (IsMoveValid(moves, toRow, toCol))
                        {
                            MovePieceAndUpdateRecord(fromRow, fromCol, toRow, toCol, pieceType);
                            currentPlayer = (currentPlayer == Color.White) ? Color.Black : Color.White;
                        }
                        else
                        {
                            Console.WriteLine("Invalid move. Please choose a valid position.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid position.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid position.");
                }
            }
        }

        private bool IsValidInput(string input)
        {
            if (input.Length != 2)
            {
                return false;
            }

            char colChar = input[0];
            char rowChar = input[1];

            return colChar >= 'a' && colChar <= 'h' && rowChar >= '1' && rowChar <= '8';
        }

        private List<Tuple<int, int>> GetPossibleMoves(int row, int col, PieceType pieceType)
        {
            List<Tuple<int, int>> moves = new List<Tuple<int, int>>();

            foreach (var move in possibleMoves[pieceType])
            {
                int newRow = row + move.Item1;
                int newCol = col + move.Item2;

                if (IsValidPosition(newRow, newCol))
                {
                    moves.Add(Tuple.Create(newRow, newCol));
                }
            }

            return moves;
        }

        private void PrintPossibleMoves(List<Tuple<int, int>> moves)
        {
            if (moves.Count == 0)
            {
                Console.WriteLine("No valid moves for the selected piece.");
            }
            else
            {
                Console.Write("Possible moves: ");
                foreach (var move in moves)
                {
                    char colChar = (char)('a' + move.Item2);
                    Console.Write($"{colChar}{move.Item1 + 1} ");
                }
                Console.WriteLine();
            }
        }

        private bool IsMoveValid(List<Tuple<int, int>> moves, int toRow, int toCol)
        {
            foreach (var move in moves)
            {
                if (move.Item1 == toRow && move.Item2 == toCol)
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsValidPosition(int row, int col)
        {
            return row >= 0 && row < 8 && col >= 0 && col < 8;
        }

        private void MovePieceAndUpdateRecord(int fromRow, int fromCol, int toRow, int toCol, PieceType pieceType)
        {
            Piece capturedPiece = board.GetPiece(toRow, toCol);

            board.MovePiece(fromRow, fromCol, toRow, toCol);

            if (capturedPiece != null)
            {
                Console.WriteLine($"The {currentPlayer} {capturedPiece.Type} in {GetCellPosition(toRow, toCol)} has been captured by {currentPlayer} {pieceType}");
            }
            else
            {
                Console.WriteLine($"{currentPlayer} {pieceType} at {GetCellPosition(fromRow, fromCol)} has been moved to {GetCellPosition(toRow, toCol)}");

            }

            RecordMove($"{currentPlayer} {pieceType} at {GetCellPosition(fromRow, fromCol)} moved to {GetCellPosition(toRow, toCol)}");
        }

        private string GetCellPosition(int row, int col)
        {
            char colChar = (char)('a' + col);
            return $"{colChar}{row + 1}";
        }

        private void HandleHelp(string input)
        {
            string position = input.Substring(0, input.IndexOf(" --help"));
            int row = position[1] - '1';
            int col = position[0] - 'a';

            PieceType pieceType = board.GetPiece(row, col).Type;
            List<Tuple<int, int>> moves = GetPossibleMoves(row, col, pieceType);
            bool canCapture = CanCaptureOpponentPiece(moves, currentPlayer == Color.White ? Color.Black : Color.White);

            if (canCapture)
            {
                Console.WriteLine($"The {currentPlayer} {pieceType} in {GetCellPosition(row, col)} can capture your opponent's piece.");
            }
            else
            {
                Console.WriteLine("Safe place");
            }
        }

        private bool CanCaptureOpponentPiece(List<Tuple<int, int>> moves, Color opponentColor)
        {
            foreach (var move in moves)
            {
                Piece piece = board.GetPiece(move.Item1, move.Item2);
                if (piece != null && piece.Color == opponentColor)
                {
                    return true;
                }
            }
            return false;
        }

        private void PrintBoard()
        {
            Console.WriteLine("**********************************************************************");

            Console.WriteLine("       a       b       c       d       e       f       g       h");
            Console.WriteLine();


            for (int row = 0; row < 8; row++)
            {
                Console.Write(row + 1);
                for (int col = 0; col < 8; col++)
                {
                    Piece piece = board.GetPiece(row, col);

                    if (piece != null)
                    {
                        string colorPrefix = (piece.Color == Color.White) ? "    W_" : "    B_";
                        string pieceSymbol = colorPrefix + piece.Type.ToString().Substring(0, 1);
                        Console.Write($" {pieceSymbol}");
                    }
                    else
                    {
                        Console.Write("     |_|");
                    }
                }
                Console.WriteLine();

                Console.WriteLine();


            }
            Console.WriteLine("***********************************************************************");

        }

        private void RecordMove(string move)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(recordFilePath, true))
                    {
                        writer.WriteLine(move);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while recording move: {ex.Message}");
                }
            }
        }

        class Program
        {
            static void Main(string[] args)
            {
                ChessGame chessGame = new ChessGame();
                chessGame.StartGame();
            }
        }
    } 