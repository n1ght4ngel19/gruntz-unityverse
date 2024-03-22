using System.Linq;

namespace GruntzUnityverse.AI {
public class Defender : BaseAI {
	public override void DecideAttack() {
		if (post.spottedEnemiez.Count == 0) {
			ReturnToPost();

			return;
		}

		target = post.spottedEnemiez.First(); // Todo: Make this smarter (e.g. closest, weakest, etc.)
		self.attackTarget = target;
		self.TryTakeAction(target.node, "Attack");
	}
}
}
