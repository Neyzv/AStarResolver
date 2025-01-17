using AStarResolver.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using AStarResolver.Enums;
using AStarResolver.Services.Coordinates;

namespace AStarResolver.Services.Grid;

public static class GridGeneratorService
{
    /// <summary>
    /// Generate a grid with obstacle (false) and accessible cell (true) based on Random Walk algorithm.
    /// </summary>
    /// <param name="minWidth"></param>
    /// <param name="maxWidth"></param>
    /// <param name="minHeight"></param>
    /// <param name="maxHeight"></param>
    /// <param name="steps"></param>
    /// <returns></returns>
    public static byte[,] GenerateGrid(
        [Range(1, byte.MaxValue)] byte minWidth,
        [Range(1, byte.MaxValue)] byte maxWidth,
        [Range(1, byte.MaxValue)] byte minHeight,
        [Range(1, byte.MaxValue)] byte maxHeight,
        [Range(1, 500)] int minSteps,
        [Range(1, 500)] int maxSteps,
        [Range(1, byte.MaxValue)] byte minIslandCount,
        [Range(1, byte.MaxValue)] byte maxIslandCount)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(maxHeight, minHeight);
        ArgumentOutOfRangeException.ThrowIfLessThan(maxWidth, minWidth);
        ArgumentOutOfRangeException.ThrowIfLessThan(maxSteps, minSteps);
        ArgumentOutOfRangeException.ThrowIfLessThan(maxIslandCount, minIslandCount);

        var width = Random.Shared.Next(minHeight, maxHeight + 1);
        var height = Random.Shared.Next(minWidth, maxWidth + 1);
        var steps = Random.Shared.Next(minSteps, maxSteps + 1);
        var islandCount = Random.Shared.Next(minIslandCount, maxIslandCount + 1);

        var grid = new byte[width, height];

        var directions = Enum.GetValues<Direction>();

        for (var a = 0; a < islandCount; a++)
        {
            var currentPosition = new Point(Random.Shared.Next(width), Random.Shared.Next(height));
            grid[currentPosition.X, currentPosition.Y] = (byte)Element.FreeCell;

            for (var i = 0; i < steps; i++)
                foreach (var val in directions.Shuffle())
                {
                    var nextPosition = CoordinatesService.GetCoordinates(currentPosition, val);

                    if (CoordinatesService.IsInGrid(nextPosition, grid))
                    {
                        grid[nextPosition.X, nextPosition.Y] = (byte)Element.FreeCell;

                        currentPosition = nextPosition;

                        break;
                    }
                }
        }

        return grid;
    }
}