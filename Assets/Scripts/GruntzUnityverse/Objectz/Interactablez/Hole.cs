using System.Collections;
using GruntzUnityverse.Actorz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Interactablez {
  public class Hole : MapObject {
    [field: SerializeField] public Sprite OpenSprite { get; set; }
    [field: SerializeField] public Sprite FilledSprite { get; set; }
    [field: SerializeField] public bool IsOpen { get; set; }

    public override IEnumerator BeUsed(Grunt grunt) {
      // Todo: Why does this not work right?
      // animancer.Play(Resources.Load<AnimationClip>("Animationz/MapObjectz/Rockz/Clipz/RockBreak_RockyRoadz"));

      yield return new WaitForSeconds(1f);

      SwitchOpen();

      grunt.TargetObject = null;
    }

    private void SwitchOpen() {
      IsOpen = !IsOpen;
      Renderer.sprite = IsOpen ? OpenSprite : FilledSprite;
    }
  }
}
