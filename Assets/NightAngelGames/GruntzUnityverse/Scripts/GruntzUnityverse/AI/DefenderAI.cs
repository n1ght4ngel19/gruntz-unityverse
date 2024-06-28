using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;

namespace GruntzUnityverse.AI {
public class DefenderAI : AI {
	protected override void FixedUpdate() {
		// When straying too far from the defended location, go back to it 
		if (self.next && !InOriginalRange(self.next)) {
			self.interactionTarget = null;
			self.attackTarget = null;

			self.travelGoal = originalNode;
			self.GoToState(StateHandler.State.Walking);

			return;
		}

		// When the attack target is out of range, stop attacking
		if (self.attackTarget != null && !InOriginalRange(self.attackTarget.node)) {
			self.interactionTarget = null;
			self.attackTarget = null;

			self.travelGoal = originalNode;
			self.GoToState(StateHandler.State.Walking);

			return;
		}

		// When attack target exists after the above checks, stop looking for a new target
		if (self.attackTarget != null) {
			return;
		}

		foreach (Grunt grunt in GameManager.instance.playerGruntz) {
			if (InOriginalRange(grunt.node)) {
				self.interactionTarget = null;
				self.attackTarget = grunt;

				self.GoToState(StateHandler.State.Attacking);

				return;
			}
		}

		self.interactionTarget = null;
		self.attackTarget = null;

		if (self.node != originalNode) {
			self.travelGoal = originalNode;
			self.GoToState(StateHandler.State.Walking);
		} else {
			self.GoToState(StateHandler.State.Idle);
		}
	}
}
}
