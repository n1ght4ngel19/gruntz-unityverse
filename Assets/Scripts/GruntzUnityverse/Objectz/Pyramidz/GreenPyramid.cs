using GruntzUnityverse.Managerz;

namespace GruntzUnityverse.Objectz.Pyramidz {
  public class GreenPyramid : Pyramid {
    private void Update() {
      if (!IsInitialized && LevelManager.Instance.GreenPyramidz.Contains(this)) {
        InitializeNodeAtOwnLocation();
      }
    }
  }
}
