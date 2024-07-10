using Cysharp.Threading.Tasks;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using GruntzUnityverse.Itemz.Base;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace GruntzUnityverse.Itemz.Misc {
public class Helpbox : LevelItem {
	[ResizableTextArea]
	public string helpboxText;


	public void SetText(string text) {
		helpboxText = text;
	}

	protected override async void Pickup(Grunt targetGrunt) {
		targetGrunt.GoToState(StateHandler.State.Committed);
		targetGrunt.enabled = false;

		targetGrunt.animancer.Play(pickupAnim);
		await UniTask.WaitForSeconds(pickupAnim.length);

		Time.timeScale = 0f;

		gameManager.helpboxUI.GetComponentInChildren<TMP_Text>().SetText(helpboxText);
		gameManager.helpboxUI.GetComponent<Canvas>().enabled = true;

		await UniTask.WaitUntil(() => Time.timeScale != 0f);

		gameManager.helpboxUI.GetComponent<Canvas>().enabled = false;

		targetGrunt.enabled = true;

		targetGrunt.GoToState(StateHandler.State.Walking);
	}

	protected override void Start() {
		if (!Settings.instance.gameSettings.showHelpboxez) {
			gameObject.SetActive(false);
		} else {
			base.Start();
		}
	}
}
}
