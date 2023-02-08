using GruntzUnityverse.Managerz;

namespace GruntzUnityverse.Objectz.Pyramidz {
  public class OrangePyramid : Pyramid {
    private void Update() {
      if (!IsInitialized && LevelManager.Instance.OrangePyramidz.Contains(this)) {
        InitializeNodeAtOwnLocation();
      }
    }
  }
}
