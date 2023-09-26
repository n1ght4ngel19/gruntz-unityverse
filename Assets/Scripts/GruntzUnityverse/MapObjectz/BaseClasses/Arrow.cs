using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Pathfinding;
using GruntzUnityverse.Utility;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz.BaseClasses {
  public class Arrow : MapObject {
    public Direction direction;
    public Node nodeInDirection;

    protected virtual void Update() {
      foreach (Grunt grunt in GameManager.Instance.currentLevelManager.allGruntz
        .Where(grunt => grunt.navigator.ownNode == ownNode && !grunt.navigator.isMoveForced)) {
        grunt.navigator.targetNode = nodeInDirection;
        grunt.navigator.isMoveForced = true;
        grunt.haveMoveCommand = true;

        return;
      }

      foreach (RollingBall ball in GameManager.Instance.currentLevelManager.rollingBallz
        .Where(rollingBall => rollingBall.ownNode == ownNode)) {
        ball.ChangeDirection(direction);

        return;
      }
    }

    public override void Setup() {
      base.Setup();

      string spriteName = spriteRenderer.sprite.name;

      if (spriteName.Contains(Direction.East.ToString())) {
        direction = Direction.East;
      } else if (spriteName.Contains(Direction.North.ToString())) {
        direction = Direction.North;
      } else if (spriteName.Contains(Direction.Northeast.ToString())) {
        direction = Direction.Northeast;
      } else if (spriteName.Contains(Direction.Northwest.ToString())) {
        direction = Direction.Northwest;
      } else if (spriteName.Contains(Direction.South.ToString())) {
        direction = Direction.South;
      } else if (spriteName.Contains(Direction.Southeast.ToString())) {
        direction = Direction.Southeast;
      } else if (spriteName.Contains(Direction.Southwest.ToString())) {
        direction = Direction.Southwest;
      } else if (spriteName.Contains(Direction.West.ToString())) {
        direction = Direction.West;
      } else {
        Debug.LogError($"Arrow sprite name {spriteName} does not contain any direction, disabling Arrow.");

        enabled = false;
      }

      SetDirection();
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

      nodeInDirection = GameManager.Instance.currentLevelManager.NodeAt(location + Vector2Direction.FromDirection(direction));
    }
  }
}
