using UnityEngine;

namespace GruntzUnityverse.Utility {
  public static class Vector2IntExtra {
    public static Vector2Int North() { return new Vector2Int(0, 1); }

    public static Vector2Int NorthEast() { return new Vector2Int(1, 1); }

    public static Vector2Int East() { return new Vector2Int(1, 0); }

    public static Vector2Int SouthEast() { return new Vector2Int(1, -1); }

    public static Vector2Int South() { return new Vector2Int(0, -1); }

    public static Vector2Int SouthWest() { return new Vector2Int(-1, -1); }

    public static Vector2Int West() { return new Vector2Int(-1, 0); }

    public static Vector2Int NorthWest() { return new Vector2Int(-1, 1); }

    public static Vector2Int Max() { return new Vector2Int(int.MaxValue, int.MaxValue); }
    public static Vector2Int Min() { return new Vector2Int(int.MinValue, int.MinValue); }

    public static Vector2Int OwnNorth(this Vector2Int vector) { return vector + North(); }

    public static Vector2Int OwnNorthEast(this Vector2Int vector) { return vector + NorthEast(); }

    public static Vector2Int OwnEast(this Vector2Int vector) { return vector + East(); }

    public static Vector2Int OwnSouthEast(this Vector2Int vector) { return vector + SouthEast(); }

    public static Vector2Int OwnSouth(this Vector2Int vector) { return vector + South(); }

    public static Vector2Int OwnSouthWest(this Vector2Int vector) { return vector + SouthWest(); }

    public static Vector2Int OwnWest(this Vector2Int vector) { return vector + West(); }

    public static Vector2Int OwnNorthWest(this Vector2Int vector) { return vector + NorthWest(); }
  }
}
