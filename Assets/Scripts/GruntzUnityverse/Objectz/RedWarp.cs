using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class RedWarp : MapObject {
    public bool isEntrance;
    public MapObject target;


    protected override void Start() {
      base.Start();

      spriteRenderer.enabled = false;
    }

    private void Update() {
      if (!isEntrance) {
        enabled = false;
      }

      foreach (Grunt grunt in LevelManager.Instance.playerGruntz.Where(grunt => grunt.AtNode(ownNode))) {
        MainCamera.transform.position = new Vector3(target.location.x, target.location.y, MainCamera.transform.position.z);
        target.spriteRenderer.enabled = true;

        TeleportTo(target, grunt);

        enabled = false;
      }
    }

    private void TeleportTo(MapObject target, Grunt grunt) {
      grunt.transform.position = new Vector3(target.location.x, target.location.y, grunt.transform.position.z);
      // Todo: Move these into separate method
      grunt.navigator.ownLocation = target.location;
      grunt.navigator.ownNode = LevelManager.Instance.NodeAt(target.location);
      grunt.navigator.targetLocation = target.location;
      grunt.navigator.pathStart = null;
      grunt.navigator.pathEnd = null;
    }
  }
}
