using System;
using System.Linq;

namespace MarsRover
{
    public class MovementSystem
    {
        private Terrain terrain;
        private Rover rover;

        public MovementSystem(Terrain terrain, Rover rover)
        {
            this.terrain = terrain;
            this.rover = rover;
        }

        public void Process(string commands)
        {
            var commandChain = commands.Select<char, Action>(c => c switch
            {
                'F' => () => { EnsureCanMove(MoveDirection.F); rover.Move(MoveDirection.F); }
                ,
                'B' => () => { EnsureCanMove(MoveDirection.B); rover.Move(MoveDirection.B); }
                ,
                'L' => () => rover.RotateLeft(),
                'R' => () => rover.RotateRight(),
                _ => throw new NotSupportedException()
            });
            foreach (var command in commandChain)
            {
                try
                {
                    command();
                }
                catch (Exception ex)
                {
                    break;
                }
            }
        }
        private void EnsureCanMove(MoveDirection direction)
        {
            var nextPosition = rover.NextPosition(direction);
            _ = terrain.HasObstacle(nextPosition.X, nextPosition.Y) ? throw new Exception() : false;
        }
    }
}
