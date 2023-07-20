using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class Checkpoint : MonoBehaviour {
    public bool isCompleted;

    public void Complete() {
      isCompleted = true;
      enabled = false;
    }
  }
}
