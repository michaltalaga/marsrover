using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Xunit;

namespace MarsRover
{
    public class Tests
    {
        [Fact]
        public void TestAllTheThings()
        {
            string map = @"
.....
..#..
.....
.###.
.....
..@..
";
            var rows = map.Trim().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Reverse().ToArray();
            var obstacles = new List<Point>();
            Rover rover = null;
            for (int y = 0; y < rows.Length; y++)
            {
                for (int x = 0; x < rows[y].Length; x++)
                {
                    if (rows[y][x] == '#') obstacles.Add(new Point(x, y));
                    else if (rows[y][x] == '@') rover = new Rover(new Position(x, rows[y].Length), new Position(y, rows.Length), FacingDirection.N);
                }
            }
            var terrain = new Terrain(rows.Length, rows[0].Length, obstacles);
            var movementSystem = new MovementSystem(terrain, rover);
            movementSystem.Process("FFF");
            Assert.Equal(new { X = 2, Y = 1 }, new { X = (int)rover.X, Y = (int)rover.Y });

        }
        [Fact]
        public void TerrainWorksLikeCharm()
        {
            var terrain = new Terrain(3, 3, new[] { new Point(1, 1) });
            Assert.True(terrain.HasObstacle(1, 1));
            Assert.False(terrain.HasObstacle(0, 0));
            var terrain2 = new Terrain(3, 3);
            Assert.False(terrain2.HasObstacle(0, 0));
        }
        [Fact]
        public void PositionSpotOn()
        {
            var position = new Position(0, 3);
            Assert.Equal(0, position);
            Assert.Equal(1, position.Next);
            Assert.Equal(2, position.Next.Next);
            Assert.Equal(0, position.Next.Next.Next);
            Assert.Equal(2, position.Prev);
            Assert.Equal(1, position.Prev.Prev);
        }

        [Fact]
        public void NothingCanStopMe()
        {
            var x = new Position(0, 3);
            var y = new Position(0, 3);
            var rover = new Rover(x, y, FacingDirection.N);
            rover.Move(MoveDirection.F);
            Assert.Equal(1, rover.Y);
            rover.Move(MoveDirection.F);
            Assert.Equal(2, rover.Y);
            rover.Move(MoveDirection.F);
            Assert.Equal(0, rover.Y);
        }
        [Fact]
        public void Runaway()
        {
            var x = new Position(0, 3);
            var y = new Position(1, 3);
            var rover = new Rover(x, y, FacingDirection.N);
            rover.Move(MoveDirection.B);
            Assert.Equal(0, rover.Y);
        }
        [Fact]
        public void SpinToWin()
        {
            var x = new Position(0, 3);
            var y = new Position(0, 3);
            var rover = new Rover(x, y, FacingDirection.N);
            rover.RotateLeft();
            Assert.Equal(FacingDirection.W, rover.FacingDirection);
            Assert.Equal(FacingDirection.E, rover.OppositeFacingDirection);
            rover.RotateRight();
            Assert.Equal(FacingDirection.N, rover.FacingDirection);
            Assert.Equal(FacingDirection.S, rover.OppositeFacingDirection);
            rover.RotateRight();
            rover.RotateRight();
            rover.RotateRight();
            Assert.Equal(FacingDirection.W, rover.FacingDirection);
            Assert.Equal(FacingDirection.E, rover.OppositeFacingDirection);
            rover.RotateRight();
            Assert.Equal(FacingDirection.N, rover.FacingDirection);
            Assert.Equal(FacingDirection.S, rover.OppositeFacingDirection);
        }
        [Theory]
        [InlineData("F", "0,1,N")]
        [InlineData("FF", "0,2,N")]
        [InlineData("FFF", "0,0,N")]
        [InlineData("FFB", "0,1,N")]
        [InlineData("FFRF", "1,2,E")]
        [InlineData("FLF", "2,1,W")]
        public void MovinAround(string commands, string statusString)
        {
            var terrain = new Terrain(3, 3, new[] { new Point(1, 1) });
            var x = new Position(0, terrain.Width);
            var y = new Position(0, terrain.Height);
            var rover = new Rover(x, y, FacingDirection.N);
            var movementSystem = new MovementSystem(terrain, rover);
            movementSystem.Process(commands);
            Assert.Equal(statusString, rover.ToStatusString());
        }
        [Theory]
        [InlineData("FRF", "0,1,E")]
        [InlineData("FRFF", "0,1,E")]
        [InlineData("FLB", "0,1,W")]
        public void ICrashIntoSomething(string commands, string statusString)
        {
            var terrain = new Terrain(3, 3, new[] { new Point(1, 1) });
            var x = new Position(0, terrain.Width);
            var y = new Position(0, terrain.Height);
            var rover = new Rover(x, y, FacingDirection.N);
            var movementSystem = new MovementSystem(terrain, rover);
            movementSystem.Process(commands);
            Assert.Equal(statusString, rover.ToStatusString());
        }


    }
}
