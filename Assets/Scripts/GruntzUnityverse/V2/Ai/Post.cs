using System.Collections.Generic;
using GruntzUnityverse.V2.Grunt;
using GruntzUnityverse.V2.Objectz;
using GruntzUnityverse.V2.Utils;
using UnityEngine;

namespace GruntzUnityverse.V2.Ai {
public class Post : GridObject {
	public List<DumbChaser> managedGruntz;
	public List<GruntV2> targetedGruntz;
	public BoxCollider2D boxCollider2D;
	public int range;

	protected override void Start() {
		base.Start();

		float wh = range * 2 + 0.1f;
		boxCollider2D.size = new Vector2(wh, wh);
	}

	private void OnValidate() {
		boxCollider2D = GetComponent<BoxCollider2D>();
		float wh = range * 2 + 1f;
		boxCollider2D.size = new Vector2(wh, wh);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		GruntV2 grunt = other.gameObject.GetComponent<GruntV2>();

		if (grunt == null || grunt.gameObject.CompareTag("Dizgruntled")) {
			return;
		}

		targetedGruntz.NotContainsAdd(grunt);
		managedGruntz.ForEach(gr => gr.ActOnTarget());
	}

	private void OnTriggerExit2D(Collider2D other) {
		GruntV2 grunt = other.gameObject.GetComponent<GruntV2>();

		if (grunt == null || grunt.gameObject.CompareTag("Dizgruntled")) {
			return;
		}

		targetedGruntz.ContainsRemove(grunt);
		managedGruntz.ForEach(gr => gr.ReturnToPost());
	}
}
}
