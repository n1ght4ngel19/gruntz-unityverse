using System;
using UnityEngine;

namespace GruntzUnityverse.V2.Objectz {
  public class GridObject : MonoBehaviour {
    public SpriteRenderer spriteRenderer;
    public Vector2Int location2D;

    /// <summary>
    /// Returns whether this GridObject is diagonal to the given NodeV2.
    /// </summary>
    /// <param name="toCheck"></param>
    /// <returns></returns>
    protected bool IsDiagonalTo(GridObject toCheck) {
      return Math.Abs(toCheck.location2D.x - location2D.x) != 0
        && Math.Abs(toCheck.location2D.y - location2D.y) != 0;
    }
  }
}
