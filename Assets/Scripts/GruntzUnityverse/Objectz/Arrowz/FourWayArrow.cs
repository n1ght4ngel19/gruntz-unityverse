using GruntzUnityverse.Actorz;
using GruntzUnityverse.Animation;
using GruntzUnityverse.Core;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Arrowz {
public class FourWayArrow : GridObject {
	private void OnTriggerEnter2D(Collider2D other) {
		if (other.TryGetComponent(out Grunt grunt)) {
			if (node.gruntOnNode != null && node.gruntOnNode != grunt) {
				node.gruntOnNode.Die(AnimationManager.instance.squashDeathAnimation);
			}

			grunt.node = node;
			grunt.transform.position = transform.position;
			grunt.spriteRenderer.sortingOrder = 10;

			grunt.forced = true;
			grunt.between = false;
			node.occupied = true;

			grunt.interactionTarget = null;
			grunt.attackTarget = null;

			grunt.travelGoal = grunt.facingDirection switch {
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

			grunt.GoToState(StateHandler.State.Walking);
		}

		if (other.TryGetComponent(out RollingBall ball)) {
			if (node.gruntOnNode != null) {
				node.gruntOnNode.Die(AnimationManager.instance.squashDeathAnimation);
			}

			ball.node = node;
			ball.transform.position = node.transform.position;

			ball.next = ball.direction switch {
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
	}
}
}
