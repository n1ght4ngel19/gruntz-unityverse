using UnityEngine;

namespace GruntzUnityverse.Utility {
  public static class ConditionalLogger {
    public static void Log(string message) {
      if (GameManager.Instance.isDebugMode) {
        Debug.Log(message);
      }
    }

    public static void LogWarning(string message) {
      if (GameManager.Instance.isDebugMode) {
        Debug.LogWarning(message);
      }
    }

    public static void LogError(string message) {
      if (GameManager.Instance.isDebugMode) {
        Debug.LogError(message);
      }
    }
  }
}
