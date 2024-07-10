using GruntzUnityverse.Actorz;
using GruntzUnityverse.Animation;
using GruntzUnityverse.Core;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Arrowz {
public class Arrow : GridObject {
	public Direction direction;

	public Node pointedNode;

	private void SetDirection() {
		string spriteName = spriteRenderer.sprite.name;

		if (spriteName.Contains("UpRight")) {
			direction = Direction.UpRight;
		} else if (spriteName.Contains("UpLeft")) {
			direction = Direction.UpLeft;
		} else if (spriteName.Contains("DownRight")) {
			direction = Direction.DownRight;
		} else if (spriteName.Contains("DownLeft")) {
			direction = Direction.DownLeft;
		} else if (spriteName.Contains("Right")) {
			direction = Direction.Right;
		} else if (spriteName.Contains("Left")) {
			direction = Direction.Left;
		} else if (spriteName.Contains("Down")) {
			direction = Direction.Down;
		} else if (spriteName.Contains("Up")) {
			direction = Direction.Up;
		} else {
			Debug.LogError("Arrow sprite name does not contain a valid direction.");
		}
	}

	protected void SetTargetNode(Direction towards) {
		pointedNode = towards switch {
			Direction.Up => node.neighbourSet.up,
			Direction.UpRight => node.neighbourSet.upRight,
			Direction.Right => node.neighbourSet.right,
			Direction.DownRight => node.neighbourSet.downRight,
			Direction.Down => node.neighbourSet.down,
			Direction.DownLeft => node.neighbourSet.downLeft,
			Direction.Left => node.neighbourSet.left,
			Direction.UpLeft => node.neighbourSet.upLeft,
			_ => null,
		};
	}

	// --------------------------------------------------
	// Lifecycle
	// --------------------------------------------------

	protected override void Start() {
		base.Start();

		SetDirection();
		SetTargetNode(direction);

		Deactivate();
	}

	protected override void OnTriggerEnter2D(Collider2D other) {
		base.OnTriggerEnter2D(other);

		if (other.TryGetComponent(out Grunt grunt)) {
			if (node.grunt != null && node.grunt != grunt && !node.grunt.between) {
				node.grunt.Die(AnimationManager.instance.squashDeathAnimation);
			}

			grunt.node = node;
			grunt.transform.position = transform.position;
			grunt.spriteRenderer.sortingOrder = 10;

			grunt.forced = true;
			grunt.between = false;
			node.occupied = true;

			grunt.interactionTarget = null;
			grunt.attackTarget = null;

			grunt.travelGoal = pointedNode;
			grunt.GoToState(StateHandler.State.Walking);
		}

		if (other.TryGetComponent(out RollingBall ball)) {
			if (node.grunt != null) {
				if (!node.grunt.between) {
					node.grunt.Die(AnimationManager.instance.squashDeathAnimation);
				}
			}

			ball.node = node;
			ball.transform.position = node.transform.position;
			ball.next = pointedNode;
			ball.direction = direction;

			ball.transform.rotation = Quaternion.identity;

			float rotateAngle = ball.direction switch {
				Direction.UpRight or Direction.DownLeft => -45,
				Direction.UpLeft or Direction.DownRight => 45,
				_ => 0,
			};

			ball.transform.Rotate(0, 0, rotateAngle);
		}
	}

	protected override void OnTriggerExit2D(Collider2D other) {
		if (other.TryGetComponent(out Grunt grunt)) {
			grunt.between = true;
			node.occupied = false;
			grunt.spriteRenderer.sortingOrder = 12;
		}
	}
}
}
