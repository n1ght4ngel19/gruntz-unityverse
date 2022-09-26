using UnityEngine;

public static class CustomStuff {
  public static Vector3 SetNavTilePosition(Vector3 vector) {
    return new Vector3(
      Mathf.Floor(vector.x) + 0.5f,
      Mathf.Floor(vector.y) + 0.5f,
      vector.z
    );
  }

  public static Vector3 SetSelectorCirclePosition(Vector3 vector) {
    return new Vector3(
      Mathf.Floor(vector.x) + 0.5f,
      Mathf.Floor(vector.y) + 0.5f,
      -10f
    );
  }
}
