using System.Drawing;
using AStarResolver.Services.Grid;
using System.Text;
using AStarResolver.Enums;
using AStarResolver.Services.Coordinates;
using AStarResolver.Services.PathFinding;

var grid = GridGeneratorService.GenerateGrid(15, 30, 8, 15, 15, 50, 3, 10);

string GetSymbol(Element element) =>
    element switch
    {
        Element.Wall => "X",
        Element.FreeCell => " ",
        Element.MarkedFreeCell => "0",
        Element.Path => ".",
        _ => throw new NotImplementedException(),
    };

void PrintGrid(byte[,] grid)
{
    var sb = new StringBuilder();
    var rows = grid.GetLength(0);
    var cols = grid.GetLength(1);

    for (int x = 0; x < rows; x++)
    {
        sb.Append('|');
    
        for (int y = 0; y < cols; y++)
            sb.Append(GetSymbol((Element)grid[x, y]));

        sb.AppendLine("|");
    }
    Console.WriteLine(sb.ToString());
    Console.WriteLine("--------------");
}



var points = new List<Point>();
for (var i = 0; i < 2; i++)
{
    if(!CoordinatesService.TryGetRandomFreeCell(grid, out var objectivePoint, points.ToArray()))
        throw new Exception("Invalid grid generation.");
    
    grid[objectivePoint.Value.X, objectivePoint.Value.Y] = (byte)Element.MarkedFreeCell;
    points.Add(objectivePoint.Value);
}

PrintGrid(grid);

var cells = PathFindingService.FindShortestPath(grid, points.ToArray());
foreach (var p in cells)
    grid[p.X, p.Y] = (byte)Element.Path;
    
PrintGrid(grid);