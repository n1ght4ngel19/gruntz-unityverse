﻿using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Misc;
using GruntzUnityverse.Objectz.Pyramidz;
using GruntzUnityverse.Objectz.Switchez;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
public class Checkpoint : MonoBehaviour {
	public List<CheckpointSwitch> switchez;
	public List<CheckpointPyramid> pyramidz;
	public List<CheckpointFlag> flagz;

	private void Update() {
		if (switchez.TrueForAll(sw => sw.IsPressed)) {
			Debug.Log("Checkpoint cleared!");

			switchez.ForEach(sw => sw.DisableTrigger());
			pyramidz.ForEach(pyramid => pyramid.Toggle());
			flagz.ForEach(flag => flag.PlayAnim());

			enabled = false;
		}
	}

	public void Setup() {
		switchez = GetComponentsInChildren<CheckpointSwitch>().ToList();

		if (switchez == null || switchez.Count == 0) {
			Debug.LogWarning($"No Switchez found for checkpoint {gameObject.name}!");
			enabled = false;
		}

		pyramidz = GetComponentsInChildren<CheckpointPyramid>().ToList();

		if (pyramidz == null || pyramidz.Count == 0) {
			Debug.LogWarning($"No Pyramidz found for checkpoint {gameObject.name}!");
			enabled = false;
		}

		flagz = GetComponentsInChildren<CheckpointFlag>().ToList();
	}
}
}