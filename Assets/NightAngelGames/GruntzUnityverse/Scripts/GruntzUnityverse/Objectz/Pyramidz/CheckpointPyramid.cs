namespace GruntzUnityverse.Objectz.Pyramidz {
public class CheckpointPyramid : Pyramid {
	public override void Deactivate() {
		base.Deactivate();

		Toggle();

		enabled = false;
	}
}
}
