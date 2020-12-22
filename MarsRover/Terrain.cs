using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MarsRover
{
    public class Terrain
    {
        IEnumerable<Point> obstacles;
        public int Height { get; }
        public int Width { get; }

        public Terrain(int height, int width, IEnumerable<Point> obstacles = null)
        {
            Height = height;
            Width = width;
            this.obstacles = obstacles;
        }

        public bool HasObstacle(int x, int y)
        {
            return obstacles?.SingleOrDefault(p => p.X == x && p.Y == y).IsEmpty == false;
        }
    }
}
