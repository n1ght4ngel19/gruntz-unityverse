using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz.Data;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.Actorz.UI {
public class AttributeBar : MonoBehaviour {
	private SpriteRenderer _spriteRenderer;
	private List<Sprite> _frames;
	private int _value;

	public void SetHidden(bool hidden) {
		_spriteRenderer.enabled = !hidden;
	}

	public void Adjust(int newValue) {
		_spriteRenderer.sprite = _frames[newValue];
		_spriteRenderer.enabled = newValue != Statz.MAX_VALUE;

		_value = newValue;
	}

	private void Awake() {
		Addressables.LoadAssetAsync<Sprite[]>($"{gameObject.name}.png").Completed += handle => {
			_frames = handle.Result.ToList();
		};

		_spriteRenderer = GetComponent<SpriteRenderer>();
	}
}
}
