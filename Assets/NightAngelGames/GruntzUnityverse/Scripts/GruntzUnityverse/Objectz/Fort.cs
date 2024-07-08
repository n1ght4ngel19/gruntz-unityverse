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

	private GameManager _gameManager;

	private void Start() {
		fortFlagz = transform.parent.GetComponentsInChildren<Flag>().ToList();
		fortFlagz.ForEach(fl => fl.PlayAnim());

		_gameManager = FindFirstObjectByType<GameManager>();
	}

	private async void OnTriggerEnter2D(Collider2D other) {
		if (!other.TryGetComponent(out Grunt grunt)) {
			return;
		}

		if (grunt.tool is not Warpstone) {
			return;
		}

		_gameManager.allGruntz.ForEach(g => g.enabled = false);

		_gameManager.playerGruntz.ForEach(pg => pg.animancer.Play(AnimationManager.instance.gruntVictoryAnimz.First()));
		await UniTask.WaitForSeconds(AnimationManager.instance.gruntVictoryAnimz.First().length);

		_gameManager.playerGruntz.ForEach(pg => pg.animancer.Play(AnimationManager.instance.gruntWarpEnterAnim));
		await UniTask.WaitForSeconds(AnimationManager.instance.gruntWarpEnterAnim.length);

		_gameManager.allGruntz.ForEach(g => g.spriteRenderer.enabled = false);

		await UniTask.WaitForSeconds(1f);

		Level.instance.gameObject.SetActive(false);
		_gameManager.actorz.SetActive(false);
		_gameManager.itemz.SetActive(false);
		_gameManager.objectz.SetActive(false);

		_gameManager.GetComponent<Settings>().gameSettings.quickStartLevel = _gameManager.levelData;

		StatzMenu.instance.Activate();
	}
}
}
