using System.Collections;
using GruntzUnityverse.V2.Grunt;

namespace GruntzUnityverse.V2.Itemz {
  public abstract class Toy : ItemV2 {
    /// <summary>
    /// The duration of this Toy's effect.
    /// </summary>
    public float duration;

    /// <summary>
    /// Called when the <see cref="GruntV2"/> is forced to use this Toy (or uses it himself, but that is rare).
    /// </summary>
    /// <returns>An IEnumerator since this is a coroutine.</returns>
    public abstract IEnumerator PlayWith();
  }
}
