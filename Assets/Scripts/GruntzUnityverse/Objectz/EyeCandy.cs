using GruntzUnityverse.Enumz;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class EyeCandy : MonoBehaviour {
    [field: SerializeField] public EyeCandyType Type { get; set; }

    private void Start() {
      if (Type == EyeCandyType.CollidingEyeCandy) {
        gameObject.AddComponent<AboveBelow>();
      }
    }
  }
}
