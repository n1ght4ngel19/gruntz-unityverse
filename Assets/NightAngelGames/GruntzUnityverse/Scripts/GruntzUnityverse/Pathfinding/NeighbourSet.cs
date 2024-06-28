using System.Collections.Generic;
using GruntzUnityverse.Animation;
using GruntzUnityverse.Utils.Extensionz;

namespace GruntzUnityverse.Pathfinding {
[System.Serializable]
public struct NeighbourSet {
	public Node up;
	public Node upRight;
	public Node right;
	public Node downRight;
	public Node down;
	public Node downLeft;
	public Node left;
	public Node upLeft;

	public List<Node> AsList() {
		List<Node> list = new List<Node>();

		list.CheckNullAdd(up);
		list.CheckNullAdd(upRight);
		list.CheckNullAdd(right);
		list.CheckNullAdd(downRight);
		list.CheckNullAdd(down);
		list.CheckNullAdd(downLeft);
		list.CheckNullAdd(left);
		list.CheckNullAdd(upLeft);

		return list;
	}

	public Node NeighbourInDirection(Direction direction) {
		return direction switch {
			Direction.Up => up,
			Direction.UpRight => upRight,
			Direction.Right => right,
			Direction.DownRight => downRight,
			Direction.Down => down,
			Direction.DownLeft => downLeft,
			Direction.Left => left,
			Direction.UpLeft => upLeft,
			_ => null,
		};
	}
}
}
