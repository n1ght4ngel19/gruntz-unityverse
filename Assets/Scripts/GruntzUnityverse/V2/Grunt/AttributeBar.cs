using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.V2.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.V2.Grunt {
public class AttributeBar : MonoBehaviour {
	private SpriteRenderer _spriteRenderer;
	private List<Sprite> _frames;
	private int _value;

	public void SetHidden(bool hidden) {
		_spriteRenderer.enabled = !hidden;
	}

	public void Adjust(int newValue) {
		_spriteRenderer.enabled = newValue.Between(0, Statz.MaxValue);

		_value = newValue;
		_spriteRenderer.sprite = _frames[_value];
	}

	private void Awake() {
		Addressables.LoadAssetAsync<Sprite[]>($"{gameObject.name}.png").Completed += handle => {
			_frames = handle.Result.ToList();
		};

		_spriteRenderer = GetComponent<SpriteRenderer>();
	}
}
}
