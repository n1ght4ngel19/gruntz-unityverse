using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Brickz {
  public class BrickFoundation : MonoBehaviour {
    private void Start() {
      LevelManager.Instance.BrickFoundationz.Add(this);
    }
  }
}
