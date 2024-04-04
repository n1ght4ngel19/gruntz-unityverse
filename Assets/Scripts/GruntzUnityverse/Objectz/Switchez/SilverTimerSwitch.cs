using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Core;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
[Serializable]
public struct PyramidData {
	public SilverPyramid pyramid;
	public float delay;
	public float duration;
}

public class SilverTimerSwitch : Switch {
	public List<PyramidData> pyramidData;

	protected override async void OnTriggerEnter2D(Collider2D other) {
		base.OnTriggerEnter2D(other);
		circleCollider2D.isTrigger = false;

		foreach (PyramidData pd in pyramidData) {
			pd.pyramid.delay = pd.delay;
			pd.pyramid.duration = pd.duration;
			pd.pyramid.Toggle();
		}

		float highestDuration = pyramidData.OrderBy(pd => pd.duration + pd.delay).Last().duration + pyramidData.OrderBy(pd => pd.duration + pd.delay).Last().delay;

		await UniTask.WaitForSeconds(highestDuration);

		await UniTask.WaitUntil(() => GameManager.instance.allGruntz.TrueForAll(gr => gr.node != node));

		Toggle();
		circleCollider2D.isTrigger = true;
	}

	protected override void OnTriggerExit2D(Collider2D other) { }
}
}
