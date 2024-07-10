namespace GruntzUnityverse.Objectz {
public class Blocker : GridObject {
	protected override void AssignNodeValues() {
		node.isBlocked = true;
	}

	protected override void Awake() {
		base.Awake();

		spriteRenderer.enabled = false;
	}
}
}
