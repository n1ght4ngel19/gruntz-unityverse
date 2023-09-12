using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz.Pyramidz;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.Switchez {
  public class CheckpointSwitch : ObjectSwitch {
    private List<CheckpointPyramid> _pyramidz;
    private ToolName _requiredTool;
    private ToyName _requiredToy;


    protected override void Start() {
      base.Start();

      _pyramidz = parent.GetComponentsInChildren<CheckpointPyramid>().ToList();

      if (_pyramidz.Count.Equals(0)) {
        DisableWithError("There is no Checkpoint Pyramid assigned to this Switch, this way the Checkpoint won't work properly!");
      }

      SetupSwitch();
      SetupSpritez();
    }

    private void Update() {
      // Todo: Replace with proper error handling like with Arrow Switchez


      if (IsRequirementSatisfied()) {
        PressSwitch();
      } else {
        ReleaseSwitch();
      }
    }

    protected override void SetupSpritez() {
      string typeAsString = GetType().ToString().Split(".").Last();

      releasedSprite = spriteRenderer.sprite;

      string requiredItemName = _requiredTool is ToolName.None
        ? _requiredToy.ToString()
        : _requiredTool.ToString();

      Debug.Log(requiredItemName);

      Addressables.LoadAssetAsync<Sprite[]>($"{GlobalNamez.SwitchSpritezPath}/{typeAsString}_{requiredItemName}.png").Completed +=
        handle => {
          pressedSprite = handle.Result[1];
        };
    }

    private bool IsRequirementSatisfied() {
      foreach (Grunt grunt in GameManager.Instance.currentLevelManager.allGruntz) {
        if (!grunt.AtNode(ownNode)) {
          continue;
        }

        if (_requiredToy is not ToyName.None && grunt.HasToy(_requiredToy)) {
          return true;
        }

        if (_requiredTool is not ToolName.None && (grunt.HasTool(_requiredTool) || _requiredTool is ToolName.Barehandz)) {
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
      } else if (spriteName.Contains(ToolName.Barehandz.ToString())) {
        _requiredTool = ToolName.Barehandz;
      } else {
        _requiredTool = ToolName.None;
      } // Todo: Other tools

      // Todo: Toy names
    }
  }
}
