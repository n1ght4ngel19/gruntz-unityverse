using GruntzUnityverse.Actorz;
using GruntzUnityverse.Objectz.Interactablez;
using UnityEngine;

namespace GruntzUnityverse.AI {
public class BrickLayerAI : AI {
	protected override void Update() {
		foreach (Grunt grunt in gruntzInRange) {
			if (!InOriginalRange(grunt.node)) {
				continue;
			}

			self.interactionTarget = null;
			self.attackTarget = grunt;

			self.GoToState(StateHandler.State.Attacking);

			enabled = false;

			return;
		}

		foreach (BrickBlock bb in FindObjectsByType<BrickBlock>(FindObjectsSortMode.None)) {
			if (!bb.enabled || !InOriginalRange(bb.node) || !self.tool.CompatibleWith(bb)) {
				continue;
			}

			self.attackTarget = null;
			self.interactionTarget = bb;

			self.GoToState(StateHandler.State.Interacting);

			enabled = false;

			return;
		}

		self.interactionTarget = null;
		self.attackTarget = null;

		self.travelGoal = originalNode;
		self.GoToState(StateHandler.State.Walking);
	}
}
}
