using System.Collections;
using System.Linq;
using GruntzUnityverse.MapObjectz.BaseClasses;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.Interactablez {
  public class Hole : MapObject {
    public Sprite openSprite;
    public Sprite filledSprite;
    public bool isOpen;
    public MapItem hiddenItem;
    private AnimationClip _dirtFlyingAnim;
    private bool _isInitialized;

    protected override void Start() {
      base.Start();

      isTargetable = true;
      ownNode.isHole = isOpen;

      Addressables.LoadAssetAsync<Sprite>($"{abbreviatedArea}_Hole_Filled.png")
        .Completed += handle => {
        filledSprite = handle.Result;
      };

      Addressables.LoadAssetAsync<Sprite>($"{abbreviatedArea}_Hole_Open.png")
        .Completed += handle => {
        openSprite = handle.Result;
      };

      Addressables.LoadAssetAsync<AnimationClip>($"Effect_{area}_Dirt_01.anim").Completed += handle => {
        _dirtFlyingAnim = handle.Result;
      };
    }

    private void Update() {
      if (!_isInitialized) {
        _isInitialized = true;

        hiddenItem = FindObjectsOfType<MapItem>()
          .FirstOrDefault(item =>
            item.ownNode == ownNode);

        hiddenItem?.SetRendererEnabled(false);
      }
    }

    public override IEnumerator BeUsed() {
      // yield return new WaitForSeconds(0.5f);

      Debug.Log("Being used");
      animancer.Play(_dirtFlyingAnim);
      yield return new WaitForSeconds(_dirtFlyingAnim.length + 0.5f);

      isOpen = !isOpen;
      ownNode.isHole = isOpen;
      spriteRenderer.sprite = isOpen ? openSprite : filledSprite;
    }
  }
}
