using System.Collections;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.MapObjectz.BaseClasses;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz {
  public class RedWarp : MapObject {
    public bool isEntrance;
    public MapObject target;
    private AnimationClip _openingAnim;
    private AnimationClip _swirlingAnim;

    private void OnEnable() {
      StartCoroutine(Open());
    }

    private void Update() {
      if (!isEntrance || !IsGruntOnTop()) {
        return;
      }

      SetEnabled(false);
      mainCamera.transform.position = new Vector3(target.location.x, target.location.y, mainCamera.transform.position.z);

      target.SetEnabled(true);
      TeleportTo(target, GetGruntOnTop());

      // Addressables.LoadAssetAsync<AudioClip>("Assets/Audio/Soundz/Sound_RedWarp.wav").Completed += handle => {
      //   GameManager.Instance.audioSource.PlayOneShot(handle.Result);
      // };
    }

    protected override void LoadAnimationz() {
      Addressables.LoadAssetAsync<AnimationClip>("RedWarp_Opening.anim").Completed += handle => {
        _openingAnim = handle.Result;
      };

      Addressables.LoadAssetAsync<AnimationClip>("RedWarp_Swirling.anim").Completed += handle => {
        _swirlingAnim = handle.Result;
      };
    }

    private IEnumerator Open() {
      animancer.Play(_openingAnim);

      yield return new WaitForSeconds(1f);

      animancer.Play(_swirlingAnim);
    }

    private void TeleportTo(MapObject targetMapObject, Grunt grunt) {
      grunt.transform.position = new Vector3(targetMapObject.location.x, targetMapObject.location.y, grunt.transform.position.z);
      // Todo: ??? Move these into separate method
      grunt.navigator.ownLocation = targetMapObject.location;
      grunt.navigator.ownNode = GameManager.Instance.currentLevelManager.NodeAt(targetMapObject.location);
      grunt.navigator.targetLocation = targetMapObject.location;
      grunt.navigator.pathStart = null;
      grunt.navigator.pathEnd = null;
    }
  }
}
