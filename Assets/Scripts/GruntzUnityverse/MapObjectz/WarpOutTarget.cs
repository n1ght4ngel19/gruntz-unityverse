using GruntzUnityverse.MapObjectz.BaseClasses;

namespace GruntzUnityverse.MapObjectz {
  public class WarpOutTarget : MapObject {
    private void Update() {
      spriteRenderer.enabled = false;
    }
  }
}
