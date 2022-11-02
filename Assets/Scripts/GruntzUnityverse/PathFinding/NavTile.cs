using UnityEngine;

namespace GruntzUnityverse.PathFinding {
  /// <summary>
  /// Representation of a Node used in A* pathfinding.
  /// </summary>
  public class NavTile : MonoBehaviour {
    public int G;
    public int H;
    public int F {get => G + H;}

    public Vector3Int gridLocation;

    // Currently not used, is this necessary?
    public bool isBlocked;
    public NavTile previous;
  }
}
