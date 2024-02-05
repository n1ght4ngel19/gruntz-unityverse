using GruntzUnityverse.V2.Itemz;
using UnityEditor;
using UnityEngine;

namespace GruntzUnityverse.V2.Objectz {
  /// <summary>
  /// A Grunt-sized piece of rock that blocks the path and possibly holds an <see cref="ItemV2"/>.
  /// </summary>
  public class RockV2 : GridObject, IObjectHolder {
    [field: SerializeField] public ItemV2 HeldItem { get; set; }

    public void DropItem(bool isSceneLoaded) {
      if (!isSceneLoaded || HeldItem == null) {
        return;
      }

      Instantiate(HeldItem, transform.position, Quaternion.identity, GameObject.Find("Itemz").transform);
    }

    protected override void OnDestroy() {
      base.OnDestroy();

      DropItem(gameObject.scene.isLoaded);
    }
  }

  #if UNITY_EDITOR
  [CustomEditor(typeof(RockV2))]
  public class RockV2Editor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      base.OnInspectorGUI();

      MonoBehaviour objectHolder = (RockV2)target;

      if (GUILayout.Button("Destroy Rock")) {
        Destroy(objectHolder.gameObject);
      }
    }
  }
  #endif
}
