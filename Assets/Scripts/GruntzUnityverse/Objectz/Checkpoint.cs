using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Pyramidz;
using GruntzUnityverse.Objectz.Switchez;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class Checkpoint : MonoBehaviour {
    private List<CheckpointSwitch> _switchez;
    private List<CheckpointPyramid> _pyramidz;

    private void Start() {
      _switchez = transform.parent.GetComponentsInChildren<CheckpointSwitch>().ToList();
      _pyramidz = transform.parent.GetComponentsInChildren<CheckpointPyramid>().ToList();
    }

    private void Update() {
      if (_switchez.Any(checkpointSwitch => !checkpointSwitch.isPressed)) {
        return;
      }

      Complete();
    }

    private void Complete() {
      foreach (CheckpointPyramid pyramid in _pyramidz) {
        pyramid.TogglePyramid();
        pyramid.enabled = false;
      }

      foreach (CheckpointSwitch sw in _switchez) {
        sw.enabled = false;
      }

      // Todo: Save game here

      enabled = false;
    }
  }
}
