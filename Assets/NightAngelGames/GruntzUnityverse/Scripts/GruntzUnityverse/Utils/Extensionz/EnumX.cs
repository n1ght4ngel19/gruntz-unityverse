using GruntzUnityverse.Animation;

namespace GruntzUnityverse.Utils.Extensionz {
public static class EnumX {
	public static Direction Opposite(this Direction dir) {
		return dir switch {
			Direction.Up => Direction.Down,
			Direction.UpRight => Direction.DownLeft,
			Direction.Right => Direction.Left,
			Direction.DownRight => Direction.UpLeft,
			Direction.Down => Direction.Up,
			Direction.DownLeft => Direction.UpRight,
			Direction.Left => Direction.Right,
			Direction.UpLeft => Direction.DownRight,
			_ => Direction.None,
		};
	}
}
}
