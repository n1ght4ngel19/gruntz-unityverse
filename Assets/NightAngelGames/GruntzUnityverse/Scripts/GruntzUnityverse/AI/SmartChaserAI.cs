using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;

namespace GruntzUnityverse.AI {
public class SmartChaserAI : AI {
	protected override void Update() {
		foreach (Grunt grunt in gruntzInRange) {
			if (grunt.tool.damage > self.tool.damage) {
				continue;
			}

			self.interactionTarget = null;
			self.attackTarget = grunt;

			self.GoToState(StateHandler.State.Attacking);

			enabled = false;

			return;
		}

		self.interactionTarget = null;
		self.attackTarget = null;

		self.GoToState(StateHandler.State.Idle);
	}
}
}
