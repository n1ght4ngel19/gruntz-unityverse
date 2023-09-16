using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.MapObjectz.BaseClasses;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.Interactablez {
  public class Hole : MapObject {
    public Sprite openSprite;
    public Sprite filledSprite;
    public bool isOpen;

    protected override void Start() {
      base.Start();

      openSprite = spriteRenderer.sprite;
      
      Addressables.LoadAssetAsync<Sprite>($"{abbreviatedArea}_Hole_Filled.png")
        .Completed += handle => {
          filledSprite = handle.Result;
      };
    }

    public override IEnumerator BeUsed(Grunt grunt) {
      yield return new WaitForSeconds(1f);

      SwitchOpen();

      grunt.targetObject = null;
    }

    private void SwitchOpen() {
      isOpen = !isOpen;
      spriteRenderer.sprite = isOpen ? openSprite : filledSprite;
    }
  }
}
