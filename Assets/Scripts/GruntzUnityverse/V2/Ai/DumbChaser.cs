using System.Linq;
using GruntzUnityverse.V2.Grunt;

namespace GruntzUnityverse.V2.Ai {
public class DumbChaser : BaseAi {
	private void Start() {
		Self = GetComponent<GruntV2>();
		post.managedGruntz.Add(this);
	}

	public override void ActOnTarget() {
		target = post.targetedGruntz.First();
		Self.attackTarget = target;

		Action();
	}

	public override void ReturnToPost() {
		target = null;
		Self.attackTarget = null;

		Self.targetNode = post.node;
		Self.Move();
	}
}
}
