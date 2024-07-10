using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Core;
using GruntzUnityverse.Itemz.Toolz;
using GruntzUnityverse.Objectz.Misc;
using GruntzUnityverse.UI;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
public class Fort : GridObject {
	public List<Flag> fortFlagz;

	protected override void Start() {
		base.Start();

		fortFlagz = transform.parent.GetComponentsInChildren<Flag>().ToList();
		fortFlagz.ForEach(fl => fl.PlayAnim());
	}

	protected override async void OnTriggerEnter2D(Collider2D other) {
		if (!other.TryGetComponent(out Grunt grunt)) {
			return;
		}

		if (grunt.tool is not Warpstone) {
			return;
		}

		gameManager.gruntz.ForEach(g => g.enabled = false);

		gameManager.playerGruntz.ForEach(pg => pg.animancer.Play(AnimationManager.instance.gruntVictoryAnimz.First()));
		await UniTask.WaitForSeconds(AnimationManager.instance.gruntVictoryAnimz.First().length);

		gameManager.playerGruntz.ForEach(pg => pg.animancer.Play(AnimationManager.instance.gruntWarpEnterAnim));
		await UniTask.WaitForSeconds(AnimationManager.instance.gruntWarpEnterAnim.length);

		gameManager.gruntz.ForEach(g => g.spriteRenderer.enabled = false);

		await UniTask.WaitForSeconds(1f);

		Level.instance.gameObject.SetActive(false);
		gameManager.actorz.SetActive(false);
		gameManager.itemz.SetActive(false);
		gameManager.objectz.SetActive(false);

		gameManager.GetComponent<Settings>().gameSettings.quickStartLevel = gameManager.levelData;

		StatzMenu.instance.Activate();
	}
}
}
