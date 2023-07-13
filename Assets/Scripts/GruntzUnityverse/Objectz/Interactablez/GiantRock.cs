using System.Collections;
using System.Collections.Generic;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Interactablez {
  public class GiantRock : MapObject, IBreakable {
    public AnimationClip BreakAnimation { get; set; }
    public GiantRockEdge giantRockEdge;
    public List<GiantRockEdge> edges;

    protected override void Start() {
      base.Start();

      AssignAreaBySpriteName();

      BreakAnimation = Resources.Load<AnimationClip>($"Animationz/MapObjectz/Rockz/Clipz/RockBreak_{area}_01");

      foreach (Node node in OwnNode.Neighbours) {
        GiantRockEdge edge = Instantiate(giantRockEdge, node.transform.position, Quaternion.identity);
        edge.mainRock = this;
        edge.BreakAnimation = BreakAnimation;
        edge.transform.parent = transform;
        edge.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
        edges.Add(edge);
      }
    }

    public IEnumerator Break() {
      // 1.5s is the delay after the beginning of the GauntletzGrunt's Rock breaking animation (when the Rock actually should break)
      yield return new WaitForSeconds(1.5f);

      Debug.Log("1.5f delay passed");

      foreach (GiantRockEdge edge in edges) {
        LevelManager.Instance.SetBlockedAt(edge.location, false);
        LevelManager.Instance.SetHardTurnAt(edge.location, false);
        edge.animancer.Play(BreakAnimation);

        Destroy(edge.gameObject);
      }

      animancer.Play(BreakAnimation);

      yield return new WaitForSeconds(1f);

      LevelManager.Instance.SetBlockedAt(location, false);
      LevelManager.Instance.SetHardTurnAt(location, false);

      Destroy(gameObject, 1f);
    }
  }
}
