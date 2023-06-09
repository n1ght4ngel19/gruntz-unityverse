using System.Collections;
using GruntzUnityverse.Managerz;

namespace GruntzUnityverse.Objectz.Brickz {
  public abstract class Brick : MapObject {
    protected override void Start() {
      base.Start();

      LevelManager.Instance.SetBlockedAt(Location, true);
    }

    public abstract IEnumerator Explode();
  }
}
