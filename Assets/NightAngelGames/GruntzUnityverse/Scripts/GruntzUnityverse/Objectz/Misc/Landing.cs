namespace GruntzUnityverse.Objectz.Misc {
public class Landing : GridObject {
	public override void Setup() {
		base.Setup();

		node.isWater = false;
		node.isFire = false;
	}
}
}
