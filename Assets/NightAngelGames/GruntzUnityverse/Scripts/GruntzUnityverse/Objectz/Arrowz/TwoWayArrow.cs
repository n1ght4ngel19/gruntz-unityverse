using GruntzUnityverse.Utils.Extensionz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Arrowz {
public class TwoWayArrow : Arrow {
	public Sprite state1Sprite;
	public Sprite state2Sprite;

	public void Toggle() {
		spriteRenderer.sprite = spriteRenderer.sprite == state1Sprite
			? state2Sprite
			: state1Sprite;

		direction = direction.Opposite();
		SetTargetNode(direction);
	}
}
}
