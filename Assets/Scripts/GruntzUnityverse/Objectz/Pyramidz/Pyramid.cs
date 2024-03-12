using Animancer;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Objectz.Interfacez;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Pyramidz {
public abstract class Pyramid : GridObject, IToggleable {
	public override void Setup() {
		base.Setup();

		Animator = GetComponent<Animator>();
		Animancer = GetComponent<AnimancerComponent>();
	}

	// --------------------------------------------------
	// IToggleable
	// --------------------------------------------------

	#region IToggleable
	[field: SerializeField] public AnimationClip ToggleOnAnim { get; set; }
	[field: SerializeField] public AnimationClip ToggleOffAnim { get; set; }
	public Animator Animator { get; set; }
	public AnimancerComponent Animancer { get; set; }

	public virtual async void Toggle() {
		AnimationClip toPlay = isObstacle ? ToggleOffAnim : ToggleOnAnim;

		Animancer.Play(toPlay);
		await UniTask.WaitForSeconds(toPlay.length);

		isObstacle = !isObstacle;
		node.isBlocked = isObstacle;
	}

	public virtual void ToggleOn() { }

	public virtual void ToggleOff() { }
	#endregion

}
}
