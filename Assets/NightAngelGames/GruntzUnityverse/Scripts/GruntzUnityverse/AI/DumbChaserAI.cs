using GruntzUnityverse.Actorz;

namespace GruntzUnityverse.AI {
public class DumbChaserAI : AI {
	protected override void Update() {
		foreach (Grunt grunt in gruntzInRange) {
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
