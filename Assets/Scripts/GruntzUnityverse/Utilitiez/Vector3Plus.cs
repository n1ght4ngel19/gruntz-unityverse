using UnityEngine;

namespace GruntzUnityverse.Utilitiez {
  /// <summary>
  /// Extension of Vector3 providing common values for diagonal directions.
  /// </summary>
  public struct Vector3Plus {
    private static readonly Vector3 uprightVector = new(1f, 1f, 0.0f);
    private static readonly Vector3 upleftVector = new(-1f, 1f, 0.0f);
    private static readonly Vector3 downrightVector = new(1f, -1f, 0.0f);
    private static readonly Vector3 downleftVector = new(-1f, -1f, 0.0f);

    /// <summary>
    /// Shorthand for writing Vector3(1, 1, 0).
    /// </summary>
    public static Vector3 upright {get => uprightVector;}

    /// <summary>
    /// Shorthand for writing Vector3(-1, 1, 0).
    /// </summary>
    public static Vector3 upleft {get => upleftVector;}

    /// <summary>
    /// Shorthand for writing Vector3(1, -1, 0).
    /// </summary>
    public static Vector3 downright {get => downrightVector;}

    /// <summary>
    /// Shorthand for writing Vector3(-1, -1, 0).
    /// </summary>
    public static Vector3 downleft {get => downleftVector;}
  }
}

