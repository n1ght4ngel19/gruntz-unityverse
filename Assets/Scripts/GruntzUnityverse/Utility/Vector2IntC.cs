using UnityEngine;

namespace GruntzUnityverse.Utility
{
  public struct Vector2IntC
  {
    public static readonly Vector2Int North = new(0, 1);
    public static Vector2Int NorthEast = new(1, 1);
    public static Vector2Int East = new(1, 0);
    public static Vector2Int SouthEast = new(1, -1);
    public static Vector2Int South = new(0, -1);
    public static Vector2Int SouthWest = new(-1, -1);
    public static Vector2Int West = new(-1, 0);
    public static Vector2Int NorthWest = new(-1, 1);
  }
}
