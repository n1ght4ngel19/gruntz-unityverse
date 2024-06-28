using System;
using System.Collections.Generic;
using Animancer;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Animation;
using GruntzUnityverse.Itemz.Base;
using GruntzUnityverse.Itemz.Toolz;
using GruntzUnityverse.Objectz;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse {
public class NewGrunt : MonoBehaviour {
	[Header("Info")]
	public bool selected;

	public float moveSpeed;

	public bool betweenTiles => transform.position.x % 1 != 0 || transform.position.y % 1 != 0;

	public bool isMoving => !betweenTiles;

	public bool canSwitchCommand => !betweenTiles;

	public bool canFly => tool is Wingz;

	[Header("Equipment")]
	public EquippedTool tool;

	public EquippedToy toy;

	public List<EquippedPowerup> powerupz;

	[Header("Animation")]
	public AnimancerComponent animancer;

	public AnimationPack animationPack;

	public Direction facingDirection;

	/// <summary>
	/// The node this Grunt is currently on.
	/// </summary>
	[Header("Pathfinding")]
	public Node node;

	/// <summary>
	/// The node the Grunt is moving towards.
	/// </summary>
	public Node goalNode;

	/// <summary>
	/// The next node the Grunt will move to.
	/// </summary>
	public Node nextNode;

	/// <summary>
	/// The location of this Grunt in 2D space.
	/// </summary>
	private Vector2Int location2D => Vector2Int.RoundToInt(transform.position);

	/// <summary>
	/// The target the Grunt will try to interact with.
	/// </summary>
	[Header("Interaction")]
	public GridObject interactionTarget;

	/// <summary>
	/// The target the Grunt will try to attack.
	/// </summary>
	public Grunt attackTarget;

	/// <summary>
	/// The target the Grunt will try to give a toy to.
	/// </summary>
	public Grunt toyTarget;

	[Header("Commands")]
	public Command activeCommand;

	public Command newCommand;

	public Action onNodeChanged;

	private void OnEnable() {
		onNodeChanged += OnNodeChanged;
	}

	private void OnNodeChanged() {
		if (newCommand == Command.None) {
			return;
		}

		activeCommand = newCommand;
		newCommand = Command.None;

		ExecuteCommand(activeCommand);
	}

	public void Select() {
		selected = true;
	}

	public void ExecuteCommand(Command command) {
		if (!canSwitchCommand) {
			return;
		}

		switch (command) {
			case Command.Move:
				break;
			case Command.Interact:
				break;
			case Command.Attack:
				break;
			case Command.GiveToy:
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(command), command, null);
		}
	}

	public void Move() {
		Vector3 moveVector = (nextNode.transform.position - node.transform.position);
		gameObject.transform.position += moveVector * (Time.fixedDeltaTime / moveSpeed);
	}

	private void FixedUpdate() {
		if (betweenTiles) {
			Move();
		}
	}
}
}
