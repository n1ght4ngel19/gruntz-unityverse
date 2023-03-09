using UnityEngine;

namespace GruntzUnityverse.Utility
{
  public struct Vector2IntCustom
  {
    public static readonly Vector2Int North = new Vector2Int(0, 1);
    public static Vector2Int NorthEast = new Vector2Int(1, 1);
    public static Vector2Int East = new Vector2Int(1, 0);
    public static Vector2Int SouthEast = new Vector2Int(1, -1);
    public static Vector2Int South = new Vector2Int(0, -1);
    public static Vector2Int SouthWest = new Vector2Int(-1, -1);
    public static Vector2Int West = new Vector2Int(-1, 0);
    public static Vector2Int NorthWest = new Vector2Int(-1, 1);
    public static Vector2Int Max = new Vector2Int(int.MaxValue, int.MaxValue);
  }
}
