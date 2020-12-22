using System;
using System.Diagnostics;

namespace MarsRover
{
    [DebuggerDisplay("{X},{Y},{FacingDirection}")]
    public class Rover
    {
        public Rover(Position x, Position y, FacingDirection facingDirection)
        {
            X = x;
            Y = y;
            FacingDirection = facingDirection;
        }

        public Position X { get; private set; }
        public Position Y { get; private set; }
        public FacingDirection FacingDirection { get; private set; }
        public FacingDirection OppositeFacingDirection
        {
            get
            {
                var numberOfDirections = Enum.GetValues(typeof(FacingDirection)).Length;
                return (FacingDirection)(((int)FacingDirection + numberOfDirections / 2) % numberOfDirections);
            }
        }

        public void Move(MoveDirection moveDirection)
        {
            (X, Y) = NextPosition(moveDirection);
        }
        public (Position X, Position Y) NextPosition(MoveDirection direction) => direction switch
        {
            MoveDirection.F => NextPosition(FacingDirection),
            MoveDirection.B => NextPosition(OppositeFacingDirection),
            _ => throw new NotSupportedException()
        };
        private (Position X, Position Y) NextPosition(FacingDirection direction) => direction switch
        {
            FacingDirection.N => (X, Y.Next),
            FacingDirection.S => (X, Y.Prev),
            FacingDirection.E => (X.Next, Y),
            FacingDirection.W => (X.Prev, Y),
            _ => throw new NotSupportedException()
        };
        public void RotateLeft()
        {
            var numberOfDirections = Enum.GetValues(typeof(FacingDirection)).Length;
            var newFacingDirection = (int)FacingDirection - 1 + numberOfDirections;
            FacingDirection = (FacingDirection)(newFacingDirection % numberOfDirections);
        }
        public void RotateRight()
        {
            var numberOfDirections = Enum.GetValues(typeof(FacingDirection)).Length;
            var newFacingDirection = (int)FacingDirection + 1;
            FacingDirection = (FacingDirection)(newFacingDirection % numberOfDirections);
        }
        public string ToStatusString()
        {
            return $"{(int)X},{(int)Y},{FacingDirection}";
        }
    }
}
