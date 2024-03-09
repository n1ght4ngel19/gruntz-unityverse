namespace GruntzUnityverse.Objectz.Misc {
public class Stair : GridObject {
	public override void Setup() {
		base.Setup();

		node.isBlocked = actAsObstacle;
	}
}
}
