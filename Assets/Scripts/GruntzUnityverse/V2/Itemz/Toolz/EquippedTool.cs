using GruntzUnityverse.V2.Actorz;
using GruntzUnityverse.V2.Animation;
using GruntzUnityverse.V2.Objectz;
using UnityEngine;

namespace GruntzUnityverse.V2.Itemz.Toolz {
public abstract class EquippedTool : ScriptableObject {
	public string toolName;
	public string description;

	[Range(0, 20)]
	public int damage;

	[Range(0, 10)]
	public int range;

	[Range(0, 5)]
	public float moveSpeed;

	public AnimationPack animationPack;

	/// <summary>
	/// Implemented when "extra" effects are needed.
	/// </summary>
	/// <param name="targetObject"></param>
	protected virtual void InteractEffect(GridObject targetObject) { }

	/// <summary>
	/// Implemented when "extra" effects are needed, e.g. when shooting a Nerfgun,
	/// the bullet needs to be instantiated and sent to travel towards the target.
	/// </summary>
	/// <param name="targetGrunt"></param>
	protected virtual void AttackEffect(Grunt targetGrunt) { }
}
}
