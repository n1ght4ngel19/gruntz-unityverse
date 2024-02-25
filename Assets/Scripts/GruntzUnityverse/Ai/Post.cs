using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using GruntzUnityverse.Pathfinding;
using GruntzUnityverse.Utils.Extensionz;
using UnityEngine;

namespace GruntzUnityverse.AI {
public class Post : MonoBehaviour {
	public List<BaseAI> managedAI;
	public List<Grunt> spottedEnemiez;
	public BoxCollider2D boxCollider2D;
	public int range;

	public Vector2 location2D;
	public Node node;

	private void Start() {
		location2D = Vector2Int.RoundToInt(transform.position);
		node = Level.Instance.levelNodes.First(n => n.location2D == location2D);

		float wh = range * 2 + 0.1f;
		boxCollider2D.size = new Vector2(wh, wh);
	}

	private void OnValidate() {
		float wh = range * 2 + 1f;
		boxCollider2D.size = new Vector2(wh, wh);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		Grunt grunt = other.gameObject.GetComponent<Grunt>();

		if (grunt == null || grunt.CompareTag("Dizgruntled")) {
			return;
		}

		spottedEnemiez.IfNotContainsAdd(grunt);
		managedAI.ForEach(mg => mg.DecideAttack());
	}

	private void OnTriggerExit2D(Collider2D other) {
		Grunt grunt = other.gameObject.GetComponent<Grunt>();

		if (grunt == null || grunt.CompareTag("Dizgruntled")) {
			return;
		}

		spottedEnemiez.RemoveIfContains(grunt);
		managedAI.ForEach(mg => mg.DecideAttack());
	}
}
}
