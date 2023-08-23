using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz {
  public class RedWarp : MapObject {
    public bool isEntrance;
    public MapObject target;
    // ------------------------------------------------------------ //

    protected override void Start() {
      base.Start();

      spriteRenderer.enabled = false;
    }
    // ------------------------------------------------------------ //

    private void Update() {
      if (!isEntrance) {
        enabled = false;
      }

      foreach (Grunt grunt in LevelManager.Instance.playerGruntz.Where(grunt => grunt.AtNode(ownNode))) {
        mainCamera.transform.position = new Vector3(target.location.x, target.location.y, mainCamera.transform.position.z);
        target.spriteRenderer.enabled = true;

        TeleportTo(target, grunt);

        enabled = false;
      }
    }
    // ------------------------------------------------------------ //

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
