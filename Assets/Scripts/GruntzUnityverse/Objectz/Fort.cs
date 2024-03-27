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

	private void Start() {
		fortFlagz = transform.parent.GetComponentsInChildren<Flag>().ToList();

		fortFlagz.ForEach(fl => fl.PlayAnim());
	}

	private async void OnTriggerEnter2D(Collider2D other) {
		if (!other.TryGetComponent(out Grunt grunt)) {
			return;
		}

		if (grunt.equippedTool is Warpstone) {
			GameManager.instance.allGruntz.ForEach(g => g.enabled = false);

			GameManager.instance.playerGruntz.ForEach(pg => pg.animancer.Play(AnimationManager.instance.gruntWarpOutAnimationz.First()));
			await UniTask.WaitForSeconds(AnimationManager.instance.gruntWarpOutAnimationz.First().length);

			GameManager.instance.playerGruntz.ForEach(pg => pg.animancer.Play(AnimationManager.instance.gruntWarpOutEndAnimation));
			await UniTask.WaitForSeconds(AnimationManager.instance.gruntWarpOutEndAnimation.length);

			GameManager.instance.allGruntz.ForEach(g => g.spriteRenderer.enabled = false);
			
			await UniTask.WaitForSeconds(1f);

			Level.instance.gameObject.SetActive(false);
			GameManager.instance.actorz.SetActive(false);
			GameManager.instance.itemz.SetActive(false);
			GameManager.instance.objectz.SetActive(false);

			StatzMenu.Instance.Activate();
		}
	}
}
}
