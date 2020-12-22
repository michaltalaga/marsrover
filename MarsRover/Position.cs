using System.Diagnostics;

namespace MarsRover
{
    [DebuggerDisplay("{value}")]
    public struct Position
    {
        int value { get; }
        int size { get; }
        public Position(int value, int size)
        {
            this.value = value;
            this.size = size;
        }
        public Position Next => new Position((value + 1) % size, size);
        public Position Prev => new Position((value - 1 + size) % size, size);


        public static implicit operator int(Position position) => position.value;
    }
}
