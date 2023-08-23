using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Utility {
  public static class Vector2Direction {
    public static Vector2Int min = new Vector2Int(int.MinValue, int.MinValue);
    public static Vector2Int max = new Vector2Int(int.MaxValue, int.MaxValue);
    public static Vector2Int north = new Vector2Int(0, 1);
    public static Vector2Int east = new Vector2Int(1, 0);
    public static Vector2Int south = new Vector2Int(0, -1);
    public static Vector2Int west = new Vector2Int(-1, 0);
    public static Vector2Int northeast = new Vector2Int(1, 1);
    public static Vector2Int northwest = new Vector2Int(-1, 1);
    public static Vector2Int southeast = new Vector2Int(1, -1);
    public static Vector2Int southwest = new Vector2Int(-1, -1);

    /// <summary>
    /// Converts a Direction to its corresponding Vector2Int.
    /// </summary>
    /// <param name="dir">The Direction to check.</param>
    /// <returns>The Vector2Int corresponding to the given Direction.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Returns with error when
    /// there was no Direction specified or an invalid Direction was specified.</exception>
    public static Vector2Int FromDirection(Direction dir) {
      return dir switch {
        Direction.North => north,
        Direction.East => east,
        Direction.South => south,
        Direction.West => west,
        Direction.Northeast => northeast,
        Direction.Northwest => northwest,
        Direction.Southeast => southeast,
        Direction.Southwest => southwest,
        Direction.None => throw new System.ArgumentOutOfRangeException(nameof(dir), dir, "No direction specified!"),
        _ => throw new System.ArgumentOutOfRangeException(nameof(dir), dir, "No direction specified!"),
      };
    }
  }
}
