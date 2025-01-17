using System.Drawing;

namespace AStarResolver.Models.PathFinding;

public sealed record PointInformations(Point Point, int DistanceToEnd)
{
    private const int DefaultReachingCost = 1;

    public PointInformations? Parent { get; private set; }

    public int ReachingCost { get; set; } = DefaultReachingCost;

    public long TotalCost =>
        DistanceToEnd + ReachingCost;

    public void SetParent(PointInformations parent)
    {
        Parent = parent;
        ReachingCost = parent.ReachingCost + DefaultReachingCost;
    }
}