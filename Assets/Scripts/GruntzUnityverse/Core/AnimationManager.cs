using System.Collections.Generic;
using UnityEngine;

namespace GruntzUnityverse.Core {
public class AnimationManager : MonoBehaviour {
	public static AnimationManager Instance { get; private set; }

	private void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(this);
		} else {
			Instance = this;
		}
	}

	[Header("Death Animationz")]
	public AnimationClip burnDeathAnimation;

	public AnimationClip electrocuteDeathAnimation;
	public AnimationClip explodeDeathAnimation;
	public AnimationClip fallDeathAnimation;
	public AnimationClip freezeDeathAnimation;
	public AnimationClip holeDeathAnimation;
	public AnimationClip karaokeDeathAnimation;
	public AnimationClip meltDeathAnimation;
	public AnimationClip sinkDeathAnimation;
	public AnimationClip squashDeathAnimation;

	[Header("Grunt Warp Animationz")]
	public AnimationClip gruntWarpEnterAnimation;

	public List<AnimationClip> gruntWarpOutAnimationz;

	public AnimationClip gruntWarpOutEndAnimation;

	[Header("Warp Animationz")]
	public AnimationClip warpAppearAnim;

	public AnimationClip warpDisappearAnim;

	public AnimationClip warpSwirlingAnim;

	public AnimationClip timeBombTickingAnim;
	public AnimationClip explosionAnim1;
	public AnimationClip explosionAnim2;
	public AnimationClip explosionAnim3;
}
}
