using System.Collections;
using GruntzUnityverse.Actorz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Hazardz {
public class Spikez : GridObject {
	public int damage = 2;

	// private void OnTriggerEnter2D(Collider2D other) {
	//   GruntV2 target = other.gameObject.GetComponent<GruntV2>();
	//
	//   if (target == null) {
	//     return;
	//   }
	// }

	private IEnumerator OnTriggerStay2D(Collider2D other) {
		Grunt target = other.gameObject.GetComponent<Grunt>();

		if (target == null) {
			yield break;
		}

		// target.TakeDamage(damage);

		yield return new WaitForSeconds(2f);
	}

	// private void OnTriggerExit2D(Collider2D other) {
	//   GruntV2 target = other.gameObject.GetComponent<GruntV2>();
	//
	//   if (target == null) {
	//     return;
	//   }
	// }
}
}
