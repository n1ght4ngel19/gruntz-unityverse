using System.Collections.Generic;
using GruntzUnityverse.V2.Utils;

namespace GruntzUnityverse.V2.Pathfinding {
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
  }
}
