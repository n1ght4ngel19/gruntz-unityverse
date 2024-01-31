using System.Collections;
using GruntzUnityverse.V2.Grunt;

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
    public float useTime;

    protected override void Pickup(GruntV2 grunt) {
      grunt.tool = grunt.gameObject.AddComponent(GetType()) as Tool;
      Destroy(gameObject);
    }

    /// <summary>
    /// Called when the <see cref="GruntV2"/> uses this Tool.
    /// This can be an attack or an interaction towards something on the map.
    /// <para/>
    /// Provides no implementation since each child class has a different effect.
    /// </summary>
    /// <returns>An IEnumerator since this is a coroutine.</returns>
    public abstract IEnumerator Use();
  }
}
