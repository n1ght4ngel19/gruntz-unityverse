using System;
using System.Collections;
using Animancer;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Itemz.Toolz;
using GruntzUnityverse.MapObjectz.Interactablez;
using GruntzUnityverse.Pathfinding;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.MapObjectz.BaseClasses {
  /// <summary>
  /// The base for all other objects.
  /// </summary>
  public class MapObject : MonoBehaviour {
    public int objectId;

    #region Fields
    public SpriteRenderer spriteRenderer; // HideInInspector
    public Transform parent;
    public Vector2Int location;
    public Node ownNode;
    public Area area;
    public string abbreviatedArea;
    public bool isTargetable;
    protected Camera mainCamera;
    protected AnimancerComponent animancer; // HideInInspector
    private Animator _animator;
    private Sprite _unusedSprite;
    #endregion

    // ------------------------------------------------------------ //

    private void OnValidate() {
      // transform.hideFlags = HideFlags.HideInInspector;
      // gameObject.GetComponent<SpriteRenderer>().hideFlags = HideFlags.HideInInspector;
    }

    private void Awake() {
      _animator ??= gameObject.AddComponent<Animator>();
      animancer ??= gameObject.AddComponent<AnimancerComponent>();
      animancer.Animator = _animator;
    }

    protected virtual void Start() { }
    // ------------------------------------------------------------ //

    /// <summary>
    /// Sets up the MapObject for use and validates the setup wherever necessary.
    /// </summary>
    public virtual void Setup() {
      objectId = GameManager.Instance.currentLevelManager.mapObjectIdCounter++;
      spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
      mainCamera = GameObject.Find("ControlledCamera").GetComponent<Camera>();
      parent = transform.parent;
      location = Vector2Int.FloorToInt(transform.position);
      ownNode = GameManager.Instance.currentLevelManager.NodeAt(location);

      AssignAreaBySpriteName();

      // Todo: Only call these in classes that need them
      LoadAnimationz();
      // StartCoroutine(LoadAndPlayAnimation());

      Addressables.LoadAssetAsync<Sprite>("Assets/Spritez/Objectz/Unused.png").Completed += handle => {
        _unusedSprite = handle.Result;
      };
    }

    /// <summary>
    /// Checks if the MapObject is a valid target for a specific Grunt.
    /// </summary>
    /// <param name="grunt">The Grunt whose target is the MapObject.</param>
    /// <returns>True if the MapObject is a valid target for the Grunt, false otherwise.</returns>
    public bool IsValidTargetFor(Grunt grunt) {
      Debug.Log("Valid");

      return grunt.equipment.tool is Gauntletz && this is IBreakable
        || grunt.equipment.tool is Shovel && this is Hole;
    }

    /// <summary>
    /// Sets the enabled state of the MapObject and its SpriteRenderer.
    /// </summary>
    /// <param name="value">True or false, based on the intent.</param>
    public void SetEnabled(bool value) {
      enabled = value;
      spriteRenderer.enabled = value;
    }

    protected void WarnWithSpriteChange(string message) {
      Debug.LogWarning(message);

      spriteRenderer.sprite = _unusedSprite;
    }

    public void SetRendererEnabled(bool value) {
      spriteRenderer.enabled = value;
    }

    /// <summary>
    /// Loads all the AnimationClips necessary for animating the MapObject.
    /// </summary>
    protected virtual void LoadAnimationz() { }

    /// <summary>
    /// Loads the  AnimationClip necessary for animating the MapObject and plays it.
    /// </summary>
    protected virtual IEnumerator LoadAndPlayAnimation() {
      yield return null;
    }

    /// <summary>
    /// Checks if the MapObject has all its necessary components and fields set up correctly.
    /// </summary>
    protected virtual void ValidateSetup() { }

    public virtual IEnumerator BeUsed() {
      yield return null;
    }

    /// <summary>
    /// Transforms the Area enum value into its abbreviated string representation.
    /// </summary>
    /// <param name="inputArea">The enum value to transform.</param>
    /// <returns>The abbreviated form, or None if not applicable.</returns>
    /// <exception cref="ArgumentException">Throws an exception if an invalid enum value has been entered.</exception>
    private static string AbbreviateArea(Area inputArea) {
      return inputArea switch {
        Area.RockyRoadz => "RR",
        Area.Gruntziclez => "GR",
        Area.TroubleInTheTropicz => "TITT",
        Area.HighOnSweetz => "HOS",
        Area.HighRollerz => "HR",
        Area.HoneyIShrunkTheGruntz => "HISTG",
        Area.TheMiniatureMasterz => "TMM",
        Area.GruntzInSpace => "GIS",
        Area.None => "None",
        _ => throw new ArgumentException("The value of area should be one of the 9 areas."),
      };
    }

    /// <summary>
    /// Assigns the appropriate Area to the MapObject based on its SpriteRenderer's Sprite's name, if applicable.
    /// </summary>
    private void AssignAreaBySpriteName() {
      if (spriteRenderer.sprite is null) {
        return;
      }

      string spriteName = spriteRenderer.sprite.name;

      area = spriteName.StartsWith(AbbreviateArea(Area.RockyRoadz))
        ? Area.RockyRoadz
        : spriteName.StartsWith(AbbreviateArea(Area.Gruntziclez))
          ? Area.Gruntziclez
          : spriteName.StartsWith(AbbreviateArea(Area.TroubleInTheTropicz))
            ? Area.TroubleInTheTropicz
            : spriteName.StartsWith(AbbreviateArea(Area.HighOnSweetz))
              ? Area.HighOnSweetz
              : spriteName.StartsWith(AbbreviateArea(Area.HighRollerz))
                ? Area.HighRollerz
                : spriteName.StartsWith(AbbreviateArea(Area.HoneyIShrunkTheGruntz))
                  ? Area.HoneyIShrunkTheGruntz
                  : spriteName.StartsWith(AbbreviateArea(Area.TheMiniatureMasterz))
                    ? Area.TheMiniatureMasterz
                    : spriteName.StartsWith(AbbreviateArea(Area.GruntzInSpace))
                      ? Area.GruntzInSpace
                      : Area.None;

      abbreviatedArea = AbbreviateArea(area);
    }
  }
}
