using System.Collections;
using GruntzUnityverse.V2.Core;
using GruntzUnityverse.V2.Grunt;

namespace GruntzUnityverse.V2.Itemz {
  /// <summary>
  /// The base class for all Powerupz.
  /// </summary>
  public abstract class Powerup : ItemV2 {
    /// <summary>
    /// The duration of the Powerup. A duration of 0 means the Powerup has an instantaneous effect.
    /// </summary>
    public float duration;

    protected override IEnumerator Pickup(GruntV2 target) {
      yield return base.Pickup(target);

      target.powerup = target.gameObject.AddComponent(GetType()) as Powerup;
      GM.Instance.levelStatz.powerupz++;
    }

    // Todo: If the Grunt already has a Powerup, we don't want to pick up another one. (Or do we?)
  }
}
