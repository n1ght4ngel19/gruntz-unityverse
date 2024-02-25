using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Actorz.BehaviourManagement;
using GruntzUnityverse.Itemz.Base;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Misc {
public class Helpbox : LevelItem {
	public string helpboxText;

	public void SetText(string text) {
		helpboxText = text;
	}

	protected override IEnumerator Pickup(Grunt targetGrunt) {
		targetGrunt.Animancer.Play(pickupAnim);
		targetGrunt.enabled = false;

		yield return new WaitForSeconds(pickupAnim.length);

		Time.timeScale = 0f;
		Debug.Log(helpboxText); // Todo: Show helpbox UI

		yield return new WaitUntil(() => Time.timeScale != 0f);

		targetGrunt.enabled = true;
		targetGrunt.intent = Intent.ToIdle;
		targetGrunt.EvaluateState(whenFalse: targetGrunt.BetweenNodes);
	}
}
}
