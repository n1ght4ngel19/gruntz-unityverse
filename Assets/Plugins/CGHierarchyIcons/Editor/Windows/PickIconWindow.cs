using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using HierarchyIcons;

namespace CGHierarchyIconsEditor
{
    internal class PickIconWindow : PopupWindowContent
    {
        readonly SerializedProperty m_iconProperty;
        Vector2 m_scrollPosition;
        int m_myUndoGroup;

        static GUIContent[] s_IconContents;
        static readonly GUIStyle BUTTON_STYLE;
        static string s_searchWord = "";
        static bool s_loadAllIcons;

        const int BUTTON_STYLE_PADDING = 2;
        const float BUTTON_SIZE = 16 + BUTTON_STYLE_PADDING * 2;
        const float COLUMNS = 12;
        const int HEADER_HEIGHT = 22;
        const int BODY_HEIGHT = 200;
        const int BOTTOM_HEIGHT = 26;

        static PickIconWindow()
        {
            BUTTON_STYLE = new GUIStyle
            {
                fixedWidth = BUTTON_SIZE,
                fixedHeight = BUTTON_SIZE,
                alignment = TextAnchor.MiddleCenter,
                onNormal = {background = TextureHelper.BUTTON_ON},
                padding = new RectOffset(
                    BUTTON_STYLE_PADDING,
                    BUTTON_STYLE_PADDING,
                    BUTTON_STYLE_PADDING,
                    BUTTON_STYLE_PADDING
                )
            };
        }

        public PickIconWindow(HierarchyIcon hierarchyIcon) : this(new SerializedObject(hierarchyIcon).FindProperty("icon"))
        {
            if (m_iconProperty == null)
                Debug.LogError("'icon' property not found in the HierarchyIcon script.");
        }

        public PickIconWindow(SerializedProperty target)
        {
            LoadIcons(false);

            m_iconProperty = target;
            m_myUndoGroup = -1;
        }

        public override void OnGUI(Rect rect)
        {
            if (m_iconProperty == null) return;

            // body - scroll view with a selection grid
            {
                float rows = Mathf.Ceil(s_IconContents.Length / COLUMNS);
                float height = rows * BUTTON_SIZE;

                // calculate rects
                Rect rectScrollViewPosition = rect;
                rectScrollViewPosition.y = HEADER_HEIGHT;
                rectScrollViewPosition.height -= HEADER_HEIGHT + BOTTOM_HEIGHT;

                Rect rectScrollView = new Rect(0, 0, rect.width - BUTTON_SIZE, height);

                m_scrollPosition = GUI.BeginScrollView(rectScrollViewPosition, m_scrollPosition, rectScrollView, false, true);
                {
                    const float X = 3;
                    Rect rectButtons = new Rect(X, 1, BUTTON_SIZE, BUTTON_SIZE);
                    int counter = 0; // used in the case we are searching
                    for (int i = 0; i < s_IconContents.Length; i++)
                    {
                        GUIContent content = s_IconContents[i];

                        if (!ContainsSearchWord(content.tooltip))
                            continue;

                        bool selected = (content.image == m_iconProperty.objectReferenceValue);
                        EditorGUI.BeginChangeCheck();
                        GUI.Toggle(rectButtons, selected, content, BUTTON_STYLE);
                        if (EditorGUI.EndChangeCheck())
                            AssignIconAndRecordUndo(i);

                        rectButtons.x += BUTTON_SIZE;

                        if ((counter + 1) % COLUMNS == 0)
                        {
                            rectButtons.y += BUTTON_SIZE;
                            rectButtons.x = X;
                        }

                        counter++;
                    }
                }
                GUI.EndScrollView();
            }

            // header
            {
                // search
                {
                    GUI.Label(new Rect(3, 3, 50, 17), "Search:");
                    Rect rectCloseSearch = new Rect(167, 4, 16, 16);

                    // the button for clicking
                    if (GUI.Button(rectCloseSearch, "", EditorStyles.label))
                        s_searchWord = "";

                    s_searchWord = GUI.TextField(new Rect(53, 3, 130, 17), s_searchWord, EditorStyles.helpBox);

                    // the button graphic on top of the text field
                    if (s_searchWord != "")
                        GUI.Label(rectCloseSearch, new GUIContent(TextureHelper.CLEAR_SEARCH, "Clear Search"));
                }

                // none button
                {
                    if (!m_iconProperty.objectReferenceValue) // disable the button if no icon
                        GUI.enabled = false;

                    GUIContent noneIconContent = new GUIContent("None", TextureHelper.REMOVE_ICON, "Remove Icon");
                    float width = EditorStyles.label.CalcSize(noneIconContent).x + 5;
                    if (GUI.Button(new Rect(rect.width - width, 4, width, HEADER_HEIGHT), noneIconContent, EditorStyles.label))
                    {
                        AssignIconAndRecordUndo(-1);
                        //editorWindow.Close();
                    }

                    GUI.enabled = true;
                }

                // horizontal line
                DrawLineH(0, HEADER_HEIGHT, rect.width, Color.gray);
            }

            Event e = Event.current;

            // other icon
            {
                // horizontal line
                DrawLineH(0, rect.height - BOTTOM_HEIGHT, rect.width, Color.gray);

                const float MARGIN_LEFT_AND_RIGHT = 50;
                const float MARGIN_TOP_AND_BOTTOM = 3;
                Rect rectButton = new Rect(
                    MARGIN_LEFT_AND_RIGHT,
                    rect.height - BOTTOM_HEIGHT + MARGIN_TOP_AND_BOTTOM,
                    rect.width - MARGIN_LEFT_AND_RIGHT * 2,
                    BOTTOM_HEIGHT - MARGIN_TOP_AND_BOTTOM * 2);

                int controlID = GUIUtility.GetControlID("rectButtonOtherIcon***".GetHashCode(), FocusType.Keyboard, rectButton);
                if (GUI.Button(rectButton, "Other..."))
                {
                    GUIUtility.keyboardControl = controlID;
                    EditorGUIUtility.ShowObjectPicker<Texture2D>(m_iconProperty.objectReferenceValue, false, string.Empty, controlID);
                }

                // handle object picker commands
                if (e.type == EventType.ExecuteCommand)
                {
                    string commandName = e.commandName;
                    if (EditorGUIUtility.GetObjectPickerControlID() == controlID && GUIUtility.keyboardControl == controlID)
                    {
                        GUI.changed = true;
                        e.Use();

                        if (commandName == "ObjectSelectorUpdated")
                        {
                            Texture2D icon = EditorGUIUtility.GetObjectPickerObject() as Texture2D;
                            AssignIconAndRecordUndo(icon);
                        }
                    }
                }

                // toggle load all icons
                {
                    const float W = 40, H = 17;
                    Rect r = new Rect(rect.width - W, rect.height - H - 3, W, H);
                    EditorGUI.BeginChangeCheck();
                    {
                        s_loadAllIcons = GUI.Toggle(r, s_loadAllIcons, new GUIContent("All", "Show all unity textures. Including yours in the project view."));
                    }
                    if (EditorGUI.EndChangeCheck())
                        LoadIcons(true);
                }
            }

            // cancel with escape
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Escape)
                editorWindow.Close();
        }

