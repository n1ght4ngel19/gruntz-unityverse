using GruntzUnityverse.MapObjectz.BaseClasses;

namespace GruntzUnityverse.MapObjectz.Brickz {
  public class BrickColumn : MapObject {
    public override void Setup() {
      base.Setup();

      GameManager.Instance.currentLevelManager.BrickColumnz.Add(this);
      ownNode.isBlocked = true;
      ownNode.isHardTurn = true;
    }
  }
}
