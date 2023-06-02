using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  /// <summary>
  /// Representation of an Object (of any kind) that is connected to a SecretSwitch.
  /// </summary>
  public class SecretObject : MapObject {
    #region Fieldz

    // [field: SerializeField] public Behaviour Behaviour { get; set; }
    [field: SerializeField] public List<MonoBehaviour> OtherBehaviours { get; set; }
    [field: SerializeField] public bool IsWalkable { get; set; }
    [field: SerializeField] public bool IsInitiallyBlocked { get; set; }
    [field: SerializeField] public float Delay { get; set; }
    [field: SerializeField] public float Duration { get; set; }

    #endregion


    protected override void Start() {
      base.Start();

      SetEnabled(false);
      // gameObject.SetActive(false);
      // Behaviour = gameObject.GetComponent<Behaviour>();
      // Renderer.enabled = false;
      // Behaviour.enabled = false;

      // Todo: Figure out what this wants to do
      OtherBehaviours = GetComponents<MonoBehaviour>().ToList();

      foreach (MonoBehaviour behaviour in OtherBehaviours.Where(
        behaviour => behaviour.GetType() != typeof(SecretObject)
      )) {
        behaviour.enabled = false;
      }
    }

    /// <summary>
    /// Activates the SecretObject.
    /// </summary>
    public void ActivateSecret() {
      IsInitiallyBlocked = LevelManager.Instance.IsBlockedAt(OwnLocation);
      SetEnabled(true);
      // gameObject.SetActive(true);
      // Renderer.enabled = true;
      // Behaviour.enabled = true;

      foreach (MonoBehaviour behaviour in OtherBehaviours.Where(
        behaviour => behaviour.GetType() != typeof(SecretObject)
      )) {
        behaviour.enabled = true;
      }

      // if (IsWalkable) {
      LevelManager.Instance.SetBlockedAt(OwnLocation, !IsWalkable);
      // }
    }

    /// <summary>
    /// Deactivates the SecretObject.
    /// </summary>
    public void DeactivateSecret() {
      LevelManager.Instance.SetBlockedAt(OwnLocation, IsInitiallyBlocked);

      Destroy(gameObject);
    }
  }
}
