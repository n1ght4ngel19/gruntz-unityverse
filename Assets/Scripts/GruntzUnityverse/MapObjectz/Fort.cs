using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.MapObjectz.BaseClasses;
using GruntzUnityverse.Pathfinding;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz {
  public class Fort : MapObject {
    [SerializeField] private FortTrigger fortTriggerPrefab;
    private List<FortTrigger> _triggerz;
    private bool _isSetup;
    private AnimationClip _fortAnim;
    // ------------------------------------------------------------ //

    protected override void Start() {
      base.Start();

      _triggerz = new List<FortTrigger>();
    }
    // ------------------------------------------------------------ //

    private void Update() {
      if (!isValidated) {
        ValidateSetup();
      }
    }
    // ------------------------------------------------------------ //

    /// <summary>
    /// Class-specific override of the base class' ValidateSetup() method.
    /// </summary>
    protected override void ValidateSetup() {
      foreach (Node node in ownNode.Neighbours) {
        SetupTriggerAtNode(node);
      }

      isValidated = true;
    }

    /// <summary>
    /// Class-specific override of the base class' LoadAndPlayAnimation() method.
    /// </summary>
    protected override IEnumerator LoadAndPlayAnimation() {
      yield return new WaitUntil(() => area != Area.None);

      Addressables.LoadAssetAsync<AnimationClip>($"Fort_{area}.anim")
        .Completed += handle => {
        _fortAnim = handle.Result;

        animancer.Play(_fortAnim);
      };
    }

    /// <summary>
    /// Sets up a FortTrigger at the given Node.
    /// </summary>
    /// <param name="node">The node to setup the trigger at.</param>
    private void SetupTriggerAtNode(Node node) {
      FortTrigger trigger = Instantiate(fortTriggerPrefab, node.transform);
      trigger.location = node.location;
      trigger.ownNode = GameManager.Instance.currentLevelManager.NodeAt(trigger.location);
      trigger.fort = this;
      _triggerz.Add(trigger);
    }
  }
}
