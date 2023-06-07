using System.Collections.Generic;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class Fort : MapObject {
    [field: SerializeField] private FortTrigger FortTriggerPrefab { get; set; }
    private List<FortTrigger> Triggers { get; set; }
    private bool IsSetup { get; set; }


    protected override void Start() {
      base.Start();

      Triggers = new List<FortTrigger>();
      
      Animancer.Play(Resources.Load<AnimationClip>("Animationz/MapObjectz/Fortz/Clipz/Fort_RockyRoadz"));
    }

    private void Update() {
      if (!IsSetup) {
        foreach (Node node in OwnNode.Neighbours) {
          SetupTriggerAtNode(node);
        }
      }

      IsSetup = true;
    }

    private void SetupTriggerAtNode(Node node) {
      FortTrigger trigger = Instantiate(FortTriggerPrefab, transform);
      trigger.OwnLocation = node.OwnLocation;
      trigger.OwnNode = LevelManager.Instance.NodeAt(trigger.OwnLocation);
      trigger.Main = this;
      Triggers.Add(trigger);
    }
  }
}
