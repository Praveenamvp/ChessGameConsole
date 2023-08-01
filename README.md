ChessGame
ChessGame is a console-based chess application written in C#. It allows two players to play chess on the command line by entering moves in the format of 'a1', 'b2', etc. The game supports standard chess rules and movement patterns for each chess piece.

How to Run the Game
Open a C# development environment (e.g., Visual Studio).
Create a new console application project.
Copy and paste the entire code provided in the Program class.
Build and run the application.

Game Rules
The game starts with White as the first player.
To move a piece, enter the position of the piece to move (e.g., 'e2') and press Enter.
The application will then display the valid moves for that piece.
Enter the new position to move the piece (e.g., 'e4') and press Enter.
If the move is valid, the piece will be moved to the new position, and the board will be updated.
If the move captures an opponent's piece, it will be displayed on the console.
The game continues until one player wins or the players decide to exit.

Commands
'exit': Ends the game and exits the application.
'Print': Displays the current state of the board without making any moves.

Chess Piece Representations
W_K: White King
W_Q: White Queen
W_B: White Bishop
W_N: White Knight
W_R: White Rook
W_P: White Pawn
B_K: Black King
B_Q: Black Queen
B_B: Black Bishop
B_N: Black Knight
B_R: Black Rook
B_P: Black Pawn




