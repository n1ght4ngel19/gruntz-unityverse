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

    protected override void Start() {
      base.Start();

      SetDirection();
    }

    protected virtual void Update() {
      if (!isValidated) {
        ValidateSetup();
      }

      foreach (Grunt grunt in GameManager.Instance.currentLevelManager.allGruntz
        .Where(grunt => grunt.AtNode(ownNode) && !grunt.navigator.isMoveForced)) {
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

      nodeInDirection = GameManager.Instance.currentLevelManager.NodeAt(location + Vector2Direction.FromDirection(direction));
    }
  }
}
