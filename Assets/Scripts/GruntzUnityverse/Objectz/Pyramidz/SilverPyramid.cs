using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Pyramidz {
  public class SilverPyramid : Pyramid {
    [field: SerializeField] public float Delay { get; set; }
    [field: SerializeField] public float Duration { get; set; }

    private void Update() {
      if (!IsInitialized && LevelManager.Instance.SilverPyramidz.Contains(this)) {
        InitializeNodeAtOwnLocation();
      }
    }
  }
}
