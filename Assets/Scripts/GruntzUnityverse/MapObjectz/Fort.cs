using System.Collections.Generic;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Pathfinding;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz {
  public class Fort : MapObject {
    [SerializeField] private FortTrigger fortTriggerPrefab;
    private List<FortTrigger> _triggerz;
    private bool _isSetup;
    private AnimationClip _fortAnim;


    protected override void Start() {
      base.Start();

      _triggerz = new List<FortTrigger>();

      animancer.Play(_fortAnim);
    }

    private void Update() {
      if (!isValidated) {
        ValidateSetup();
      }
    }

    protected override void ValidateSetup() {
      foreach (Node node in ownNode.Neighbours) {
        SetupTriggerAtNode(node);
      }

      isValidated = true;
    }

    protected override void LoadAnimationz() {
      Addressables.LoadAssetAsync<AnimationClip>($"Fort_{area}.anim")
        .Completed += (handle) => {
        _fortAnim = handle.Result;
      };
    }

    private void SetupTriggerAtNode(Node node) {
      FortTrigger trigger = Instantiate(fortTriggerPrefab, node.transform);
      trigger.location = node.location;
      trigger.ownNode = LevelManager.Instance.NodeAt(trigger.location);
      trigger.fort = this;
      _triggerz.Add(trigger);
    }
  }
}
