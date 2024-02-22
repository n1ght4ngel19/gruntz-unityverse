using System.Collections.Generic;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Objectz;
using UnityEngine;

namespace GruntzUnityverse.Ai {
public class Post : GridObject {
	public List<DumbChaser> managedGruntz;
	public List<Grunt> targetedGruntz;
	public BoxCollider2D boxCollider2D;
	public int range;

	// protected override void Start() {
	// 	base.Start();
	//
	// 	float wh = range * 2 + 0.1f;
	// 	boxCollider2D.size = new Vector2(wh, wh);
	// }
	//
	// private void OnValidate() {
	// 	boxCollider2D = GetComponent<BoxCollider2D>();
	// 	float wh = range * 2 + 1f;
	// 	boxCollider2D.size = new Vector2(wh, wh);
	// }
	//
	// private void OnTriggerEnter2D(Collider2D other) {
	// 	Grunt grunt = other.gameObject.GetComponent<Grunt>();
	//
	// 	if (grunt == null || grunt.gameObject.CompareTag("Dizgruntled")) {
	// 		return;
	// 	}
	//
	// 	targetedGruntz.NotContainsAdd(grunt);
	// 	managedGruntz.ForEach(gr => gr.ActOnTarget());
	// }
	//
	// private void OnTriggerExit2D(Collider2D other) {
	// 	Grunt grunt = other.gameObject.GetComponent<Grunt>();
	//
	// 	if (grunt == null || grunt.gameObject.CompareTag("Dizgruntled")) {
	// 		return;
	// 	}
	//
	// 	targetedGruntz.ContainsRemove(grunt);
	// 	managedGruntz.ForEach(gr => gr.ReturnToPost());
	// }
}
}
