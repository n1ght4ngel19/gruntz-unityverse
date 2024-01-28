using HierarchyIcons;
using UnityEngine;
using UnityEditor;

namespace CGHierarchyIconsEditor
{
    [InitializeOnLoad]
    public class ShowIconOnHierarchyView
    {
        const float MAX_ICON_SIZE = 16;

        static ShowIconOnHierarchyView()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemCallback;
        }

        static void HierarchyWindowItemCallback(int instanceID, Rect selectionRect)
        {
            GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (gameObject == null)
                return;

            HierarchyIcon[] hierarchyIcons = gameObject.GetComponents<HierarchyIcon>();
            foreach (HierarchyIcon hierarchyIcon in hierarchyIcons)
            {
                Texture2D icon = hierarchyIcon.icon ? hierarchyIcon.icon : TextureHelper.NO_ICON;
                float width = Mathf.Min(icon.width, MAX_ICON_SIZE);
                float height = Mathf.Min(icon.height, MAX_ICON_SIZE);

                // get the starting x position based on the direction
                float x = selectionRect.x;
                if (hierarchyIcon.direction == HierarchyIcon.Direction.LeftToRight)
                    x += hierarchyIcon.position * MAX_ICON_SIZE;
                else
                    x += (selectionRect.width - width) - hierarchyIcon.position * MAX_ICON_SIZE;

                // draw the icon
                Rect rect = new Rect(x, selectionRect.y, width, height);
                GUI.DrawTexture(rect, icon);

                // set link cursor
                EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);

                // and draw a button for change the icon and display the tooltip
                if (GUI.Button(rect, new GUIContent(string.Empty, hierarchyIcon.tooltip), EditorStyles.label))
                    PopupWindow.Show(rect, new PickIconWindow(hierarchyIcon));
            }
        }
    }
}