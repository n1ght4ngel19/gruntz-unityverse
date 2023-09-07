using System.Collections.Generic;
using System.Linq;

namespace GruntzUnityverse.MapObjectz {
  /// <summary>
  /// A generic MapObject that can be set up manually to replicate the functionalities of any other MapObject.
  /// This is needed since 
  /// </summary>
  public class SecretObject : MapObject {
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
    private List<MapObject> _otherComponents;
    private bool _isInitialized;
    // ------------------------------------------------------------ //

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
      _isInitiallyBlocked = GameManager.Instance.currentLevelManager.IsBlockedAt(location);
      _isInitiallyBurn = GameManager.Instance.currentLevelManager.IsBurnAt(location);
      _isInitiallyDeath = GameManager.Instance.currentLevelManager.IsDeathAt(location);
      _isInitiallyEdge = GameManager.Instance.currentLevelManager.IsEdgeAt(location);
      _isInitiallyHardTurn = GameManager.Instance.currentLevelManager.IsHardTurnAt(location);
      _isInitiallyVoid = GameManager.Instance.currentLevelManager.IsVoidAt(location);
      _isInitiallyWater = GameManager.Instance.currentLevelManager.IsWaterAt(location);
      _otherComponents.ForEach(mapObject => mapObject.enabled = true);
      SetEnabled(true);

      GameManager.Instance.currentLevelManager.SetBlockedAt(location, isBlocked);
      // Todo: Other Node flags
    }

    /// <summary>
    /// Deactivates the SecretObject.
    /// </summary>
    public void DeactivateSecret() {
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
