using System;
using System.Linq;

namespace TicTacToe
{
    public class Game
    {
        public Player[][] Fields { get; }
        public Player CurrentPlayer { get; private set; } = Player.Cross;

        public Game()
        {
            Fields = Enumerable.Range(0, 3)
                .Select(x =>
                {
                    return Enumerable.Range(0, 3)
                        .Select(y => Player.None)
                        .ToArray();
                }).ToArray();
        }

        public GameResult Play(int x, int y)
        {
            ValidateIndex(x);
            ValidateIndex(y);
            ValidateField(x, y);

            Fields[--x][--y] = CurrentPlayer;

            if (IsGameWon())
                return new GameResult {IsGameEnd = true, Winner = CurrentPlayer};
            if (IsDraw())
                return new GameResult {IsGameEnd = true, Winner = Player.None};
            ChangePlayer();
            return new GameResult();
        }

        private bool IsDraw()
        {
            return Fields.SelectMany(row => row)
                .All(field => field != Player.None);
        }

        private bool IsGameWon()
        {
            return IsWonInHorizontal() || IsWonInVertical() || IsWonInDiagonal();
        }

        private bool IsWonInHorizontal()
        {
            for (var row = 0; row < Fields.Length; row++){
                if (CurrentPlayer == Fields[row][0]
                    && CurrentPlayer == Fields[row][1]
                    && CurrentPlayer == Fields[row][2])
                    return true;
            }
            return false;
        }

        private bool IsWonInVertical()
        {
            for (var column = 0; column < Fields.Length; column++){
                if (CurrentPlayer == Fields[0][column]
                    && CurrentPlayer == Fields[1][column]
                    && CurrentPlayer == Fields[2][column])
                    return true;
            }
            return false;
        }

        private bool IsWonInDiagonal()
        {
            if (CurrentPlayer == Fields[0][0]
                && CurrentPlayer == Fields[1][1]
                && CurrentPlayer == Fields[2][2])
                return true;

            if (CurrentPlayer == Fields[0][2]
                && CurrentPlayer == Fields[1][1]
                && CurrentPlayer == Fields[2][0])
                return true;
            return false;
        }

        private void ChangePlayer()
        {
            if (CurrentPlayer == Player.Cross)
                CurrentPlayer = Player.Circle;
            else if (CurrentPlayer == Player.Circle)
                CurrentPlayer = Player.Cross;                
        }

        private void ValidateField(int x, int y)
        {
            if (Fields[--x][--y] != Player.None)
                throw new ArgumentException("Field is alredy changed");
        }

        private void ValidateIndex(int index)
        {
            if (index <= 0 || index >= 4)
                throw new ArgumentException("Index out of range");
        }

        public override string ToString()
        {
            var rowsString = Fields.Select(row => string.Join("|", row.Select(field => (char)field)));
            return string.Join($"{Environment.NewLine}-----{Environment.NewLine}", rowsString);
        }
    }

    public class GameResult
    {
        public bool IsGameEnd { get; set; }
        public Player? Winner { get; set; }
    }
}
