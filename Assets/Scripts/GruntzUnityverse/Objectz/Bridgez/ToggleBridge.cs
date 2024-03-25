﻿using System.Collections;
using Animancer;
using GruntzUnityverse.Core;
using GruntzUnityverse.Itemz.Toolz;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Bridgez {
public class ToggleBridge : GridObject {
	public bool isDeathBridge;
	public bool raised;

	public float delay;
	public float duration;

	public AnimationClip raiseAnim;
	public AnimationClip lowerAnim;
	public AnimancerComponent animancer;

	public override void Setup() {
		base.Setup();

		animancer ??= GetComponent<AnimancerComponent>();
		node.isWater = isWater;
	}

	private IEnumerator Start() {
		yield return new WaitForSeconds(delay);

		StartCoroutine(Toggle());
	}

	public IEnumerator Toggle() {
		AnimationClip toPlay = raised ? lowerAnim : raiseAnim;

		animancer.Play(toPlay);

		yield return new WaitForSeconds(toPlay.length);

		raised = !raised;

		switch (node.tileType) {
			case TileType.Water:
				node.isWater = !raised;

				break;
			case TileType.Fire:
				node.isFire = !raised;

				break;
		}

		// Check if a Grunt is standing no the Bridge when it's lowered
		if (node.gruntOnNode != null) {
			// Kill the Grunt if it doesn't have a Toob or Wingz equipped
			if (node.isWater && node.gruntOnNode.equippedTool is not Wingz or Toob) {
				node.gruntOnNode.Die(AnimationManager.instance.sinkDeathAnimation, false, false);
			}

			if (node.isFire && node.gruntOnNode.equippedTool is not Wingz) {
				node.gruntOnNode.Die(AnimationManager.instance.burnDeathAnimation, false, false);
			}
		}

		yield return new WaitForSeconds(duration);

		StartCoroutine(Toggle());
	}
}
}
