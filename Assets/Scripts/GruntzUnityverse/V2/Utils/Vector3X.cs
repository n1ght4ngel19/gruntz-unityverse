using UnityEngine;

namespace GruntzUnityverse.V2.Utils {
  public static class Vector3X {
    public static Vector3 FromCameraView(this Vector3 vector3, Camera camera) {
      return camera.ScreenToWorldPoint(vector3);
    }
  }
}
