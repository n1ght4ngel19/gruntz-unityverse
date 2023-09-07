using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Brickz {
  public class BrickFoundation : MonoBehaviour {
    private void Start() {
      GameManager.Instance.currentLevelManager.BrickFoundationz.Add(this);
    }
  }
}
