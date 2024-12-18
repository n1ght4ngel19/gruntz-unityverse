﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GruntzUnityverse.Objectz.Pyramidz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Switchez {
public class SilverTimerSwitch : Switch {
	public List<PyramidData> pyramidData;

	protected override async void OnTriggerEnter2D(Collider2D other) {
		base.OnTriggerEnter2D(other);

		Deactivate();

		foreach (PyramidData pd in pyramidData) {
			pd.pyramid.delay = pd.delay;
			pd.pyramid.duration = pd.duration;

			pd.pyramid.Toggle();
		}

		float highestDuration =
			pyramidData.OrderBy(pd => pd.duration + pd.delay).Last().duration
			+ pyramidData.OrderBy(pd => pd.duration + pd.delay).Last().delay;

		await UniTask.WaitForSeconds(highestDuration);

		await UniTask.WaitUntil(() => gameManager.gruntz.TrueForAll(gr => gr.node != node));

		Toggle();
		Activate();
	}
}

[Serializable]
public struct PyramidData {
	public SilverPyramid pyramid;

	public float delay;

	public float duration;
}
}
