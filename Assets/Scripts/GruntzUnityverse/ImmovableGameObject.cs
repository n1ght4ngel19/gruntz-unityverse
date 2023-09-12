using UnityEngine;

namespace GruntzUnityverse {
  public class ImmovableGameObject : MonoBehaviour {
    private void OnValidate() {
      transform.hideFlags = HideFlags.NotEditable;
    }
  }
}
