using System.Collections.Generic;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class Fort : MapObject {
    [field: SerializeField] private FortTrigger FortTriggerPrefab { get; set; }
    private List<FortTrigger> _triggerz;
    private bool IsSetup { get; set; }


    protected override void Start() {
      base.Start();

      _triggerz = new List<FortTrigger>();

      Animancer.Play(Resources.Load<AnimationClip>("Animationz/MapObjectz/Fortz/Clipz/Fort_RockyRoadz"));
    }

    private void Update() {
      if (!IsSetup) {
        foreach (Node node in ownNode.Neighbours) {
          SetupTriggerAtNode(node);
        }
      }

      IsSetup = true;
    }

    private void SetupTriggerAtNode(Node node) {
      FortTrigger trigger = Instantiate(FortTriggerPrefab, node.transform);
      trigger.location = node.location;
      trigger.ownNode = LevelManager.Instance.NodeAt(trigger.location);
      trigger.ownFort = this;
      _triggerz.Add(trigger);
    }
  }
}
