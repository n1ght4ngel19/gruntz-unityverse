using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
  public class CheckpointSwitch : ObjectSwitch {
    private Checkpoint _ownCheckpoint;
    private List<CheckpointPyramid> _pyramidz;
    private ToolName _requiredTool;
    private ToyName _requiredToy;


    protected override void Start() {
      base.Start();

      _ownCheckpoint = transform.parent.GetComponent<Checkpoint>();
      _pyramidz = transform.parent.GetComponentsInChildren<CheckpointPyramid>().ToList();

      SetupSprite();
    }

    private void Update() {
      // Todo: Replace with proper error handling like with Arrow Switchez
      if (_pyramidz.Count.Equals(0)) {
        Debug.LogError("There is no Pyramid assigned to this Switch, this way the Checkpoint won't work properly!");

        enabled = false;
      }

      foreach (Grunt grunt in LevelManager.Instance.allGruntz) {
        if (!grunt.AtLocation(location)) {
          continue;
        }

        if (_requiredToy is not ToyName.None && grunt.HasToy(_requiredToy)) {
          PressSwitch();
          break;
        }

        if (_requiredTool is ToolName.Barehandz) {
          PressSwitch();
          break;
        }

        if (grunt.HasTool(_requiredTool)) {
          PressSwitch();
          break;
        }

        ReleaseSwitch();
      }
    }

    private void SetupSprite() {
      string spriteName = SpriteRenderer.sprite.name;

      if (spriteName.Contains(ToolName.Gauntletz.ToString())) {
        _requiredTool = ToolName.Gauntletz;
      }
      else if (spriteName.Contains(ToolName.Shovel.ToString())) {
        _requiredTool = ToolName.Shovel;
      }
      else if (spriteName.Contains(ToolName.Warpstone.ToString())) {
        _requiredTool = ToolName.Warpstone;
      }
      else {
        _requiredTool = ToolName.Barehandz;
      }

      // Todo: Others
    }
  }
}
