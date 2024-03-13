using System.Collections;
using GruntzUnityverse.Actorz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Hazardz {
public class Spikez : Hazard {
	public float damageRate;

	protected override IEnumerator Damage() {
		while (enabled) {
			yield return new WaitUntil(() => GruntOnTop != null);

			// If GruntOnTop becomes null while waiting, we won't damage anything, even though we should
			// That's why we need to store the GruntOnTop in a variable before waiting
			Grunt toDamage = GruntOnTop;

			yield return new WaitForSeconds(damageRate);

			toDamage.TakeDamage(damage);
		}
	}
}
}
