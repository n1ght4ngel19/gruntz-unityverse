using GruntzUnityverse.Actorz;
using GruntzUnityverse.Animation;
using GruntzUnityverse.Core;
using GruntzUnityverse.Objectz;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Base {
public abstract class EquippedTool : ScriptableObject {
	public string toolName;
	public string description;

	[Range(0, 20)]
	public int damage;

	[Range(0, 10)]
	public int range;

	[Range(0, 5)]
	public float toolMoveSpeed;

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

	public virtual bool CompatibleWith(GridObject target) => false;

	public virtual AnimationClip cursor => AnimationManager.instance.cursorDefault;
}
}
