using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Switchez;

namespace GruntzUnityverse.Objectz.Pyramidz {
  public class CheckpointPyramid : Pyramid {
    private List<CheckpointSwitch> Switchez { get; set; }

    protected override void Start() {
      base.Start();

      Switchez = transform.parent.GetComponentsInChildren<CheckpointSwitch>().ToList();
    }

    private void Update() {
      if (Switchez.Any(checkpointSwitch => !checkpointSwitch.IsPressed)) {
        return;
      }

      TogglePyramid();

      foreach (CheckpointSwitch sw in Switchez) {
        sw.enabled = false;
      }

      enabled = false;
    }
  }
}
