using System.Linq;
using GruntzUnityverse.Actorz.BehaviourManagement;

namespace GruntzUnityverse.AI {
public class Defender : BaseAI {
	public override void DecideAttack() {
		if (target != null) {
			return;
		}

		if (post.spottedEnemiez.Count == 0) {
			ReturnToPost();

			return;
		}

		target = post.spottedEnemiez.First(); // Todo: Make this smarter (e.g. closest, weakest, etc.)
		self.attackTarget = target;
		self.HandleActionCommand(target.node, Intent.ToAttack);
	}
}
}
