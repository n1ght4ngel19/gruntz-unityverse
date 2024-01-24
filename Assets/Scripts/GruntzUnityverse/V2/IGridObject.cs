using UnityEngine;

namespace GruntzUnityverse.V2 {
  public interface IGridObject {
    /// <summary>
    /// The location of the object in the grid.
    /// </summary>
    public Vector2Int Location2D { get; set; }
  }
}
