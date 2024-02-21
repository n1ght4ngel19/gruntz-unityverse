﻿using System.Collections;
using GruntzUnityverse.V2.Actorz;
using UnityEngine;

namespace GruntzUnityverse.V2.Itemz.Misc {
public class Helpbox : LevelItem {
	public string helpboxText;

	public void SetText(string text) {
		helpboxText = text;
	}

	protected override IEnumerator Pickup(Grunt targetGrunt) {
		yield return base.Pickup(targetGrunt);

		Time.timeScale = 0f;

		// Todo: Show helpbox UI
		Debug.Log(helpboxText);
	}
}
}
