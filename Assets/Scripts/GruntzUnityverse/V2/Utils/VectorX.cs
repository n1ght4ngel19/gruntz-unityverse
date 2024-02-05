using UnityEngine;

namespace GruntzUnityverse.V2.Utils {
  public static class VectorX {
    // --------------------------------------------------
    // Vector2
    // --------------------------------------------------
    public static Vector2 RoundedToInt(this Vector2 vector2) {
      return new Vector2(Mathf.RoundToInt(vector2.x), Mathf.RoundToInt(vector2.y));
    }
    
    // --------------------------------------------------
    // Vector2Int
    // --------------------------------------------------
    public static Vector3 ToVector3(this Vector2Int vector2Int) {
      return new Vector3(vector2Int.x, vector2Int.y, 0);
    }

    // --------------------------------------------------
    // Directions
    // --------------------------------------------------
    // Floats
    public static Vector2 Up(this Vector2 vector)
      => vector + Vector2.up;

    public static Vector3 Up(this Vector3 vector)
      => vector + Vector3.up;

    public static Vector2 UpRight(this Vector2 vector)
      => vector + Vector2.up + Vector2.right;

    public static Vector3 UpRight(this Vector3 vector)
      => vector + Vector3.up + Vector3.right;

    public static Vector2 Right(this Vector2 vector)
      => vector + Vector2.right;

    public static Vector3 Right(this Vector3 vector)
      => vector + Vector3.right;

    public static Vector2 DownRight(this Vector2 vector)
      => vector + Vector2.down + Vector2.right;

    public static Vector3 DownRight(this Vector3 vector)
      => vector + Vector3.down + Vector3.right;

    public static Vector2 Down(this Vector2 vector)
      => vector + Vector2.down;

    public static Vector3 Down(this Vector3 vector)
      => vector + Vector3.down;

    public static Vector2 DownLeft(this Vector2 vector)
      => vector + Vector2.down + Vector2.left;

    public static Vector3 DownLeft(this Vector3 vector)
      => vector + Vector3.down + Vector3.left;

    public static Vector2 Left(this Vector2 vector)
      => vector + Vector2.left;

    public static Vector3 Left(this Vector3 vector)
      => vector + Vector3.left;

    // Ints
    public static Vector2Int Up(this Vector2Int vector)
      => vector + Vector2Int.up;

    public static Vector3Int Up(this Vector3Int vector)
      => vector + Vector3Int.up;

    public static Vector2Int UpRight(this Vector2Int vector)
      => vector + Vector2Int.up + Vector2Int.right;

    public static Vector3Int UpRight(this Vector3Int vector)
      => vector + Vector3Int.up + Vector3Int.right;

    public static Vector2Int Right(this Vector2Int vector)
      => vector + Vector2Int.right;

    public static Vector3Int Right(this Vector3Int vector)
      => vector + Vector3Int.right;

    public static Vector2Int DownRight(this Vector2Int vector)
      => vector + Vector2Int.down + Vector2Int.right;

    public static Vector3Int DownRight(this Vector3Int vector)
      => vector + Vector3Int.down + Vector3Int.right;

    public static Vector2Int Down(this Vector2Int vector)
      => vector + Vector2Int.down;

    public static Vector3Int Down(this Vector3Int vector)
      => vector + Vector3Int.down;

    public static Vector2Int DownLeft(this Vector2Int vector)
      => vector + Vector2Int.down + Vector2Int.left;

    public static Vector3Int DownLeft(this Vector3Int vector)
      => vector + Vector3Int.down + Vector3Int.left;

    public static Vector2Int Left(this Vector2Int vector)
      => vector + Vector2Int.left;

    public static Vector3Int Left(this Vector3Int vector)
      => vector + Vector3Int.left;

    public static Vector2Int UpLeft(this Vector2Int vector)
      => vector + Vector2Int.up + Vector2Int.left;

    public static Vector3Int UpLeft(this Vector3Int vector)
      => vector + Vector3Int.up + Vector3Int.left;

    // --------------------------------------------------
    // Vector3
    // --------------------------------------------------
    public static Vector3 RoundedToInt(this Vector3 vector3) {
      return new Vector3(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y), Mathf.RoundToInt(vector3.z));
    }

    public static Vector3 RoundedToInt(this Vector3 vector3, float z) {
      return new Vector3(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y), z);
    }
  }
}
