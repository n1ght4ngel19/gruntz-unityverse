using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz {
  public class EyeCandy : MonoBehaviour {
    public EyeCandyType eyeCandyType;
    // ------------------------------------------------------------ //

    private void Start() {
      if (eyeCandyType == EyeCandyType.CollidingEyeCandy) {
        gameObject.AddComponent<AboveBelow>();
      }
    }
  }
}
