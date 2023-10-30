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
    public MapObject hiddenItem;
    private AnimationClip _dirtFlyingAnim;
    private bool _isInitialized;

    public override void Setup() {
      base.Setup();

      isTargetable = true;
      ownNode.isHole = isOpen;
    }

    protected override void LoadAnimationz() {
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

    // Todo: Disable Update
    private void Update() {
      if (!_isInitialized) {
        _isInitialized = true;

        hiddenItem = FindObjectsOfType<MapObject>()
          .FirstOrDefault(item =>
            item.ownNode == ownNode && item != this);

        hiddenItem?.SetRendererEnabled(false);
      }
    }

    public override IEnumerator BeUsed() {
      // Todo: Play on dummy object
      // animancer.Play(_dirtFlyingAnim);

      // Todo: Play appropriate sound
      Addressables.LoadAssetAsync<AudioClip>($"{abbreviatedArea}_RockBreak.wav").Completed += handle => {
        GameManager.Instance.audioSource.PlayOneShot(handle.Result);
      };

      yield return new WaitForSeconds(2.5f);

      // Todo: Destroy dummy object

      isOpen = !isOpen;
      ownNode.isHole = isOpen;
      hiddenItem?.SetRendererEnabled(true);

      spriteRenderer.sprite =
        spriteRenderer.sprite == openSprite
          ? filledSprite
          : openSprite;
    }
  }
}
