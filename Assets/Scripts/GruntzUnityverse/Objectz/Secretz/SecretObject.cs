﻿using System.Collections;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Secretz {
public class SecretObject : MonoBehaviour {
	public float delay;
	public float duration;

	public IEnumerator ToggleOn() {
		yield return new WaitForSeconds(delay);

		gameObject.SetActive(true);

		yield return new WaitForSeconds(duration);

		gameObject.SetActive(false);
	}
}
}