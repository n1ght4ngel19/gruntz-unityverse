using UnityEngine;

namespace GruntzUnityverse.V2 {
  public static class Vector3X {
    public static Vector3 RoundedToInt(this Vector3 vector3) {
      return new Vector3(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y), Mathf.RoundToInt(vector3.z));
    }

    public static Vector3 RoundedToIntWithCustomZ(this Vector3 vector3, float z) {
      return new Vector3(Mathf.RoundToInt(vector3.x), Mathf.RoundToInt(vector3.y), z);
    }

    public static Vector3 FromCamera(this Vector3 vector3) {
      return Camera.main.ScreenToWorldPoint(vector3);
    }
  }
}
