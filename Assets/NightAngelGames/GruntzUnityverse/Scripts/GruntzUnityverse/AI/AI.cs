using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.AI {
public abstract class AI : MonoBehaviour {
	public GameManager gameManager;

	public int range;

	public Vector2Int startingLocation;

	protected Node originalNode => Level.instance.levelNodes.First(n => n.location2D == startingLocation);

	protected Grunt self => GetComponent<Grunt>();

	protected List<Grunt> gruntzInRange => gameManager.playerGruntz.Where(grunt => InRange(grunt.node)).ToList();

	protected bool InOriginalRange(Node otherNode) {
		return Mathf.Abs(originalNode.location2D.x - otherNode.location2D.x) <= range
			&& Mathf.Abs(originalNode.location2D.y - otherNode.location2D.y) <= range;
	}

	protected bool InRange(Node otherNode) {
		return Mathf.Abs(self.node.location2D.x - otherNode.location2D.x) <= range
			&& Mathf.Abs(self.node.location2D.y - otherNode.location2D.y) <= range;
	}

	private void Start() {
		gameManager = FindFirstObjectByType<GameManager>();

		startingLocation = Vector2Int.RoundToInt(transform.position);
	}

	protected abstract void Update();
}
}
