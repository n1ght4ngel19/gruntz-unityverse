using GruntzUnityverse.MapObjectz.BaseClasses;

namespace GruntzUnityverse.MapObjectz {
  public class Stair : MapObject {
    public bool isBlocked;

    public override void Setup() {
      base.Setup();

      ownNode.isBlocked = isBlocked;
    }
  }
}
