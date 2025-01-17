using System.Drawing;

namespace AStarResolver.Extensions;

public static class PointExtensions
{
    public static int ManhattanDistanceTo(this Point from, Point to) =>
        Math.Abs(from.X - to.X) + Math.Abs(from.Y - to.Y);
}