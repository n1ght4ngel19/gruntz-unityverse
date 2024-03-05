using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Actorz.BehaviourManagement;
using GruntzUnityverse.Core;
using GruntzUnityverse.Itemz.Base;
using TMPro;
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
		targetGrunt.next.isReserved = false;

		yield return new WaitForSeconds(pickupAnim.length);

		Time.timeScale = 0f;

		GameManager.Instance.helpboxUI.GetComponentInChildren<TMP_Text>().SetText(helpboxText);
		GameManager.Instance.helpboxUI.SetActive(true);

		Debug.Log(helpboxText); // Todo: Show helpbox UI

		yield return new WaitUntil(() => Time.timeScale != 0f);

		GameManager.Instance.helpboxUI.SetActive(false);

		targetGrunt.enabled = true;
		targetGrunt.intent = Intent.ToIdle;
		targetGrunt.EvaluateState(whenFalse: targetGrunt.BetweenNodes);
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (!other.TryGetComponent(out Grunt grunt)) {
			return;
		}

		GetComponent<SpriteRenderer>().enabled = true;
	}
}
}
