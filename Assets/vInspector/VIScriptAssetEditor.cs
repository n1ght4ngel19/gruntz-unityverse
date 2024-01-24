#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEditor.ShortcutManagement;
using System.Reflection;
using System.Linq;
using System.Diagnostics;
using Type = System.Type;
using static VInspector.Libs.VUtils;
using static VInspector.Libs.VGUI;


namespace VInspector
{
    class VIScriptAssetEditor
    {
        static void HeaderGUI(Editor editor)
        {
            if (editor.GetType() != typeof(Editor).Assembly.GetType("UnityEditor.MonoScriptImporterInspector")) return;

            if (!((editor.target as MonoImporter)?.GetScript() is MonoScript script)) return;

            if (!(script.GetClass() is Type classType)) return;

            var staticVars = classType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static).Where(r => !r.IsLiteral && !r.IsInitOnly && supportedTypes.Any(rr => rr.IsAssignableFrom(r.FieldType))).ToArray();
            var staticFuncs = classType.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static).Where(r => r.GetParameters().Count() == 0).ToArray();

            var nothingShown = true;

            void saveDefaultValues()
            {
                if (defaultValuesByClassType.ContainsKey(classType)) return;



                defaultValuesByClassType[classType] = new Dictionary<FieldInfo, object>();
                foreach (var r in staticVars)
                    defaultValuesByClassType[classType][r] = r.GetValue(null);




            }

            void drawBackground()
            {
                var lineCol = Greyscale(.1f);

                Space(10);
                var lineRect = ExpandWidthLabelRect();
                lineRect = lineRect.SetWidthFromMid(lineRect.width + 123).SetHeight(1);

                lineRect.Draw(lineCol);

                var bgRect = lineRect.MoveY(1).SetHeight(123123);
                bgRect.Draw(backgroundCol);

            }
            void drawStaticVars()
            {
                if (!staticVars.Any()) return;

                nothingShown = false;

                if (!(staticVariablesExpanded = Foldout("Static Variables", staticVariablesExpanded))) return;

                foreach (var field in staticVars)
                {
                    var name = "  " + field.Name.PrettifyVarName(false);


                    if (typeof(Object).IsAssignableFrom(field.FieldType))
                        field.SetValue(null, ResettableField(name, (Object)field.GetValue(null), (Object)defaultValuesByClassType[classType][field]));

                    if (field.FieldType == typeof(float))
                        field.SetValue(null, ResettableField(name, (float)field.GetValue(null), (float)defaultValuesByClassType[classType][field]));

                    if (field.FieldType == typeof(int))
                        field.SetValue(null, ResettableField(name, (int)field.GetValue(null), (int)defaultValuesByClassType[classType][field]));

                    if (field.FieldType == typeof(string))
                        field.SetValue(null, ResettableField(name, (string)field.GetValue(null), (string)defaultValuesByClassType[classType][field]));

                    if (field.FieldType == typeof(bool))
                        field.SetValue(null, ResettableField(name, (bool)field.GetValue(null), (bool)defaultValuesByClassType[classType][field]));

                }

                Space(6);

            }
            void drawStaticFuncs()
            {
                if (!staticFuncs.Any()) return;

                nothingShown = false;

                if (!(staticFunctionsExpanded = Foldout("Static Functions", staticFunctionsExpanded))) return;

                Space(6);

                foreach (var function in staticFuncs)
                {
                    var name = " " + function.Name;//.Decamelcase();

                    Space(-2);
                    GUILayout.BeginHorizontal();
                    Space(16);
                    if (GUILayout.Button(name, GUILayout.Height(28)))
                        function.Invoke(null, null);
                    Space(8);
                    GUILayout.EndHorizontal();

                }

                Space(10);

            }


            saveDefaultValues();

            drawBackground();

            Space(-8);
            EditorGUI.indentLevel = 1;

            drawStaticVars();
            drawStaticFuncs();

            if (nothingShown)
                Space(-16);

            EditorGUI.indentLevel = 0;

        }

        static bool staticVariablesExpanded { get => EditorPrefs.GetBool("VIScriptAssetEditor-staticVariablesExpanded", false); set => EditorPrefs.SetBool("VIScriptAssetEditor-staticVariablesExpanded", value); }
        static bool staticFunctionsExpanded { get => EditorPrefs.GetBool("VIScriptAssetEditor-staticMethodsExpanded", false); set => EditorPrefs.SetBool("VIScriptAssetEditor-staticMethodsExpanded", value); }

        static Dictionary<System.Type, Dictionary<FieldInfo, object>> defaultValuesByClassType = new Dictionary<System.Type, Dictionary<FieldInfo, object>>();

        static System.Type[] supportedTypes = new[] { typeof(Object), typeof(float), typeof(int), typeof(string), typeof(bool) };


#if !DISABLED
        [InitializeOnLoadMethod]
        static void Init()
        {
            Editor.finishedDefaultHeaderGUI -= HeaderGUI;
            Editor.finishedDefaultHeaderGUI += HeaderGUI;
        }
#endif
    }

}
#endif
