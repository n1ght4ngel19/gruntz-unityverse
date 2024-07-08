using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;

namespace GruntzUnityverse.AI {
public class SmartChaserAI : AI {
	protected override void FixedUpdate() {
		foreach (Grunt grunt in GameManager.instance.playerGruntz) {
			if (grunt.tool.damage > self.tool.damage) {
				continue;
			}

			if (InRange(grunt.node)) {
				self.interactionTarget = null;
				self.attackTarget = grunt;

				self.GoToState(StateHandler.State.Attacking);

				enabled = false;

				return;
			}
		}

		self.interactionTarget = null;
		self.attackTarget = null;

		self.GoToState(StateHandler.State.Idle);
	}
}
}
