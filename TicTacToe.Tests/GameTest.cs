using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;

namespace TicTacToe.Tests
{
    [TestFixture]
    public class GameTest
    {
        private Game _game;

        [SetUp]
        public void SetUp()
        {
            _game = new Game();
        }

        [Test]
        public void Should_Score_Board_Be_Empty_On_Game_Beginning()
        {
            _game.Fields
                .SelectMany(row => row)
                .Should()
                .OnlyContain(c => c == Player.None);
        }

        [Test]
        public void Should_Score_Board_Be_3x3_Size()
        {
            _game.Fields.Should().HaveCount(3);
            foreach (var row in _game.Fields)
                row.Should().HaveCount(3);
        }

        [Test]
        public void Should_Cross_Player_Have_First_Turn()
        {
            _game.CurrentPlayer.Should().Be(Player.Cross);
        }

        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(4, 1)]
        [TestCase(1, 4)]
        public void Should_Throw_Argument_Exception_Given_Player_Move_Outside_Board(int x, int y)
        {
            Assert.Throws<ArgumentException>(() => _game.Play(x, y));
        }

        [Test]
        public void Should_Player_After_Turn_Change_Filed_On_Score_Board()
        {
            _game.Play(1, 1);
            _game.Fields.First().First().Should().Be(Player.Cross);
        }

        [Test]
        public void Should_Play_Change_Only_One_Field()
        {
            _game.Play(1, 1);
            _game.Fields.SelectMany(row => row)
                .Should().ContainSingle(field => field == Player.Cross);
        }

        [Test]
        public void Should_Change_Player_After_First_Turn()
        {
            _game.Play(1,1);
            _game.CurrentPlayer.Should().Be(Player.Circle);
        }

        [Test]
        public void Should_Throw_Exception_Playing_Twice_On_Same_Field()
        {
            _game.Play(1,1);
            Assert.Throws<ArgumentException>(() => _game.Play(1, 1));
        }

        [Test]
        public void Should_Change_Player_After_Second_Turn()
        {
            _game.Play(1,1);
            _game.Play(1,2);
            _game.CurrentPlayer.Should().Be(Player.Cross);
        }

        [Test]
        public void Should_End_Game_After_Winning_In_Vertical()
        {
            _game.Play(1,1).IsGameEnd.Should().BeFalse();//X
            _game.Play(2,1).IsGameEnd.Should().BeFalse();//O
            _game.Play(1,2).IsGameEnd.Should().BeFalse();//X
            _game.Play(2,2).IsGameEnd.Should().BeFalse();//O
            var gameResult = _game.Play(1,3);//X
            gameResult.IsGameEnd.Should().BeTrue();
            gameResult.Winner.Should().Be(Player.Cross);
        }

        [Test]
        public void Should_End_Game_After_Winning_In_Horizontal()
        {
            _game.Play(1,1).IsGameEnd.Should().BeFalse();//X
            _game.Play(2,1).IsGameEnd.Should().BeFalse();//O
            _game.Play(1,2).IsGameEnd.Should().BeFalse();//X
            _game.Play(2,2).IsGameEnd.Should().BeFalse();//O
            var gameResult = _game.Play(1,3);//X
            gameResult.IsGameEnd.Should().BeTrue();
            gameResult.Winner.Should().Be(Player.Cross);
        }

        [Test]
        public void Should_End_Game_After_Winning_In_Diagonal_From_Top_Left()
        {
            _game.Play(1,1).IsGameEnd.Should().BeFalse();//X
            _game.Play(2,1).IsGameEnd.Should().BeFalse();//O
            _game.Play(2,2).IsGameEnd.Should().BeFalse();//X
            _game.Play(3,1).IsGameEnd.Should().BeFalse();//O
            var gameResult = _game.Play(3,3);//X
            gameResult.IsGameEnd.Should().BeTrue();
            gameResult.Winner.Should().Be(Player.Cross);
        }

        [Test]
        public void Should_End_Game_After_Winning_In_Diagonal_From_Top_Right()
        {
            _game.Play(3,1).IsGameEnd.Should().BeFalse();//X
            _game.Play(2,1).IsGameEnd.Should().BeFalse();//O
            _game.Play(2,2).IsGameEnd.Should().BeFalse();//X
            _game.Play(3,2).IsGameEnd.Should().BeFalse();//O
            var gameResult = _game.Play(1,3);//X
            gameResult.IsGameEnd.Should().BeTrue();
            gameResult.Winner.Should().Be(Player.Cross);
        }

        [Test]
        public void Should_End_Game_When_All_Fields_Are_Occupied_But_Without_Winner()
        {
            _game.Play(1,1).IsGameEnd.Should().BeFalse();//X
            _game.Play(2,1).IsGameEnd.Should().BeFalse();//O
            _game.Play(1,2).IsGameEnd.Should().BeFalse();//X
            _game.Play(2,2).IsGameEnd.Should().BeFalse();//O
            _game.Play(2,3).IsGameEnd.Should().BeFalse();//X
            _game.Play(1,3).IsGameEnd.Should().BeFalse();//O
            _game.Play(3,1).IsGameEnd.Should().BeFalse();//X
            _game.Play(3,2).IsGameEnd.Should().BeFalse();//O
            var gameResult = _game.Play(3, 3);//X

            gameResult.IsGameEnd.Should().BeTrue();
            gameResult.Winner.Should().Be(Player.None);
        }

        [Test]
        public void Should_Convert_Empty_Board_To_String()
        {
            var expectedString = string.Join(Environment.NewLine,
                " | | ",
                "-----",
                " | | ",
                "-----",
                " | | ");
            _game.ToString().Should().Be(expectedString);
        }

        [Test]
        public void Should_Convert_End_game_Board_To_String()
        {
            _game.Play(1, 1);//X
            _game.Play(2, 1);//O
            _game.Play(1, 2);//X
            _game.Play(2, 2);//O
            _game.Play(1, 3);//X

            var expectedString = string.Join(Environment.NewLine,
                "X|X|X",
                "-----",
                "O|O| ",
                "-----",
                " | | ");
            _game.ToString().Should().Be(expectedString);
        }
    }
}
