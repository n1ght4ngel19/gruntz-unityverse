using GruntzUnityverse.Actorz;
using GruntzUnityverse.Animation;
using GruntzUnityverse.Core;
using GruntzUnityverse.Objectz;
using NaughtyAttributes;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Base {
[CreateAssetMenu(fileName = "New Tool", menuName = "Gruntz Unityverse/Toolz/Equipped Tool")]
public class EquippedTool : ScriptableObject {
	[EnumFlags]
	public ToolCode toolCode;

	public string toolName;

	public string description;

	[Range(0, 20)]
	public int damage;

	[Range(0, 10)]
	public int range;

	[Range(0, 5)]
	public float toolMoveSpeed;

	[Expandable]
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

public enum ToolCode {
	BareHandz,
	Bomb,
	Boomerang,
	BoxingGlovez,
	BrickLayer,
	Club,
	Gauntletz,
	GooberStraw,
	GravityBootz,
	GunHat,
	NerfGun,
	Rockz,
	Shield,
	Shovel,
	Spring,
	SpyGear,
	Sword,
	TimeBombz,
	Toob,
	Warpstone,
	WelderKit,
	Wingz,
}
}
