using System;
using System.Collections;
using Animancer;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  /// <summary>
  /// The base for all Objects that can be interacted with on a Level.
  /// </summary>
  public class MapObject : MonoBehaviour {
    public Area area;
    public Vector2Int location;

    public Node OwnNode { get; set; }
    public AnimancerComponent animancer;
    protected Animator Animator;
    protected SpriteRenderer SpriteRenderer;
    protected Transform OwnTransform;
    protected Camera MainCamera;


    protected virtual void Start() {
      location = Vector2Int.FloorToInt(transform.position);
      OwnTransform = gameObject.GetComponent<Transform>();
      SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
      MainCamera = Camera.main;
      Animator = gameObject.AddComponent<Animator>();
      animancer = gameObject.AddComponent<AnimancerComponent>();
      animancer.Animator = Animator;
      OwnNode = LevelManager.Instance.NodeAt(location);
    }

    protected void SetEnabled(bool value) {
      enabled = value;
      SpriteRenderer.enabled = value;
    }

    protected void AssignAreaBySpriteName() {
      string spriteName = SpriteRenderer.sprite.name;

      if (spriteName.StartsWith("RR_")) {
        area = Area.RockyRoadz;
      } else if (spriteName.StartsWith("GR_")) {
        area = Area.Gruntziclez;
      } else if (spriteName.StartsWith("TITT_")) {
        area = Area.TroubleInTheTropicz;
      } else if (spriteName.StartsWith("HOS_")) {
        area = Area.HighOnSweetz;
      } else if (spriteName.StartsWith("HR_")) {
        area = Area.HighRollerz;
      } else if (spriteName.StartsWith("HISTG_")) {
        area = Area.HoneyIShrunkTheGruntz;
      } else if (spriteName.StartsWith("TMM_")) {
        area = Area.TheMiniatureMasterz;
      } else if (spriteName.StartsWith("GIS_")) {
        area = Area.GruntzInSpace;
      } else {
        throw new ArgumentException("The value of area should be one of the 9 areas.");
      }
    }

    public virtual IEnumerator BeUsed() {
      yield return null;
    }

    public virtual IEnumerator BeUsed(Grunt grunt) {
      yield return null;
    }
  }
}
