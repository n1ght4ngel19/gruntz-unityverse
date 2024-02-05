using System.Collections.Generic;
using GruntzUnityverse.V2.Utils;

namespace GruntzUnityverse.V2.Pathfinding {
  [System.Serializable]
  public struct NeighbourSet {
    public NodeV2 up;
    public NodeV2 upRight;
    public NodeV2 right;
    public NodeV2 downRight;
    public NodeV2 down;
    public NodeV2 downLeft;
    public NodeV2 left;
    public NodeV2 upLeft;

    public List<NodeV2> AsList() {
      List<NodeV2> list = new List<NodeV2>();

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
