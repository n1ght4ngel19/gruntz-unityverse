using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using GruntzUnityverse.Objectz.Interactablez;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.AI {
public class BrickLayerAI : AI {
	protected override void FixedUpdate() {
		foreach (Grunt grunt in GameManager.instance.playerGruntz) {
			if (InOriginalRange(grunt.node)) {
				self.interactionTarget = null;
				self.attackTarget = grunt;

				self.GoToState(StateHandler.State.Attacking);

				enabled = false;

				return;
			}
		}

		foreach (BrickBlock bb in FindObjectsByType<BrickBlock>(FindObjectsSortMode.None)) {
			if (bb.enabled && InOriginalRange(bb.node) && self.equippedTool.CompatibleWith(bb)) {
				self.attackTarget = null;
				self.interactionTarget = bb;

				self.GoToState(StateHandler.State.Interacting);

				enabled = false;

				return;
			}
		}

		self.interactionTarget = null;
		self.attackTarget = null;

		self.travelGoal = originalNode;
		self.GoToState(StateHandler.State.Walking);
	}
}
}
