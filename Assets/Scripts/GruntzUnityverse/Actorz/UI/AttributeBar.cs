using System.Collections.Generic;
using GruntzUnityverse.Actorz.Data;
using UnityEngine;

namespace GruntzUnityverse.Actorz.UI {
public class AttributeBar : MonoBehaviour {
	public SpriteRenderer spriteRenderer;
	public List<Sprite> frames;

	public void SetHidden(bool hidden) {
		spriteRenderer.enabled = !hidden;
	}

	public void Adjust(int newValue) {
		spriteRenderer.sprite = frames[newValue];
		spriteRenderer.enabled = newValue != Statz.MAX_VALUE;
	}
}
}
