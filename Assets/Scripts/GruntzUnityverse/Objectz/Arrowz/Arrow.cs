using GruntzUnityverse.Actorz;
using GruntzUnityverse.Actorz.BehaviourManagement;
using GruntzUnityverse.Animation;
using GruntzUnityverse.Core;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Arrowz {
public class Arrow : GridObject {
	public Direction direction;
	public Node pointedNode;

	public override void Setup() {
		base.Setup();

		SetDirection();
		SetTargetNode();
	}

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

	protected void SetTargetNode() {
		pointedNode = direction switch {
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

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent(out Grunt grunt)) {
			grunt.node = node;
			grunt.transform.position = transform.position;

			if (node.gruntOnNode != null && node.gruntOnNode != grunt) {
				node.gruntOnNode.Die(AnimationManager.Instance.squashDeathAnimation);
			}

			grunt.interactionTarget = null;
			grunt.attackTarget = null;

			grunt.travelGoal = pointedNode;
			grunt.intent = Intent.ToMove;
			grunt.EvaluateState();

			node.gruntOnNode = grunt;
			grunt.spriteRenderer.sortingOrder = 10;
		}

		if (other.TryGetComponent(out RollingBall ball)) {
			ball.direction = direction;
			ball.next = pointedNode;
		}
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (other.TryGetComponent(out Grunt grunt)) {
			grunt.spriteRenderer.sortingOrder = 12;
		}
	}
}
}
