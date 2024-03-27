using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using GruntzUnityverse.Itemz.Base;
using TMPro;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Misc {
public class Helpbox : LevelItem {
	public string helpboxText;

	protected override void Start() {
		if (!Settings.instance.gameSettings.showHelpboxez) {
			gameObject.SetActive(false);
		} else {
			base.Start();
		}
	}

	public void SetText(string text) {
		helpboxText = text;
	}

	protected override IEnumerator Pickup(Grunt targetGrunt) {
		targetGrunt.enabled = false;

		targetGrunt.animancer.Play(pickupAnim);

		yield return new WaitForSeconds(pickupAnim.length);

		Time.timeScale = 0f;

		GameManager.instance.helpboxUI.GetComponentInChildren<TMP_Text>().SetText(helpboxText);
		GameManager.instance.helpboxUI.GetComponent<Canvas>().enabled = true;

		yield return new WaitUntil(() => Time.timeScale != 0f);

		GameManager.instance.helpboxUI.GetComponent<Canvas>().enabled = false;

		targetGrunt.enabled = true;

		targetGrunt.GoToState(StateHandler.State.Walking);
	}

	private void OnTriggerExit2D(Collider2D other) {
		if (!other.TryGetComponent(out Grunt _)) {
			return;
		}

		GetComponent<SpriteRenderer>().enabled = true;
	}
}
}
