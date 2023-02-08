using GruntzUnityverse.Managerz;

namespace GruntzUnityverse.Objectz.Pyramidz {
  public class RedPyramid : Pyramid {
    private void Update() {
      if (!IsInitialized && LevelManager.Instance.RedPyramidz.Contains(this)) {
        InitializeNodeAtOwnLocation();
      }
    }
  }
}
