using UnityEngine;

namespace GruntzUnityverse.PathFinding {
  public class NavTile : MonoBehaviour {
    public int G;
    public int H;
    public int F {get => G + H;}

    public Vector3Int gridLocation;

    public bool isBlocked;
    public NavTile previous;
  }
}
