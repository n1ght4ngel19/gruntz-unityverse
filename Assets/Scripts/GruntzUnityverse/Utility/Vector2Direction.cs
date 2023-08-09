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
  }
}
