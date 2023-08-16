using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.MapObjectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class CheckpointSwitch : ObjectSwitch {
    private List<CheckpointPyramid> _pyramidz;
    private ToolName _requiredTool;
    private ToyName _requiredToy;


    protected override void Start() {
      base.Start();

      _pyramidz = transform.parent.GetComponentsInChildren<CheckpointPyramid>().ToList();

      SetupSwitch();
    }

    private void Update() {
      // Todo: Replace with proper error handling like with Arrow Switchez
      if (_pyramidz.Count.Equals(0)) {
        Debug.LogError("There is no Pyramid assigned to this Switch, this way the Checkpoint won't work properly!");

        enabled = false;
      }

      if (IsRequirementSatisfied()) {
        PressSwitch();
      } else {
        ReleaseSwitch();
      }
    }

    private bool IsRequirementSatisfied() {
      foreach (Grunt grunt in LevelManager.Instance.allGruntz) {
        if (!grunt.AtNode(ownNode)) {
          continue;
        }

        if (_requiredToy is not ToyName.None && grunt.HasToy(_requiredToy)) {
          return true;
        }

        if (_requiredTool is ToolName.Barehandz) {
          return true;
        }

        if (grunt.HasTool(_requiredTool)) {
          return true;
        }
      }

      return false;
    }

    private void SetupSwitch() {
      string spriteName = spriteRenderer.sprite.name;

      if (spriteName.Contains(ToolName.Gauntletz.ToString())) {
        _requiredTool = ToolName.Gauntletz;
      } else if (spriteName.Contains(ToolName.Shovel.ToString())) {
        _requiredTool = ToolName.Shovel;
      } else if (spriteName.Contains(ToolName.Warpstone.ToString())) {
        _requiredTool = ToolName.Warpstone;
      } else {
        _requiredTool = ToolName.Barehandz;
      }

      // Todo: Others
    }
  }
}
