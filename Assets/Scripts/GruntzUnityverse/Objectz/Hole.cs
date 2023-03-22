using System.Collections;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Objectz.Itemz.Toolz;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class Hole : MapObject {
    [field: SerializeField] public Sprite OpenSprite { get; set; }
    [field: SerializeField] public Sprite FilledSprite { get; set; }
    [field: SerializeField] public bool IsOpen { get; set; }


    private void Update() {
      // if (!IsOpen) {
      //   return;
      // }

      foreach (Grunt grunt in LevelManager.Instance.PlayerGruntz.Where(
        grunt => grunt.IsOnLocation(OwnLocation) && grunt.enabled && IsOpen
      )) {
        StartCoroutine(grunt.Die(DeathType.FallInHole));

        break;
      }

      // If a Grunt that is beside the Hole and has a Shovel equipped and is targeting the Hole, dig the Hole
      foreach (Grunt grunt in LevelManager.Instance.AllGruntz.Where(
        grunt => grunt.NavComponent.OwnNode.Neighbours.Contains(OwnNode)
          && grunt.HasTool(ToolType.Shovel)
          && grunt.TargetObject.Equals(this)
      )) {
        StartCoroutine(((Shovel)grunt.Equipment.Tool).DigHole(grunt));
      }
    }

    /// <summary>
    /// Toggles whether the Hole is open.
    /// </summary>
    /// <returns>An <see cref="IEnumerator"/> since this is a <see cref="Coroutine"/></returns>
    public IEnumerator Dig() {
      // Animator.Play("DirtFlying");
      yield return new WaitForSeconds(0.5f);

      IsOpen = !IsOpen;
      Renderer.sprite = IsOpen ? OpenSprite : FilledSprite;
    }
  }
}
