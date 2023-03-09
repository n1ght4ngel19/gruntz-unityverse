using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  public class SecretObject : MapObject {
    #region Fieldz

    [field: SerializeField] public Behaviour Behaviour { get; set; }
    [field: SerializeField] public List<MonoBehaviour> OtherBehaviours { get; set; }
    [field: SerializeField] public bool IsWalkable { get; set; }
    [field: SerializeField] public bool IsInitiallyBlocked { get; set; }
    [field: SerializeField] public float Delay { get; set; }
    [field: SerializeField] public float Duration { get; set; }

    #endregion


    protected override void Start() {
      base.Start();

      Behaviour = gameObject.GetComponent<Behaviour>();
      Renderer.enabled = false;
      Behaviour.enabled = false;

      OtherBehaviours = GetComponents<MonoBehaviour>()
        .ToList();

      foreach (MonoBehaviour behaviour in OtherBehaviours.Where(
        behaviour => behaviour.GetType() != typeof(SecretObject)
      )) {
        behaviour.enabled = false;
      }
    }

    public void ActivateSecret() {
      IsInitiallyBlocked = LevelManager.Instance.IsBlockedAt(OwnLocation);

      Renderer.enabled = true;
      Behaviour.enabled = true;

      foreach (MonoBehaviour behaviour in OtherBehaviours.Where(
        behaviour => behaviour.GetType() != typeof(SecretObject)
      )) {
        behaviour.enabled = true;
      }

      if (IsWalkable) {
        LevelManager.Instance.FreeNodeAt(OwnLocation);
      }
    }

    public void DeactivateSecret() {
      if (IsInitiallyBlocked) {
        LevelManager.Instance.BlockNodeAt(OwnLocation);
      } else {
        LevelManager.Instance.FreeNodeAt(OwnLocation);
      }

      Destroy(gameObject);
    }
  }
}
