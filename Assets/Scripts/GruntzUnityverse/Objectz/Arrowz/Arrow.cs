using GruntzUnityverse.Actorz;
using GruntzUnityverse.Animation;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Arrowz {
public class Arrow : GridObject {
	public Direction direction;
	public Node pointedNode;

	protected override void Start() {
		base.Start();

		SetDirection();
		SetTargetNode();

		// Disable node's trigger so that the Arrow's behaviour/effect doesn't clash with the node's effect
		// Essentially, disable the node's effect to safely overwrite its effect
		node.circleCollider2D.isTrigger = false;
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
		Grunt grunt = other.GetComponent<Grunt>();

		if (grunt == null) {
			return;
		}

		grunt.node = node;
		grunt.location2D = node.location2D;
		// grunt.flagz.moving = false;
		// grunt.flagz.interrupted = false;
		// grunt.flagz.moveForced = true;

		node.isReserved = false;

		// grunt.targetNode = pointedNode;
		grunt.next = pointedNode;

		#region Reset
		// grunt.onNodeChanged.RemoveAllListeners();
		// grunt.onTargetReached.RemoveAllListeners();

		grunt.interactionTarget = null;
		grunt.attackTarget = null;

		// grunt.flagz.setToInteract = false;
		// grunt.flagz.setToAttack = false;
		// grunt.flagz.setToGive = false;
		#endregion

		grunt.transform.position = transform.position;
		// grunt.onNodeChanged.AddListener(grunt.MoveToNode);
		// grunt.onNodeChanged.Invoke();
	}
}
}
