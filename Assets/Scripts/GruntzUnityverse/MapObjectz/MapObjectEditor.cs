using GruntzUnityverse.MapObjectz.BaseClasses;
using UnityEditor;
using UnityEngine;

namespace GruntzUnityverse.MapObjectz {
  [CustomEditor(typeof(MapObject))]
  public class MapObjectEditor : Editor {
    private void OnEnable() {
      (target as MapObject).GetComponent<SpriteRenderer>().hideFlags = HideFlags.HideInInspector;
    }
  }
}
