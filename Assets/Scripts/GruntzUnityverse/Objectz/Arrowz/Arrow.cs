using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Pathfinding;
using GruntzUnityverse.Utility;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Arrowz {
  public class Arrow : MapObject {
    public Direction direction;


    protected override void Start() {
      base.Start();

      SetDirection();
    }

    protected virtual void Update() {
      if (!isValidated) {
        ValidateSetup();
      }

      foreach (Grunt grunt in LevelManager.Instance.allGruntz.Where(grunt => grunt.AtNode(ownNode))) {
        Node targetNode = LevelManager.Instance.NodeAt(location + Vector2Direction.FromDirection(direction));
        grunt.navigator.targetNode = targetNode;
        grunt.navigator.haveMoveCommand = true;
        grunt.navigator.isMoveForced = true;

        return;
      }
    }

    protected override void ValidateSetup() {
      if (direction == Direction.None) {
        Debug.LogError("Arrow direction is None, disabling Arrow.");

        enabled = false;
      }

      isValidated = true;
    }

    private void SetDirection() {
      string spriteName = spriteRenderer.sprite.name;

      direction = spriteName.Contains(StringDirection.Northeast)
        ? Direction.Northeast
        : spriteName.Contains(StringDirection.Northwest)
        ? Direction.Northwest
        : spriteName.Contains(StringDirection.North)
        ? Direction.North
        : spriteName.Contains(StringDirection.Southeast)
        ? Direction.Southeast
        : spriteName.Contains(StringDirection.Southwest)
        ? Direction.Southwest
        : spriteName.Contains(StringDirection.South)
        ? Direction.South
        : spriteName.Contains(StringDirection.East)
        ? Direction.East
        : spriteName.Contains(StringDirection.West)
        ? Direction.West
        : Direction.None;
    }
  }
}
