using System;
using System.Collections;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz {
  public class RedWarp : MapObject {
    public bool isEntrance;
    public MapObject target;
    private AnimationClip _openingAnim;
    private AnimationClip _swirlingAnim;
    // ------------------------------------------------------------ //

    protected override void Start() {
      base.Start();

      Addressables.LoadAssetAsync<AnimationClip>("RedWarp_Opening.anim").Completed += handle => {
        _openingAnim = handle.Result;
      };

      Addressables.LoadAssetAsync<AnimationClip>("RedWarp_Swirling.anim").Completed += handle => {
        _swirlingAnim = handle.Result;
      };

      spriteRenderer.enabled = false;
    }
    // ------------------------------------------------------------ //

    private void OnEnable() {
      StartCoroutine(Open());
    }

    private void Update() {
      animancer.Play(_swirlingAnim);

      if (!isEntrance) {
        enabled = false;
      }

      foreach (Grunt grunt in LevelManager.Instance.playerGruntz.Where(grunt => grunt.AtNode(ownNode))) {
        mainCamera.transform.position = new Vector3(target.location.x, target.location.y, mainCamera.transform.position.z);
        target.spriteRenderer.enabled = true;
        StatzManager.acquiredSecretz++;

        TeleportTo(target, grunt);

        enabled = false;
      }
    }
    // ------------------------------------------------------------ //

    private IEnumerator Open() {
      Addressables.LoadAssetAsync<AnimationClip>("RedWarp_Opening.anim").Completed += handle => {
        animancer.Play(handle.Result);
      };

      yield return new WaitForSeconds(2f);


    }

    private void TeleportTo(MapObject targetMapObject, Grunt grunt) {
      grunt.transform.position = new Vector3(targetMapObject.location.x, targetMapObject.location.y, grunt.transform.position.z);
      // Todo: Move these into separate method
      grunt.navigator.ownLocation = targetMapObject.location;
      grunt.navigator.ownNode = LevelManager.Instance.NodeAt(targetMapObject.location);
      grunt.navigator.targetLocation = targetMapObject.location;
      grunt.navigator.pathStart = null;
      grunt.navigator.pathEnd = null;
    }
  }
}
