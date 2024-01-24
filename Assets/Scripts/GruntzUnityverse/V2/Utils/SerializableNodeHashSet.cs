using System.Collections.Generic;
using UnityEngine;

namespace GruntzUnityverse.V2.Utils {
  public class SerializableNodeHashSet : HashSet<NodeV2>, ISerializationCallbackReceiver {
    public HashSet<NodeV2> nodeList = new HashSet<NodeV2>();

    [SerializeField]
    private List<NodeV2> _nodeListSerialized = new List<NodeV2>();

    public void OnBeforeSerialize() {
      _nodeListSerialized.Clear();

      foreach (NodeV2 node in nodeList) {
        _nodeListSerialized.Add(node);
      }
    }

    public void OnAfterDeserialize() {
      nodeList.Clear();

      foreach (NodeV2 node in _nodeListSerialized) {
        nodeList.Add(node);
      }
    }
  }
}