        static bool ContainsSearchWord(string name)
        {
            string searchWordLower = s_searchWord.Trim().ToLower();

            if (name.ToLower().Contains(searchWordLower))
                return true;

            return false;
        }

        public static void DrawRect(Rect r, Color c)
        {
            GUI.color = c;
            GUI.DrawTexture(r, EditorGUIUtility.whiteTexture);
            GUI.color = Color.white;
        }

        public static void DrawLineH(float x, float y, float width, Color c)
        {
            DrawRect(new Rect(x, y, width, 1), c);
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(
                BUTTON_SIZE * COLUMNS + BUTTON_SIZE,
                HEADER_HEIGHT + BODY_HEIGHT + BOTTOM_HEIGHT
            );
        }

        void AssignIconAndRecordUndo(Texture2D newIcon)
        {
            if (m_iconProperty.objectReferenceValue != newIcon)
            {
                if (m_myUndoGroup == -1)
                {
                    Undo.IncrementCurrentGroup();
                    m_myUndoGroup = Undo.GetCurrentGroup();
                }

                m_iconProperty.objectReferenceValue = newIcon;
                m_iconProperty.serializedObject.ApplyModifiedProperties();

                // repaint the hierarchy
                EditorApplication.RepaintHierarchyWindow();
            }
        }

        void AssignIconAndRecordUndo(int newBuiltinIconIndex)
        {
            AssignIconAndRecordUndo(GetIcon(newBuiltinIconIndex));
        }

        public override void OnClose()
        {
            Undo.CollapseUndoOperations(m_myUndoGroup);
        }

        static Texture2D GetIcon(int index)
        {
            LoadIcons(false);

            if (index >= 0 && index < s_IconContents.Length)
                return (Texture2D) s_IconContents[index].image;

            return null;
        }

        static void LoadIcons(bool showAllChanged)
        {
            if (s_IconContents == null || showAllChanged)
            {
                LoadAssetPreviews();

                List<GUIContent> listIcons = new List<GUIContent>();

                // find all unity textures
                Texture[] textures = Resources.FindObjectsOfTypeAll<Texture>();
                Array.Sort(textures, (pA, pB) => string.Compare(pA.name, pB.name, StringComparison.OrdinalIgnoreCase));

                string ICON_TEXT = "icon";
                foreach (Texture texture in textures)
                {
                    string name = texture.name;
                    if (name == string.Empty) continue;

                    if (!s_loadAllIcons && !name.ToLower().Contains(ICON_TEXT)) continue;

                    listIcons.Add(new GUIContent(texture, name));
                }

                s_IconContents = listIcons.ToArray();
            }
        }

        /// <summary>
        /// Load all assets preview so that we can find it
        /// </summary>
        static void LoadAssetPreviews()
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!assembly.FullName.StartsWith("UnityEngine")) // load only unity engine assemblies
                    continue;

                foreach (Type type in assembly.GetTypes())
                    AssetPreview.GetMiniTypeThumbnail(type);
            }
        }
    }
}