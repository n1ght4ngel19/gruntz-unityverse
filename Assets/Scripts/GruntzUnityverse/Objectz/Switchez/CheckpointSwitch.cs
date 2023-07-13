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

      string spriteName = SpriteRenderer.sprite.name;
    }

    private void Update() {
      // Todo: Replace with proper error handling like with Arrow Switchez
      if (Pyramidz.Count.Equals(0)) {
        Debug.LogError("There is no Pyramid assigned to this Switch, this way the Checkpoint won't work properly!");

        enabled = false;
      }

      if (LevelManager.Instance.allGruntz.Any(
        grunt => grunt.AtLocation(location)
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
