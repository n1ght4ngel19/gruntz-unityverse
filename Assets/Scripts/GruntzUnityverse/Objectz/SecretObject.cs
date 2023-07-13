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
    public List<MonoBehaviour> otherBehaviours;
    public bool isBlocked;
    public bool isBurn;
    public bool isDeath;
    public bool isEdge;
    public bool isHardTurn;
    public bool isVoid;
    public bool isWater;
    public float delay;
    public float duration;
    private bool _isInitiallyBlocked;
    private bool _isInitiallyBurn;
    private bool _isInitiallyDeath;
    private bool _isInitiallyEdge;
    private bool _isInitiallyHardTurn;
    private bool _isInitiallyVoid;
    private bool _isInitiallyWater;


    protected override void Start() {
      base.Start();

      SetEnabled(false);

      // Todo: Figure out what this wants to do (seems like nothing, but have to make sure)
      otherBehaviours = GetComponents<MonoBehaviour>().ToList();

      foreach (MonoBehaviour behaviour in otherBehaviours.Where(
        behaviour => behaviour.GetType() != typeof(SecretObject)
      )) {
        behaviour.enabled = false;
      }
    }

    /// <summary>
    /// Activates the SecretObject.
    /// </summary>
    public void ActivateSecret() {
      _isInitiallyBlocked = LevelManager.Instance.IsBlockedAt(location);
      _isInitiallyBurn = LevelManager.Instance.IsBurnAt(location);
      _isInitiallyDeath = LevelManager.Instance.IsDeathAt(location);
      _isInitiallyEdge = LevelManager.Instance.IsEdgeAt(location);
      _isInitiallyHardTurn = LevelManager.Instance.IsHardTurnAt(location);
      _isInitiallyVoid = LevelManager.Instance.IsVoidAt(location);
      _isInitiallyWater = LevelManager.Instance.IsWaterAt(location);
      SetEnabled(true);

      foreach (MonoBehaviour behaviour in otherBehaviours.Where(
        behaviour => behaviour.GetType() != typeof(SecretObject)
      )) {
        behaviour.enabled = true;
      }

      LevelManager.Instance.SetBlockedAt(location, isBlocked);
    }

    /// <summary>
    /// Deactivates the SecretObject.
    /// </summary>
    public void DeactivateSecret() {
      LevelManager.Instance.SetBlockedAt(location, _isInitiallyBlocked);
      LevelManager.Instance.SetBurnAt(location, _isInitiallyBurn);
      LevelManager.Instance.SetDeathAt(location, _isInitiallyDeath);
      LevelManager.Instance.SetEdgeAt(location, _isInitiallyEdge);
      LevelManager.Instance.SetHardTurnAt(location, _isInitiallyHardTurn);
      LevelManager.Instance.SetVoidAt(location, _isInitiallyVoid);
      LevelManager.Instance.SetWaterAt(location, _isInitiallyWater);

      Destroy(gameObject);
    }
  }
}
