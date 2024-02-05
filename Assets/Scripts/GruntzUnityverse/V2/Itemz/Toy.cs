using System.Collections;
using GruntzUnityverse.V2.Core;
using GruntzUnityverse.V2.Grunt;

namespace GruntzUnityverse.V2.Itemz {
  public abstract class Toy : ItemV2 {
    /// <summary>
    /// The duration of this Toy's effect.
    /// </summary>
    public float duration;

    protected override IEnumerator Pickup(GruntV2 target) {
      yield return base.Pickup(target);

      target.toy = target.gameObject.AddComponent(GetType()) as Toy;
      GM.Instance.levelStatz.toyz++;
    }

    /// <summary>
    /// Called when the <see cref="GruntV2"/> uses this Toy.
    /// The target can be either another Grunt himself.
    /// <para/>
    /// Provides no implementation since each child class has a different effect.
    /// </summary>
    /// <returns>An IEnumerator since this is a coroutine.</returns>
    public abstract IEnumerator Use();
  }
}
