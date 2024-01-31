using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.V2.Grunt {
  public class AttributeBar : MonoBehaviour {
    private SpriteRenderer _spriteRenderer;
    private List<Sprite> _frames;
    private int _value;

    public void SetValue(int newValue) {
      _value = newValue;
      _spriteRenderer.sprite = _frames[_value];
    }

    public void SetHidden(bool hidden) {
      _spriteRenderer.enabled = !hidden;
    }

    private void Awake() {
      Addressables.LoadAssetAsync<Sprite[]>($"{gameObject.name}.png").Completed += handle => {
        _frames = handle.Result.ToList();
      };
    }
  }
}
