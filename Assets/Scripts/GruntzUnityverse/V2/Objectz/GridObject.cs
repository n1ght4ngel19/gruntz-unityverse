using System;
using System.Linq;
using GruntzUnityverse.V2.Core;
using GruntzUnityverse.V2.Editor;
using GruntzUnityverse.V2.Pathfinding;
using UnityEngine;

namespace GruntzUnityverse.V2.Objectz {
  public abstract class GridObject : MonoBehaviour {
    // Flags for granular control over the object's node

    /// <summary>
    /// Represents whether this GridObject behaves like an obstacle.
    /// </summary>
    [Header("Flags")]
    public bool actAsObstacle;

    public bool actAsWater;
    public bool actAsFire;
    public bool actAsVoid;

    /// <summary>
    /// The location of this GridObject in 2D space.
    /// </summary>
    [Header("GridObject")]
    public Vector2Int location2D;

    /// <summary>
    /// The <see cref="NodeV2"/> that this GridObject currently occupies.
    /// </summary>
    [HideInNormalInspector]
    public NodeV2 node;

    /// <summary>
    /// The SpriteRenderer of this GridObject.
    /// </summary>
    [HideInNormalInspector]
    protected SpriteRenderer spriteRenderer;

    /// <summary>
    /// The collider used for checking interactions with this GridObject.
    /// </summary>
    [HideInNormalInspector]
    protected CircleCollider2D circleCollider2D;

    protected virtual void Awake() {
      location2D = Vector2Int.RoundToInt(transform.position);
      spriteRenderer = GetComponent<SpriteRenderer>();
      circleCollider2D = GetComponent<CircleCollider2D>();
    }

    protected virtual void Start() {
      node = LevelV2.Instance.levelNodes.First(n => n.location2D == location2D);
      node.isBlocked = actAsObstacle;
      node.isWater = actAsWater;
      node.isFire = actAsFire;
      node.isVoid = actAsVoid;
    }

    /// <summary>
    /// Checks whether this GridObject is diagonal to the given GridObject.
    /// </summary>
    /// <param name="other">The <see cref="GridObject"/> to check.</param>
    /// <returns>A boolean indicating whether this GridObject is diagonal to <see cref="other"/> or not.</returns>
    protected bool IsDiagonalTo(GridObject other) {
      return Math.Abs(other.location2D.x - location2D.x) != 0
        && Math.Abs(other.location2D.y - location2D.y) != 0;
    }

    /// <summary>
    /// Disables the ability of this GridObject to collide with other objects (with a <see cref="Rigidbody2D"/>).
    /// This is needed since OnTriggerExit2D can be called even after the object or its
    /// collider has been destroyed. If we disable the collider's ability to be triggered,
    /// we can check it before inside OnTriggerEnter2D and OnTriggerExit2D.
    /// </summary>
    public void DisableTrigger() {
      circleCollider2D.isTrigger = false;
    }

    protected bool SceneLoaded() {
      return gameObject.scene.isLoaded;
    }

    /// <summary>
    /// Reverts the effects this GridObject has on the NodeV2 it is occupying.
    /// Can be overridden to add additional effects specific to child classes.
    /// </summary>
    protected virtual void OnDestroy() {
      if (!SceneLoaded()) {
        return;
      }

      node.isBlocked = actAsObstacle ? false : node.isBlocked;
    }
  }
}
