using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;

namespace GruntzUnityverse.AI {
public class DefenderAI : AI {
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

		self.interactionTarget = null;
		self.attackTarget = null;

		self.travelGoal = originalNode;
		self.GoToState(StateHandler.State.Walking);
	}
}
}
