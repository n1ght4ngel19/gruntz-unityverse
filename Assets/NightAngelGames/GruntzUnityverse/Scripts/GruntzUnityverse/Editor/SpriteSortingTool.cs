using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Objectz.Misc;
using UnityEditor;
using UnityEngine;


public class SpriteSortingTool : EditorWindow {
    [MenuItem("Tools/Sprite Sorting Tool")]
    public static void ShowWindow() {
        GetWindow<SpriteSortingTool>("Sprite Sorting Tool");
    }

    private void OnGUI() {
        if (GUILayout.Button("Sort Sprites by Y Position")) {
            SortSelectedSprites();
        }
    }

    private void SortSelectedSprites() {
        List<EyeCandy> selectedObjects = FindObjectsByType<EyeCandy>(FindObjectsSortMode.None).ToList();

        // Sort by Y position
        selectedObjects.Sort((a, b) => b.transform.position.y.CompareTo(a.transform.position.y));

        int sortingOrder = 0;

        foreach (EyeCandy ec in selectedObjects) {
            SpriteRenderer sr = ec.GetComponent<SpriteRenderer>();

            if (sr is not null) {
                sr.sortingOrder = sortingOrder++;
            }
        }

        Debug.Log("Sprites sorted by Y position!");
    }
}
