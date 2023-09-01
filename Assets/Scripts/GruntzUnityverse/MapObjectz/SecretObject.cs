
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Managerz;

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
    // ------------------------------------------------------------ //

    protected override void Start() {
      base.Start();


      SetEnabled(false);
      _otherComponents = new List<MapObject>();
      _otherComponents = gameObject.GetComponents<MapObject>().Where(mapObject => mapObject != this).ToList();
      _otherComponents.ForEach(mapObject => mapObject.enabled = false);
    }
    // ------------------------------------------------------------ //

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
      _otherComponents.ForEach(mapObject => mapObject.enabled = true);
      SetEnabled(true);

      LevelManager.Instance.SetBlockedAt(location, isBlocked);
      // Todo: Other Node flags
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
