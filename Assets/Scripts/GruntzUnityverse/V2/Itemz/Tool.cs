using System.Collections;
using GruntzUnityverse.V2.Grunt;
using UnityEngine;

namespace GruntzUnityverse.V2.Itemz {
  /// <summary>
  /// The base class for all Toolz.
  /// </summary>
  public abstract class Tool : ItemV2 {
    /// <summary>
    /// The base damage this Tool applies, without any modifiers.
    /// </summary>
    public int damage;

    /// <summary>
    /// The range of this Tool in tilez.
    /// </summary>
    public int range;

    /// <summary>
    /// The time needed in secondz for a <see cref="GruntV2"/> with this Tool to move one tile.
    /// </summary>
    public float moveSpeed;

    public float rechargeTime;

    /// <summary>
    /// The time needed in secondz before this Tool's effect is applied (e.g. before a rock is thrown).
    /// </summary>
    // Todo: Maybe not needed -> use animation length instead
    public float useTime;

    // Todo: Maybe not needed -> use animation length instead
    public float attackTime;

    protected override IEnumerator Pickup(GruntV2 target) {
      yield return base.Pickup(target);

      target.tool = target.gameObject.AddComponent(GetType()) as Tool;
      // Todo: Set target's animation pack to this Tool's animation pack
    }

    /// <summary>
    /// Called when the <see cref="GruntV2"/> uses this Tool.
    /// This can be an attack or an interaction towards something on the map.
    /// <para/>
    /// Provides no implementation since each child class has a different effect.
    /// </summary>
    /// <returns>An IEnumerator since this is a coroutine.</returns>
    public abstract IEnumerator Use(GruntV2 target);

    public abstract IEnumerator Use(GameObject target);
  }
}
