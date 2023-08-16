using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Brickz {
  public class BrickFoundation : MonoBehaviour {
    private void Start() {
      LevelManager.Instance.BrickFoundationz.Add(this);
    }
  }
}
