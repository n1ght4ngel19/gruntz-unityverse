using GruntzUnityverse.Actorz;
using GruntzUnityverse.Actorz.BehaviourManagement;
using GruntzUnityverse.Animation;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Base {
public class EquippedToy : ScriptableObject {
	public string toyName;
	public string description;

	[Range(0, 100)]
	public int duration;

	[Range(0, 5)]
	public float toyMoveSpeed;

	public AnimationPack animationPack;

	public virtual void ForcePlay(Grunt targetGrunt) {
		targetGrunt.intent = Intent.ToIdle;

		targetGrunt.EvaluateState(whenFalse: targetGrunt.BetweenNodes);

		targetGrunt.equippedToy = null;
	}
}
}
