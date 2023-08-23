using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.MapObjectz.Pyramidz;
using GruntzUnityverse.MapObjectz.Switchez;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz {
  public class Checkpoint : MonoBehaviour {
    private List<CheckpointSwitch> _switchez;
    private List<CheckpointPyramid> _pyramidz;
    // ------------------------------------------------------------ //

    private void Start() {
      _switchez = transform.parent.GetComponentsInChildren<CheckpointSwitch>().ToList();
      _pyramidz = transform.parent.GetComponentsInChildren<CheckpointPyramid>().ToList();
    }
    // ------------------------------------------------------------ //

    private void Update() {
      if (_switchez.Any(checkpointSwitch => !checkpointSwitch.isPressed)) {
        return;
      }

      Complete();
    }
    // ------------------------------------------------------------ //

    /// <summary>
    /// Takes care of everything related to completing a Checkpoint,
    /// such as saving the game or disabling the Checkpoint.
    /// </summary>
    private void Complete() {
      _pyramidz.ForEach(pyramid => {
        pyramid.Toggle();
        pyramid.enabled = false;
      });

      _switchez.ForEach(sw => sw.enabled = false);

      // Todo: Save game here

      enabled = false;
    }
  }
}
