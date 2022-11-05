using UnityEngine;

namespace GruntzUnityverse.Utilitiez {
  /// <summary>
  /// Extension of Vector2.
  /// </summary>
  public struct Vector2Plus {
    private static readonly Vector2 uprightVector = new(1f, 1f);
    private static readonly Vector2 upleftVector = new(-1f, 1f);
    private static readonly Vector2 downrightVector = new(1f, -1f);
    private static readonly Vector2 downleftVector = new(-1f, -1f);
    private static readonly Vector2 halfVector = new(0.5f, 0.5f);

    /// <summary>
    /// Shorthand for writing Vector(1, 1).
    /// </summary>
    public static Vector2 upright {get => uprightVector;}

    /// <summary>
    /// Shorthand for writing Vector(-1, 1).
    /// </summary>
    public static Vector2 upleft {get => upleftVector;}

    /// <summary>
    /// Shorthand for writing Vector(1, -1).
    /// </summary>
    public static Vector2 downright {get => downrightVector;}

    /// <summary>
    /// Shorthand for writing Vector(-1, -1).
    /// </summary>
    public static Vector2 downleft {get => downleftVector;}
    
    /// <summary>
    /// Shorthand for writing Vector(0.5, 0.5).
    /// </summary>
    public static Vector2 half {get => halfVector;}

    public static Vector2 RoundToDecimalPlaces(int decimalPlaces, Vector2 vector) {
      float multiplier = 1;

      for (int i = 0; i < decimalPlaces; i++) {
        multiplier *= 10f;
      }

      return new Vector2(
        Mathf.Round(vector.x * multiplier) / multiplier,
        Mathf.Round(vector.y * multiplier) / multiplier
      );
    }

    public static Vector2 RoundToDecimalPlaces(int decimalPlaces, Vector3 vector) {
      float multiplier = 1;

      for (int i = 0; i < decimalPlaces; i++) {
        multiplier *= 10f;
      }

      return new Vector2(
        Mathf.Round(vector.x * multiplier) / multiplier,
        Mathf.Round(vector.y * multiplier) / multiplier
      );
    }
    
    public static bool AreEqual(Vector2 vector1, Vector2 vector2) {
      return RoundToDecimalPlaces(2, vector1) == RoundToDecimalPlaces(2, vector2);
    }

    public static bool AreEqual(Vector3 vector1, Vector3 vector2) {
      return RoundToDecimalPlaces(2, vector1) == RoundToDecimalPlaces(2, vector2);
    }
  }
}

