using UnityEngine;

namespace GruntzUnityverse.V2 {
  public static class NumberX {
    public static int AsInt(this float value) {
      return Mathf.RoundToInt(value);
    }
  }
}
