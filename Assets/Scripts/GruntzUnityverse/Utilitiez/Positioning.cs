using UnityEngine;

namespace GruntzUnityverse.Utilitiez {
  public static class Positioning {
    /// <summary>
    /// Gives a corrected position for a Node.
    /// </summary>
    /// <param name="vector">The vector that determines the Node's z-index.</param>
    /// <returns>The corrected position of a Node.</returns>
    public static Vector3 SetNavTilePosition(Vector3 vector) {
      return new Vector3(
        Mathf.Floor(vector.x) + 0.5f,
        Mathf.Floor(vector.y) + 0.5f,
        vector.z
      );
    }

    /// <summary>
    /// Gives a corrected position for the SelectorCircle.
    /// </summary>
    /// <param name="vector">The vector that determines the SelectorCircle's x and y coordinates.</param>
    /// <returns>The corrected position of the SelectorCircle.</returns>
    public static Vector3 SetSelectorCirclePosition(Vector3 vector) {
      return new Vector3(
        Mathf.Floor(vector.x) + 0.5f,
        Mathf.Floor(vector.y) + 0.5f,
        -10f
      );
    }
  }
}
