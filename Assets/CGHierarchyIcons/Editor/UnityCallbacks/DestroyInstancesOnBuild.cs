using System;
using System.Collections.Generic;
using HierarchyIcons;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

// https://forum.unity.com/threads/excluding-certain-monobehaviour-classes-from-build.568090/#post-3779695
public class DestroyInstancesOnBuild : MonoBehaviour
{
    [PostProcessScene]
    public static void DeleteObjects()
    {
        if (BuildPipeline.isBuildingPlayer)
        {
            Type type = typeof(HierarchyIcon);
            Debug.Log("Destroying all instances of '" + type.Name + "' from scenes on build!");

            foreach (Component obj in FindObjectsOfTypeAll(type, true))
                DestroyImmediate(obj);
        }
    }

    static List<Component> FindObjectsOfTypeAll(Type type, bool findInactive = false)
    {
        List<Component> results = new List<Component>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded)
            {
                GameObject[] allGameObjects = scene.GetRootGameObjects();
                for (int j = 0; j < allGameObjects.Length; j++)
                {
                    GameObject go = allGameObjects[j];
                    results.AddRange(go.GetComponentsInChildren(type, findInactive));
                }
            }
        }

        return results;
    }
}