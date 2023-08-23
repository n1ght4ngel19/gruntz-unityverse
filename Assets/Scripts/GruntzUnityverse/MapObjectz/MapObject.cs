using System;
using System.Collections;
using Animancer;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.MapObjectz.Interactablez;
using GruntzUnityverse.MapObjectz.Itemz.Toolz;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz {
  /// <summary>
  /// The base for all other objects.
  /// </summary>
  public class MapObject : MonoBehaviour {
    public Area area;
    [HideInInspector] public string abbreviatedArea;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    public Vector2Int location;
    public Node ownNode;
    protected Transform ownTransform;
    protected Camera mainCamera;
    protected Transform parent;
    protected AnimancerComponent animancer;
    protected bool isValidated;
    private Animator _animator;
    // ------------------------------------------------------------ //

    protected virtual void Start() {
      location = Vector2Int.FloorToInt(transform.position);
      ownNode = LevelManager.Instance.NodeAt(location);
      spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
      ownTransform = gameObject.GetComponent<Transform>();
      parent = ownTransform.parent;
      mainCamera = Camera.main;
      _animator ??= gameObject.AddComponent<Animator>();
      animancer ??= gameObject.AddComponent<AnimancerComponent>();
      animancer.Animator = _animator;

      AssignAreaBySpriteName();
      LoadAnimationz();
      StartCoroutine(LoadAndPlayAnimation());
    }
    // ------------------------------------------------------------ //

    /// <summary>
    /// Checks if the MapObject is a valid target for a specific Grunt.
    /// </summary>
    /// <param name="grunt">The Grunt whose target is the MapObject.</param>
    /// <returns>True if the MapObject is a valid target for the Grunt, false otherwise.</returns>
    public bool IsValidTargetFor(Grunt grunt) {
      return grunt.equipment.tool is Gauntletz && this is IBreakable
        || grunt.equipment.tool is Shovel && this is Hole;
    }

    /// <summary>
    /// Sets the enabled state of the MapObject and its SpriteRenderer.
    /// </summary>
    /// <param name="value">True or false, based on the intent.</param>
    protected void SetEnabled(bool value) {
      enabled = value;
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

    public virtual IEnumerator BeUsed(Grunt grunt) {
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
