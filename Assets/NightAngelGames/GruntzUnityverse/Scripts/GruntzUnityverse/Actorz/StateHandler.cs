﻿using System;
using NaughtyAttributes;
using UnityEngine;

namespace GruntzUnityverse.Actorz {
public class StateHandler : MonoBehaviour {
	public bool isInstance => gameObject.scene.name != null;

	[DisableIf(nameof(isInstance))]
	public Grunt grunt;

	[ReadOnly]
	public State currentState;

	[ReadOnly]
	public State goToState;

	public void SwitchState() {
		currentState = goToState;

		switch (goToState) {
			case State.Walking: {
				grunt.TryWalk();

				break;
			}
			case State.Idle: {
				grunt.Idle();

				break;
			}
			case State.Interacting: {
				grunt.TryTakeAction(grunt.interactionTarget.node, "Interact");

				break;
			}
			case State.Attacking: {
				grunt.TryTakeAction(grunt.attackTarget.node, "Attack");

				break;
			}
			case State.HostileIdle: {
				grunt.Idle(true);

				break;
			}
			case State.Giving: {
				grunt.TryGiveToy();

				break;
			}
			case State.Dying:
				break;
			case State.Playing:
				break;
			case State.Committed:
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(goToState), goToState, "Invalid state");
		}
	}

	public enum State {
		Idle = 0,
		Walking = 1,
		Interacting = 2,
		Attacking = 3,
		HostileIdle = 4,
		Giving = 5,
		Dying = 6,
		Playing = 7,
		Committed = 8,
	}
}
}
