using GruntzUnityverse.MapObjectz.BaseClasses;

namespace GruntzUnityverse.MapObjectz.Brickz {
  public class BrickFoundation : MapObject {
    public override void Setup() {
      base.Setup();
      
      GameManager.Instance.currentLevelManager.BrickFoundationz.Add(this);
    }
  }
}
