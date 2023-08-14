using System.Collections;
using System.Collections.Generic;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Pathfinding;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.Objectz.Interactablez {
  public class GiantRock : MapObject, IBreakable {
    public AnimationClip BreakAnimation { get; set; }
    public GiantRockEdge giantRockEdge;
    public List<GiantRockEdge> edgez;


    protected override void Start() {
      base.Start();

      foreach (Node node in ownNode.Neighbours) {
        GiantRockEdge edge = Instantiate(giantRockEdge, node.transform.position, Quaternion.identity);
        edge.mainRock = this;
        edge.BreakAnimation = BreakAnimation;
        edge.transform.parent = transform;
        edge.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
        edgez.Add(edge);
      }
    }

    public IEnumerator Break() {
      // 1.5s is the delay after the beginning of the GauntletzGrunt's Rock breaking animation (when the Rock actually should break)
      yield return new WaitForSeconds(1.5f);

      Debug.Log("1.5f delay passed");

      foreach (GiantRockEdge edge in edgez) {
        LevelManager.Instance.SetBlockedAt(edge.location, false);
        LevelManager.Instance.SetHardTurnAt(edge.location, false);

        Destroy(edge.gameObject);
      }

      animancer.Play(BreakAnimation);

      yield return new WaitForSeconds(1f);

      LevelManager.Instance.SetBlockedAt(location, false);
      LevelManager.Instance.SetHardTurnAt(location, false);

      Destroy(gameObject, 1f);
    }

    protected override void LoadAnimationz() {
      Addressables.LoadAssetAsync<AnimationClip>($"RockBreak_{abbreviatedArea}_01.anim").Completed += (handle) => {
        BreakAnimation = handle.Result;
      };
    }
  }
}
