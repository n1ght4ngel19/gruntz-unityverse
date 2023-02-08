using GruntzUnityverse.Managerz;

namespace GruntzUnityverse.Objectz.Pyramidz {
  public class BlackPyramid : Pyramid {
    private void Update() {
      if (!IsInitialized && LevelManager.Instance.BlackPyramidz.Contains(this)) {
        InitializeNodeAtOwnLocation();
      }
    }
  }
}
