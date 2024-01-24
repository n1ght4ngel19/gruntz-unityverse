using System.Collections.Generic;

namespace GruntzUnityverse.V2 {
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

      if (up != null) {
        list.Add(up);
      }

      if (upRight != null) {
        list.Add(upRight);
      }

      if (right != null) {
        list.Add(right);
      }

      if (downRight != null) {
        list.Add(downRight);
      }

      if (down != null) {
        list.Add(down);
      }

      if (downLeft != null) {
        list.Add(downLeft);
      }

      if (left != null) {
        list.Add(left);
      }

      if (upLeft != null) {
        list.Add(upLeft);
      }

      return list;
    }
  }
}
