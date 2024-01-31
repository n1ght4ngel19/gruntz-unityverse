using System.Collections;
using GruntzUnityverse.V2.Grunt;

namespace GruntzUnityverse.V2.Itemz {
  /// <summary>
  /// The base class for all Powerupz.
  /// </summary>
  public abstract class Powerup : ItemV2 {
    /// <summary>
    /// The duration of the Powerup.
    /// </summary>
    public float duration;

    /// <summary>
    /// The <see cref="GruntV2"/> affected by the Powerup.
    /// </summary>
    public GruntV2 affectedGrunt;

    protected override void Pickup(GruntV2 grunt) {
      // Todo: If the Grunt already has a Powerup, we don't want to pick up another one. (Or do we?)
      grunt.powerup = grunt.gameObject.AddComponent(GetType()) as Powerup;
    }

    /// <summary>
    /// Called when a <see cref="GruntV2"/> picks up this Powerup.
    /// </summary>
    /// <returns>An IEnumerator since this is a coroutine.</returns>
    public abstract IEnumerator Activate();
  }
}
