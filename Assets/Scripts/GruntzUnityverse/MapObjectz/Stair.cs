using GruntzUnityverse.Managerz;

namespace GruntzUnityverse.MapObjectz {
  public class Stair : MapObject {
    public bool isBlocked;
    // ------------------------------------------------------------ //

    private void Update() {
      GameManager.Instance.currentLevelManager.SetBlockedAt(location, isBlocked);
      enabled = false;
    }
  }
}
