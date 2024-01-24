#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using System.Reflection;
using System.Linq;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using static VHierarchy.Libs.VUtils;
using static VHierarchy.Libs.VGUI;
using static VHierarchy.VHierarchyData;


namespace VHierarchy
{
    public class VHierarchyIconEditor : CustomPopupWindow
    {
        void OnGUI()
        {
            if (iconRows == null)
                if (goData != null) Init(go, goData);
                else { Close(); return; }

            VHierarchy.data?.RecordUndo();

            var buttonSize = 18;
            var buttonSpacing = 2;
            var rowStartX = 6;

            var bgSelected = new Color(.3f, .5f, .7f, .8f);
            var bgHovered = Greyscale(1, .3f);

            void icons()
            {
                string hoveredIcon = null;

                void iconButton(Rect rect, string icon)
                {
                    if (icon == initIcon)
                        rect.Draw(bgSelected);

                    if (rect.Resize(-1).IsHovered())
                    {
                        rect.Draw(bgHovered);
                        hoveredIcon = icon;
                    }

                    if (e.mouseDown() && rect.Resize(-1).IsHovered())
                        Close();

                    GUI.skin.label.alignment = TextAnchor.MiddleCenter;
                    GUI.Label(rect, EditorGUIUtility.IconContent(icon == "" ? "CrossIcon" : icon));
                    GUI.skin.label.alignment = TextAnchor.MiddleLeft;

                }

                for (int i = 0; i < iconRows.Length; i++)
                {
                    GUILayout.Label("");

                    var iconRect = lastRect.SetWidth(buttonSize).SetHeightFromMid(buttonSize).MoveX(rowStartX);

                    foreach (var icon in iconRows[i])
                    {
                        iconButton(iconRect, icon);
                        iconRect = iconRect.MoveX(buttonSize + buttonSpacing);
                    }

                    Space(buttonSpacing);

                }

                goData.icon = hoveredIcon ?? initIcon;

            }
            void colors_()
            {
                int hoveredIColor = -1;

                void colorButton(Rect rect, int iColor)
                {
                    if (iColor == initIColor)
                        rect.Draw(bgSelected);

                    if (rect.Resize(-1).IsHovered())
                    {
                        rect.Draw(bgHovered);
                        hoveredIColor = iColor;
                    }

                    if (e.mouseDown() && rect.Resize(-1).IsHovered())
                        Close();

                    if (iColor == 0)
                    {
                        SetLabelAlignmentCenter();
                        GUI.Label(rect, EditorGUIUtility.IconContent("CrossIcon"));
                        ResetLabelStyle();
                    }
                    else
                        rect.Resize(4).Draw(GetColor(iColor) * (EditorGUIUtility.isProSkin ? 1.3f : 1));

                }

                GUILayout.Label("");

                var iconRect = lastRect.SetWidth(buttonSize).SetHeightFromMid(buttonSize).MoveX(rowStartX);

                for (int i = 0; i < colorsN; i++)
                {
                    colorButton(iconRect, i);
                    iconRect = iconRect.MoveX(buttonSize + buttonSpacing);
                }

                if (hoveredIColor != -1)
                    goData.iColor = hoveredIColor;
                else
                    goData.iColor = initIColor;
                // goData.color = hoveredIColor ?? initIColor;

            }

            // HeaderGUI();

            BeginIndent(8);

            Space(13);
            colors_();

            Space(12);
            icons();

            EndIndent(8);

            if (e.keyDown() && e.keyCode == KeyCode.Escape)
            {
                goData.icon = initIcon;
                goData.iColor = initIColor;
                Close();
            }

            if (Application.platform != RuntimePlatform.OSXEditor)
                DrawOutline();

            Repaint();
            EditorApplication.RepaintHierarchyWindow();

        }


        public void Init(GameObject go, GameObjectData goData)
        {
            this.goData = goData;

            initIcon = goData.icon;
            initIColor = goData.iColor;

            iconRows = new string[][]
            {
            new[]
            {
                "",
                "Folder Icon",
                "Canvas Icon",
                "AvatarMask On Icon",
                "cs Script Icon",
                "StandaloneInputModule Icon",
                "EventSystem Icon",
                "Terrain Icon",
                "ScriptableObject Icon",

            },
            new[]
            {
                "Camera Icon",
                "ParticleSystem Icon",
                "TrailRenderer Icon",
                "Material Icon",
                "ReflectionProbe Icon",
            },
            new[]
            {
                "Light Icon",
                "DirectionalLight Icon",
                "LightmapParameters Icon",
                "LightProbes Icon",
            },
            new[]
            {
                "Rigidbody Icon",
                "BoxCollider Icon",
                "SphereCollider Icon",
                "CapsuleCollider Icon",
                "WheelCollider Icon",
                "MeshCollider Icon",
            },
            new[]
            {
                "AudioSource Icon",
                "AudioClip Icon",
                "AudioListener Icon",
                "AudioEchoFilter Icon",
                "AudioReverbZone Icon",
            },
            new[]
            {
                "PreMatCube",
                "PreMatSphere",
                "PreMatCylinder",
                "PreMatQuad",
                "Favorite",
                #if UNITY_2021_3_OR_NEWER
                "Settings Icon",
                #endif

            },

            };

            VHierarchy.data.Dirty();

            Undo.undoRedoPerformed += RepaintOnUndoRedo;

        }
        GameObject go;
        GameObjectData goData;
        string initIcon;
        int initIColor;
        static string[][] iconRows;



        public static Color GetColor(int iColor)
        {
            if (colors != null) return colors[iColor];


            colors = new Color[colorsN];

            colors[0] = default;

#if UNITY_2022_1_OR_NEWER
            colors[1] = Greyscale(EditorGUIUtility.isProSkin ? .16f : .9f);
#else
            colors[1] = Greyscale(EditorGUIUtility.isProSkin ? .315f : .9f);
#endif

            for (int i = 2; i < colors.Length; i++)
            {
                var color = EditorGUIUtility.isProSkin ?
                    HSLToRGB((i + 1f) / (colors.Length - 1), .4f, .34f)
                  : HSLToRGB((i + 1f) / (colors.Length - 1), .6f, .8f);
                color *= 1.1f;
                color.a = 1;

                colors[i] = color;
            }

            return colors[iColor];

        }
        static Color[] colors;
        public static int colorsN = 10;





        static void RepaintOnUndoRedo()
        {
            EditorApplication.RepaintProjectWindow();
            Undo.undoRedoPerformed -= RepaintOnUndoRedo;
        }


        public override float initWidth => 224;
        public override float initHeight => 178;

    }
}
#endif