using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class CheckpointSwitch : ObjectSwitch {
    private List<CheckpointPyramid> Pyramidz { get; set; }
    [field: SerializeField] public ToolName RequiredTool { get; set; }
    [field: SerializeField] public ToyName RequiredToy { get; set; }


    protected override void Start() {
      base.Start();

      Pyramidz = transform.parent.GetComponentsInChildren<CheckpointPyramid>().ToList();
    }

    private void Update() {
      if (Pyramidz.Count.Equals(0)) {
        Debug.LogError("There is no Pyramid assigned to this Switch, this way the Checkpoint won't work properly!");

        enabled = false;
      }

      if (LevelManager.Instance.AllGruntz.Any(
        grunt => grunt.IsOnLocation(Location)
          && (grunt.HasTool(RequiredTool)
            || grunt.HasToy(RequiredToy)
            || RequiredTool == ToolName.Barehandz
            || RequiredToy == ToyName.None)
      )) {
        PressSwitch();
      } else {
        ReleaseSwitch();
      }
    }
  }
}
