using GruntzUnityverse.V2.Actorz;
using GruntzUnityverse.V2.Actorz.BehaviourManagement;
using GruntzUnityverse.V2.Animation;
using UnityEngine;

namespace GruntzUnityverse.V2.Itemz.Toyz {
[CreateAssetMenu(fileName = "New Equipped Toy", menuName = "Gruntz Unityverse/Equipped Toy")]
public class EquippedToy : ScriptableObject {
	public string toyName;
	public string description;

	[Range(0, 100)]
	public int duration;

	public AnimationPack animationPack;

	public virtual void ForcePlay(Grunt targetGrunt) {
		targetGrunt.intent = Intent.ToIdle;

		targetGrunt.EvaluateState(whenFalse: targetGrunt.betweenNodes);

		targetGrunt.equippedToy = null;
	}
}
}
