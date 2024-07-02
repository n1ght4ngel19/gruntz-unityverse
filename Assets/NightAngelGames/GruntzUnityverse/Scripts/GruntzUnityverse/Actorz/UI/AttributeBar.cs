using System.Collections.Generic;
using GruntzUnityverse.Actorz.Data;
using UnityEngine;

namespace GruntzUnityverse.Actorz.UI {
public class AttributeBar : MonoBehaviour {
	public SpriteRenderer spriteRenderer;
	public List<Sprite> frames;

	private void Awake() {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void SetHidden(bool hidden) {
		spriteRenderer.enabled = !hidden;
	}

	public void Adjust(float newValue) {
		spriteRenderer.sprite = frames[Mathf.RoundToInt(newValue)];
		spriteRenderer.enabled = newValue != Statz.MAX_VALUE;
	}
}
}
