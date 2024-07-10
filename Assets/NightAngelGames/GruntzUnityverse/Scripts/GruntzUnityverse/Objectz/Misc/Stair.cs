namespace GruntzUnityverse.Objectz.Misc {
public class Stair : GridObject {
	protected override void AssignNodeValues() {
		node.isBlocked = isObstacle;
	}
}
}
