using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.AI {
public abstract class AI : MonoBehaviour {
	public int range;
	public Vector2Int originalLocation;
	protected Node originalNode => Level.instance.levelNodes.First(n => n.location2D == originalLocation);
	protected Grunt self => GetComponent<Grunt>();

	protected bool InOriginalRange(Node otherNode) {
		return Mathf.Abs(originalNode.location2D.x - otherNode.location2D.x) <= range
			&& Mathf.Abs(originalNode.location2D.y - otherNode.location2D.y) <= range;
	}
	
	protected bool InRange(Node otherNode) {
		return Mathf.Abs(self.node.location2D.x - otherNode.location2D.x) <= range
			&& Mathf.Abs(self.node.location2D.y - otherNode.location2D.y) <= range;
	}

	private void Start() {
		originalLocation = Vector2Int.RoundToInt(transform.position);
	}

	// public void Setup() {
	// 	originalLocation = Vector2Int.RoundToInt(transform.position);
	// }

	protected abstract void FixedUpdate();
}
}
