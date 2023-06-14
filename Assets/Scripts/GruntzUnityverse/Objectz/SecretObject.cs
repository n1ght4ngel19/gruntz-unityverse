using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  /// <summary>
  /// Representation of an Object (of any kind) that is connected to a SecretSwitch.
  /// </summary>
  public class SecretObject : MapObject {
    // [field: SerializeField] public Behaviour Behaviour { get; set; }
    [field: SerializeField] public List<MonoBehaviour> OtherBehaviours { get; set; }
    [field: SerializeField] public bool IsWalkable { get; set; }
    [field: SerializeField] public float Delay { get; set; }
    [field: SerializeField] public float Duration { get; set; }
    private bool IsInitiallyBlocked { get; set; }


    protected override void Start() {
      base.Start();

      SetEnabled(false);

      // Todo: Figure out what this wants to do (seems like nothing, but have to make sure)
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
      IsInitiallyBlocked = LevelManager.Instance.IsInaccessibleAt(Location);
      SetEnabled(true);

      foreach (MonoBehaviour behaviour in OtherBehaviours.Where(
        behaviour => behaviour.GetType() != typeof(SecretObject)
      )) {
        behaviour.enabled = true;
      }

      LevelManager.Instance.SetInaccessibleAt(Location, !IsWalkable);
    }

    /// <summary>
    /// Deactivates the SecretObject.
    /// </summary>
    public void DeactivateSecret() {
      LevelManager.Instance.SetInaccessibleAt(Location, IsInitiallyBlocked);

      Destroy(gameObject);
    }
  }
}
