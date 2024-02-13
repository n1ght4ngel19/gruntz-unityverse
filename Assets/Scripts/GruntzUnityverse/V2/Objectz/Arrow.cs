using GruntzUnityverse.V2.Grunt;
using GruntzUnityverse.V2.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.V2.Objectz {
public class Arrow : GridObject {
	public DirectionV2 direction;
	public NodeV2 targetNode;

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
			direction = DirectionV2.UpRight;
		} else if (spriteName.Contains("UpLeft")) {
			direction = DirectionV2.UpLeft;
		} else if (spriteName.Contains("DownRight")) {
			direction = DirectionV2.DownRight;
		} else if (spriteName.Contains("DownLeft")) {
			direction = DirectionV2.DownLeft;
		} else if (spriteName.Contains("Right")) {
			direction = DirectionV2.Right;
		} else if (spriteName.Contains("Left")) {
			direction = DirectionV2.Left;
		} else if (spriteName.Contains("Down")) {
			direction = DirectionV2.Down;
		} else if (spriteName.Contains("Up")) {
			direction = DirectionV2.Up;
		} else {
			Debug.LogError("Arrow sprite name does not contain a valid direction.");
		}
	}

	protected void SetTargetNode() {
		targetNode = direction switch {
			DirectionV2.Up => node.neighbourSet.up,
			DirectionV2.UpRight => node.neighbourSet.upRight,
			DirectionV2.Right => node.neighbourSet.right,
			DirectionV2.DownRight => node.neighbourSet.downRight,
			DirectionV2.Down => node.neighbourSet.down,
			DirectionV2.DownLeft => node.neighbourSet.downLeft,
			DirectionV2.Left => node.neighbourSet.left,
			DirectionV2.UpLeft => node.neighbourSet.upLeft,
			_ => null,
		};
	}

	private void OnTriggerEnter2D(Collider2D other) {
		GruntV2 grunt = other.GetComponent<GruntV2>();

		if (grunt == null) {
			return;
		}

		grunt.node = node;
		grunt.location2D = node.location2D;
		grunt.flagz.moving = false;
		grunt.flagz.interrupted = false;

		grunt.flagz.moveForced = true;
		grunt.targetNode = targetNode;
		grunt.next = targetNode;

		Debug.Log("onNodeChanged");
		grunt.onNodeChanged.Invoke();
	}
}
}
