using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Switchez;

namespace GruntzUnityverse.Objectz.Pyramidz {
  public class CheckpointPyramid : Pyramid {
    public Checkpoint ownCheckpoint;
    private List<CheckpointSwitch> switchez;

    protected override void Start() {
      base.Start();

      ownCheckpoint = transform.parent.GetComponent<Checkpoint>();
      switchez = transform.parent.GetComponentsInChildren<CheckpointSwitch>().ToList();
    }

    private void Update() {
      if (switchez.Any(checkpointSwitch => !checkpointSwitch.IsPressed)) {
        return;
      }

      TogglePyramid();
      ownCheckpoint.Complete();

      foreach (CheckpointSwitch sw in switchez) {
        sw.enabled = false;
      }

      enabled = false;
    }
  }
}
