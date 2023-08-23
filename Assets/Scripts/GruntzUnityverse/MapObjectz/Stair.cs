using GruntzUnityverse.Managerz;

namespace GruntzUnityverse.MapObjectz {
  public class Stair : MapObject {
    public bool isBlocked;
    // ------------------------------------------------------------ //

    private void Update() {
      LevelManager.Instance.SetBlockedAt(location, isBlocked);
      enabled = false;
    }
  }
}
