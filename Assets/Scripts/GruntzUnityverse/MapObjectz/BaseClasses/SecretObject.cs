using System.Collections.Generic;
using System.Linq;

namespace GruntzUnityverse.MapObjectz.BaseClasses {
  /// <summary>
  /// A generic MapObject that can be set up manually to replicate the functionalities of any other MapObject.
  /// This is needed since 
  /// </summary>
  public class SecretObject : MapObject {
    public float delay;
    public float duration;

    public bool isBlocked;
    public bool isBurn;
    public bool isDeath;
    public bool isEdge;
    public bool isHardTurn;
    public bool isVoid;
    public bool isWater;

    private bool _isInitiallyBlocked;
    private bool _isInitiallyBurn;
    private bool _isInitiallyDeath;
    private bool _isInitiallyEdge;
    private bool _isInitiallyHardTurn;
    private bool _isInitiallyVoid;
    private bool _isInitiallyWater;
    private List<MapObject> _otherComponents;
    private bool _isInitialized;

    private List<MapObject> _siblings;

    private void Update() {
      if (!_isInitialized) {
        _isInitialized = true;

        SetEnabled(false);
        _otherComponents = new List<MapObject>();
        _otherComponents = gameObject.GetComponents<MapObject>().Where(mapObject => mapObject != this).ToList();
        _otherComponents.ForEach(mapObject => mapObject.enabled = false);
      }
    }

    /// <summary>
    /// Activates the SecretObject.
    /// </summary>
    public void ActivateSecret() {
      // Deactivate any other MapObjects that are at the same location.
      _siblings = GameManager.Instance.currentLevelManager.mapObjectz.Where(mo => mo.location == location && mo != this).ToList();
      _siblings.ForEach(sibling => sibling.SetEnabled(false));

      _isInitiallyBlocked = ownNode.isBlocked;
      _isInitiallyBurn = ownNode.isBurn;
      _isInitiallyDeath = ownNode.isDeath;
      _isInitiallyEdge = ownNode.isEdge;
      _isInitiallyHardTurn = ownNode.isHardTurn;
      _isInitiallyVoid = ownNode.isVoid;
      _isInitiallyWater = ownNode.isWater;

      // Enable Components other than the SecretObject (if there are any).
      _otherComponents.ForEach(mapObject => mapObject.SetEnabled(true));

      SetEnabled(true);

      ownNode.isBlocked = isBlocked;
      ownNode.isBurn = isBurn;
      ownNode.isDeath = isDeath;
      ownNode.isEdge = isEdge;
      ownNode.isHardTurn = isHardTurn;
      ownNode.isVoid = isVoid;
      ownNode.isWater = isWater;
    }

    /// <summary>
    /// Deactivates the SecretObject.
    /// </summary>
    public void DeactivateSecret() {
      _siblings.ForEach(sibling => sibling.SetEnabled(true));

      GameManager.Instance.currentLevelManager.SetBlockedAt(location, _isInitiallyBlocked);
      GameManager.Instance.currentLevelManager.SetBurnAt(location, _isInitiallyBurn);
      GameManager.Instance.currentLevelManager.SetDeathAt(location, _isInitiallyDeath);
      GameManager.Instance.currentLevelManager.SetEdgeAt(location, _isInitiallyEdge);
      GameManager.Instance.currentLevelManager.SetHardTurnAt(location, _isInitiallyHardTurn);
      GameManager.Instance.currentLevelManager.SetVoidAt(location, _isInitiallyVoid);
      GameManager.Instance.currentLevelManager.SetWaterAt(location, _isInitiallyWater);

      Destroy(gameObject);
    }
  }
}
