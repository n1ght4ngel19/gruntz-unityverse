using System.Collections;
using Animancer;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.Objectz {
  /// <summary>
  /// The base for all Objects that can be interacted with on a Level.
  /// </summary>
  public class MapObject : MonoBehaviour {
    [field: SerializeField] public bool IsHardTurn { get; set; }
    public Vector2Int Location { get; set; }

    // [CanBeNull] public Node OwnNode { get; set; }
    public Node OwnNode { get; set; }
    protected SpriteRenderer Renderer { get; private set; }
    protected Transform OwnTransform { get; private set; }
    protected Camera MainCamera { get; private set; }
    protected Animator Animator { get; private set; }
    protected AnimancerComponent Animancer { get; private set; }


    protected virtual void Awake() {
      Location = Vector2Int.FloorToInt(transform.position);
      OwnTransform = gameObject.GetComponent<Transform>();
      Renderer = gameObject.GetComponent<SpriteRenderer>();
      MainCamera = Camera.main;
      Animator = gameObject.AddComponent<Animator>();
      Animancer = gameObject.AddComponent<AnimancerComponent>();
    }

    protected virtual void Start() {
      OwnNode = LevelManager.Instance.NodeAt(Location);
      LevelManager.Instance.NodeAt(Location).isHardTurn = IsHardTurn;
    }

    protected void SetEnabled(bool value) {
      enabled = value;
      Renderer.enabled = value;
    }

    public virtual IEnumerator BeUsed() {
      yield return null;
    }

    public virtual IEnumerator BeUsed(Grunt grunt) {
      yield return null;
    }
  }
}
