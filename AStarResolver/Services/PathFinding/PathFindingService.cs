using AStarResolver.Extensions;
using AStarResolver.Models.PathFinding;
using System.Drawing;
using AStarResolver.Enums;
using AStarResolver.Services.Coordinates;

namespace AStarResolver.Services.PathFinding;

public static class PathFindingService
{
    private static Point[] FindShortestPath(byte[,] grid, Point startPoint, Point endPoint)
    {
        var activePoints = new Dictionary<int, Dictionary<int, PointInformations>>
        {
            [startPoint.X] = new()
            {
                [startPoint.Y] = new PointInformations(startPoint, startPoint.ManhattanDistanceTo(endPoint))
            }
        };
        var visitedPoints = new Dictionary<int, HashSet<int>>();

        var pointToCheck = activePoints[startPoint.X][startPoint.Y];
        var directions = Enum.GetValues<Direction>();

        while (pointToCheck is not null
               && (pointToCheck.Point.X != endPoint.X || pointToCheck.Point.Y != endPoint.Y))
        {
            if (!visitedPoints.ContainsKey(pointToCheck.Point.X))
                visitedPoints.Add(pointToCheck.Point.X, new HashSet<int>());

            visitedPoints[pointToCheck.Point.X].Add(pointToCheck.Point.Y);

            if (!activePoints.TryGetValue(pointToCheck.Point.X, out var yDict)
                || !yDict.ContainsKey(pointToCheck.Point.Y))
                throw new KeyNotFoundException($"Can not find point {pointToCheck} in {nameof(activePoints)}.");

            yDict.Remove(pointToCheck.Point.Y);

            if (yDict.Count is 0)
                activePoints.Remove(pointToCheck.Point.Y);

            foreach (var direction in directions)
            {
                var point = CoordinatesService.GetCoordinates(pointToCheck.Point, direction);
                
                if (!CoordinatesService.IsInGrid(point, grid)
                    || grid[point.X, point.Y] is (byte)Element.Wall
                    || (visitedPoints.TryGetValue(point.X, out var yDict2) && yDict2.Contains(point.Y)))
                    continue;
                
                var pointInformations = new PointInformations(point, point.ManhattanDistanceTo(endPoint));
                pointInformations.SetParent(pointToCheck);
                
                if (!activePoints.ContainsKey(pointInformations.Point.X))
                    activePoints[pointInformations.Point.X] = new Dictionary<int, PointInformations>();
                
                if (!activePoints[pointInformations.Point.X].TryGetValue(pointInformations.Point.Y, out var existantPointInfos)
                    || existantPointInfos.TotalCost > pointInformations.TotalCost)
                    activePoints[pointInformations.Point.X][pointInformations.Point.Y] = pointInformations;
            }

            pointToCheck = activePoints
                .Values
                .SelectMany(x => x.Values)
                .OrderBy(x => x.TotalCost)
                .FirstOrDefault();
        }

        var result = new List<Point>();
        while (pointToCheck is not null)
        {
            result.Add(pointToCheck.Point);
            pointToCheck = pointToCheck.Parent;
        }

        result.Reverse();
        
        return result.ToArray();
    }

    public static List<Point> FindShortestPath(byte[,] grid, params Point[] keyPoints)
    {
        if (keyPoints.Length < 2)
            throw new Exception("Can not resolve a path which contains less than 2 elements.");

        var points = new List<Point>();

        for (var i = 1; i < keyPoints.Length; i++)
        {
            points.AddRange(FindShortestPath(grid, keyPoints[i - 1], keyPoints[i]));

            if (points.Count is 0 || points.Last().X != keyPoints[i].X || points.Last().Y != keyPoints[i].Y)
                break;
        }

        return points;
    }
}
