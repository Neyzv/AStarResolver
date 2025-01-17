using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using AStarResolver.Enums;

namespace AStarResolver.Services.Coordinates;

public static class CoordinatesService
{
    public static Point GetCoordinates(Point from, Direction direction) =>
        direction switch
        {
            Direction.Top => new Point(from.X, from.Y + 1),
            Direction.Bottom => new Point(from.X, from.Y - 1),
            Direction.Left => new Point(from.X - 1, from.Y),
            Direction.Right => new Point(from.X + 1, from.Y),
            _ => throw new ArgumentException(null, nameof(direction)),
        };

    public static bool IsInGrid(Point p, byte[,] grid)
    {
        var width = grid.GetLength(0);
        var height = grid.GetLength(1);
        
        return p.X >= 0 && p.X < width
            && p.Y >= 0 && p.Y < height;
    }

    public static bool TryGetRandomFreeCell(byte[,] grid, [NotNullWhen(true)] out Point? p,
        Point[]? disallowedPoints = null)
    {
        p = null;
        
        var freePoints = new List<Point>();
        
        var rows = grid.GetLength(0);
        var cols = grid.GetLength(1);

        for (var x = 0; x < rows; x++)
            for (var y = 0; y < cols; y++)
                if (grid[x, y] > (byte)Element.Wall && (disallowedPoints is null || !disallowedPoints.Any(p => p.X == x && p.Y == y)))
                    freePoints.Add(new Point(x, y));
        
        if (freePoints.Count is 0)
            return false;
        
        p = freePoints.ElementAt(Random.Shared.Next(freePoints.Count));

        return true;
    }
}