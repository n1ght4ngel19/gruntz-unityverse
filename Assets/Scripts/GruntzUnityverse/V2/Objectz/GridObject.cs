using System;
using System.Security.Cryptography;
using UnityEngine;

namespace GruntzUnityverse.V2.Objectz {
  public class GridObject : MonoBehaviour {
    /// <summary>
    /// The location of this GridObject in 2D space.
    /// </summary>
    [Header("GridObject")]
    public Vector2Int location2D;

    /// <summary>
    /// Returns whether this GridObject is an obstacle.
    /// </summary>
    public bool isBlocking;

    /// <summary>
    /// The SpriteRenderer of this GridObject.
    /// </summary>
    protected SpriteRenderer spriteRenderer;

    /// <summary>
    /// The collider used for checking interactions with this GridObject.
    /// </summary>
    protected CircleCollider2D circleCollider2D;

    protected virtual void Awake() {
      location2D = Vector2Int.RoundToInt(transform.position);
      spriteRenderer = GetComponent<SpriteRenderer>();
      circleCollider2D = GetComponent<CircleCollider2D>();
    }

    /// <summary>
    /// Returns whether this GridObject is diagonal to the given NodeV2.
    /// </summary>
    /// <param name="toCheck"></param>
    /// <returns></returns>
    protected bool IsDiagonalTo(GridObject toCheck) {
      return Math.Abs(toCheck.location2D.x - location2D.x) != 0
        && Math.Abs(toCheck.location2D.y - location2D.y) != 0;
    }

    public void DisableTrigger() {
      circleCollider2D.isTrigger = false;
    }
  }
}
