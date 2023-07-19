using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace KevinCastejon.AnimationGenerator
{
    /// <summary>
    /// Generates AnimationClip assets from a Texture2D spritesheet asset and allows to save and load a configuration file
    /// </summary>
    internal class AnimationGeneratorWindow : EditorWindow
    {
        private SpriteSheetConfiguration _config;
        private SerializedObject _srlzConfig;
        private Texture2D _spritesheet;
        private List<Sprite> _sprites = new List<Sprite>();
        private bool _selectFromProjectView;
        private string _internalPath;
        private int _spriteIndexPreview;
        private float _nextFrameTime;
        private Editor _previewEditor;
        private ReorderableList _list;
        private Vector2 _scrollPos;
        private float _currentScrollViewHeight;
        private bool _resize;
        private GUIStyle _horizontalLineStyle;
        internal SpriteSheetConfiguration Config
        {
            get => _config;
            set
            {
                _config = value;
                _srlzConfig = new SerializedObject(_config);
                InitializeList();
            }
        }

        private void OnEnable()
        {
            _internalPath = EditorPrefs.HasKey("AnimationGeneratorSpriteRendererInternalPath") ? EditorPrefs.GetString("AnimationGeneratorSpriteRendererInternalPath") : "";
            _selectFromProjectView = EditorPrefs.HasKey("AnimationGeneratorSelectFromProjectView") ? EditorPrefs.GetBool("AnimationGeneratorSelectFromProjectView") : false;
            titleContent = new GUIContent("Animation Generator");
            minSize = new Vector2(600, 400);
            _currentScrollViewHeight = this.position.height * 0.5f;
        }

        private void InitializeList()
        {
            SerializedProperty animations = _srlzConfig.FindProperty("_animations");
            _list = new ReorderableList(_srlzConfig, animations, true, false, false, false);
            _list.drawHeaderCallback = DrawHeaderCallback;
            _list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => DrawElementCallback(rect, index, animations);
            _list.elementHeightCallback = (int index) => ElementHeightCallback(index, animations);
            _list.onSelectCallback = (ReorderableList list) => OnSelectCallback(list, animations);
            if (_list.count > 0)
            {
                _list.Select(0);
            }
        }

        private void OnRemoveCallback(ReorderableList list, SerializedProperty animations)
        {
            animations.DeleteArrayElementAtIndex(list.index);
            if (list.count > 0)
            {
                list.index = list.index - 1 >= 0 ? list.index - 1 : 0;
            }
            else
            {
                list.index = -1;
            }
        }

        private void OnSelectCallback(ReorderableList list, SerializedProperty animations)
        {
            _nextFrameTime = (float)EditorApplication.timeSinceStartup;
            _spriteIndexPreview = GetSpriteStartIndexFromAnimationIndex(_list.index, animations);
        }

        private void OnAddCallback(ReorderableList list, SerializedProperty animations)
        {
            int index = list.count == 0 ? 0 : list.index + 1;
            animations.InsertArrayElementAtIndex(index);
            list.index = index;
            SerializedProperty newAnim = animations.GetArrayElementAtIndex(index);
            newAnim.FindPropertyRelative("_name").stringValue = MakeNameUnique(animations, "NewAnimation", index);
            newAnim.FindPropertyRelative("_length").intValue = 1;
            newAnim.FindPropertyRelative("_loop").boolValue = true;
            newAnim.FindPropertyRelative("_autoFramerate").boolValue = true;
        }

        private float ElementHeightCallback(int index, SerializedProperty animations)
        {
            return 100f;
        }

        private void DrawElementCallback(Rect rect, int index, SerializedProperty animations)
        {
            SerializedProperty property = animations.GetArrayElementAtIndex(index);
            SerializedProperty name = property.FindPropertyRelative("_name");
            SerializedProperty length = property.FindPropertyRelative("_length");
            SerializedProperty loop = property.FindPropertyRelative("_loop");
            SerializedProperty autoFramerate = property.FindPropertyRelative("_autoFramerate");
            SerializedProperty frameRate = property.FindPropertyRelative("_frameRate");
            rect.height -= 4;
            float rectWid = rect.width;
            float rectX = rect.x;
            rect.height /= 5;
            rect.width = rectWid * 0.25f;
            EditorGUI.LabelField(rect, name.stringValue, new GUIStyle(EditorStyles.boldLabel));
            rect.x = rectX + rectWid * 0.25f;
            rect.width = rectWid * 0.75f;
            EditorGUI.BeginChangeCheck();
            string newName = EditorGUI.DelayedTextField(rect, new GUIContent("Name"), name.stringValue);
            if (EditorGUI.EndChangeCheck())
            {
                name.stringValue = MakeNameUnique(animations, newName, index);
            }
            rect.y += rect.height;
            length.intValue = EditorGUI.IntField(rect, new GUIContent("Frame Count"), length.intValue);
            length.intValue = Mathf.Max(length.intValue, 1);
            rect.y += rect.height;
            loop.boolValue = EditorGUI.Toggle(rect, new GUIContent("Loop"), loop.boolValue);
            rect.y += rect.height;
            autoFramerate.boolValue = EditorGUI.Toggle(rect, new GUIContent("AutoFramerate", "AutoFramerate will set the framerate equals to the number of frames so the animation duration will be 1 second"), autoFramerate.boolValue);
            if (autoFramerate.boolValue)
            {
                EditorGUI.BeginDisabledGroup(true);
                frameRate.intValue = length.intValue;
            }
            rect.y += rect.height;
            frameRate.intValue = EditorGUI.IntField(rect, new GUIContent("Frame Rate"), frameRate.intValue);
            frameRate.intValue = Mathf.Max(frameRate.intValue, 0);
            if (autoFramerate.boolValue)
            {
                EditorGUI.EndDisabledGroup();
            }
        }

        private void DrawHeaderCallback(Rect rect)
        {
            EditorGUI.LabelField(rect, new GUIContent("Animations"));
        }

        private void Update()
        {
            if (_config == null)
            {
                Close();
            }
            if (_srlzConfig == null)
            {
                _srlzConfig = new SerializedObject(_config);
            }
            if (_selectFromProjectView && Selection.activeObject && (Selection.activeObject as Texture2D) != null && _spritesheet != Selection.activeObject as Texture2D)
            {
                _spritesheet = Selection.activeObject as Texture2D;
                _sprites = GetSprites();
            }

            //
            // Removed this part that was handling sprite modifications with open window but was drastically slowing the editor
            //

            //if (_spritesheet)
            //{
            //    List<Sprite> refreshedSprites = GetSprites();
            //    if (refreshedSprites.Count != _sprites.Count)
            //    {
            //        _sprites = refreshedSprites;
            //    }
            //}

            if (_sprites.Contains(null))
            {
                Close();
            }
            if (_list != null && _list.count > 0 && _spritesheet != null)
            {
                if (EditorApplication.timeSinceStartup >= _nextFrameTime)
                {
                    SerializedProperty animations = _srlzConfig.FindProperty("_animations");
                    SerializedProperty animation = animations.GetArrayElementAtIndex(_list.index);
                    SerializedProperty length = animation.FindPropertyRelative("_length");
                    float duration = 1f / animation.FindPropertyRelative("_frameRate").intValue;
                    int spriteStartIndex = GetSpriteStartIndexFromAnimationIndex(_list.index, animations);
                    int spriteEndIndex = spriteStartIndex + length.intValue - 1;
                    _nextFrameTime = (float)EditorApplication.timeSinceStartup + duration;
                    _spriteIndexPreview = _spriteIndexPreview + 1 > spriteEndIndex ? spriteStartIndex : _spriteIndexPreview + 1;
                    Repaint();
                }
            }
        }

        private void OnGUI()
        {
            if (_config == null)
            {
                Close();
                return;
            }
            if (_srlzConfig == null)
            {
                _srlzConfig = new SerializedObject(_config);
            }
            DefineStyles();
            EditorGUILayout.LabelField(new GUIContent("Spritesheet", "Select a Texture2D spritesheet asset with its Sprite Mode set to Multiple, ensure to properly cut the spritesheet into the Sprite Editor. This tool only support ordered sprites packing."));
            Texture2D oldSpriteSheet = _spritesheet;
            EditorGUI.BeginDisabledGroup(_selectFromProjectView);
            _spritesheet = (Texture2D)EditorGUILayout.ObjectField(_spritesheet, typeof(Texture2D), false);
            EditorGUI.EndDisabledGroup();
            if (_spritesheet && _spritesheet != oldSpriteSheet)
            {
                _sprites = GetSprites();
            }
            EditorGUI.BeginChangeCheck();
            _selectFromProjectView = EditorGUILayout.ToggleLeft(new GUIContent("Switch on project view selection", "If enabled, selecting a Texture2D asset into the project view will open it on this window."), _selectFromProjectView);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool("AnimationGeneratorSelectFromProjectView", _selectFromProjectView);
            }

            if (_list == null)
            {
                InitializeList();
            }

            _srlzConfig.Update();
            EditorGUI.BeginChangeCheck();
            _internalPath = EditorGUILayout.TextField("Sprite internal path", _internalPath);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetString("AnimationGeneratorSpriteRendererInternalPath", _internalPath);
            }
            SerializedProperty animations = _srlzConfig.FindProperty("_animations");

            EditorGUILayout.LabelField("Animations", new GUIStyle(EditorStyles.boldLabel));
            _scrollPos = GUILayout.BeginScrollView(_scrollPos, GUILayout.Height(_currentScrollViewHeight - 100f));
            Rect topRect = EditorGUILayout.GetControlRect(false, _list.GetHeight() - 22f);
            _list.DoList(topRect);
            GUILayout.EndScrollView();
            GUILayout.BeginHorizontal();
            Rect rect = EditorGUILayout.GetControlRect(false);
            if (GUI.Button(new Rect(rect.width - 55f, rect.y, 25f, rect.height), "+"))
            {
                OnAddCallback(_list, animations);
            }
            else if (GUI.Button(new Rect(rect.width - 30f, rect.y, 25f, rect.height), "-"))
            {
                OnRemoveCallback(_list, animations);
            }
            GUILayout.EndHorizontal();
            ResizeScrollView();
            if (_list.count > 0 && _spritesheet != null)
            {
                int spriteStartIndex = GetSpriteStartIndexFromAnimationIndex(_list.index, animations);
                int spriteEndIndex = spriteStartIndex + animations.GetArrayElementAtIndex(_list.index).FindPropertyRelative("_length").intValue - 1;
                bool canGenerateOne = true;
                if (spriteEndIndex < _sprites.Count)
                {
                    Rect editorRect = EditorGUILayout.GetControlRect(false, position.height - (_currentScrollViewHeight + 35f) - 35f);
                    _previewEditor = Editor.CreateEditor(_sprites[_spriteIndexPreview]);
                    _previewEditor.OnPreviewGUI(new Rect(editorRect), new GUIStyle { normal = { background = Texture2D.grayTexture } });
                }
                else
                {
                    GUILayout.Label("The selected animation exceed the spritesheet sprites count");
                    canGenerateOne = false;
                }
                int totalLength = 0;
                for (int i = 0; i < animations.arraySize; i++)
                {
                    totalLength += animations.GetArrayElementAtIndex(i).FindPropertyRelative("_length").intValue;
                }
                bool canGenerateAll = totalLength <= _sprites.Count;
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(!canGenerateAll);
                if (GUILayout.Button(new GUIContent("Generate all animations", canGenerateAll ? "Generate all animations on the list" : "Some animations on the list exceed the spritesheet sprites count.")))
                {
                    WriteFiles(animations);
                }
                EditorGUI.EndDisabledGroup();
                EditorGUI.BeginDisabledGroup(!canGenerateOne);
                if (GUILayout.Button(new GUIContent("Generate selected animation", canGenerateOne ? "Generate selected animation on the list" : "The selected animation exceeds the spritesheet sprites count.")))
                {
                    WriteFile(animations, _list.index);
                }
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                if (_spritesheet == null)
                {
                    EditorGUILayout.LabelField("Select a spritesheet to preview animations");
                }
                else if (_list.count == 0)
                {
                    EditorGUILayout.LabelField("Add animations on the list to preview them");
                }
            }
            _srlzConfig.ApplyModifiedProperties();
        }


        private void ResizeScrollView()
        {
            _currentScrollViewHeight = Mathf.Clamp(_currentScrollViewHeight, Mathf.Min(_list.GetHeight() + 100f - 16f, 200f), Mathf.Min(_list.GetHeight() + 100f - 16f, position.height - 200f));
            if (position.height > 400)
            {
                Rect orect = EditorGUILayout.GetControlRect(false);
                float handlesWidth = 15f;
                Rect cursorChangeRect = new Rect(0, _currentScrollViewHeight + 26f, orect.width, 5f);
                Rect cursorChangeTopRect = new Rect(orect.width * 0.5f - handlesWidth * 0.5f + 5f, _currentScrollViewHeight + 26f, handlesWidth, 1f);
                Rect cursorChangeBottomRect = new Rect(orect.width * 0.5f - handlesWidth * 0.5f + 5f, _currentScrollViewHeight + 26f + 4f, handlesWidth, 1f);
                Texture2D handleBar = Texture2D.grayTexture;
                Texture2D handles = Texture2D.whiteTexture;

                EditorGUIUtility.AddCursorRect(cursorChangeRect, MouseCursor.ResizeVertical);
                if (Event.current.type == EventType.MouseDown && cursorChangeRect.Contains(Event.current.mousePosition))
                {
                    _resize = true;
                }
                if (_resize)
                {
                    _currentScrollViewHeight = Mathf.Clamp(Event.current.mousePosition.y - 26f, Mathf.Min(_list.GetHeight() + 100f - 16f, 200f), Mathf.Min(_list.GetHeight() + 100f - 16f, position.height - 200f));
                }
                GUI.DrawTexture(cursorChangeRect, handleBar);
                GUI.DrawTexture(cursorChangeTopRect, handles);
                GUI.DrawTexture(cursorChangeBottomRect, handles);
                if (Event.current.type == EventType.MouseUp)
                {
                    _resize = false;
                }
            }
        }
        private void DefineStyles()
        {
            _horizontalLineStyle = new GUIStyle();
            _horizontalLineStyle.normal.background = EditorGUIUtility.whiteTexture;
            _horizontalLineStyle.margin = new RectOffset(0, 0, 4, 4);
            _horizontalLineStyle.fixedHeight = 1;
        }

        private List<Sprite> GetSprites()
        {
            List<Sprite> sprites = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(_spritesheet)).OfType<Sprite>().ToList();
            sprites.Sort(delegate (Sprite x, Sprite y)
            {
                return EditorUtility.NaturalCompare(x.name, y.name);
            });
            return sprites;
        }

        private int GetSpriteStartIndexFromAnimationIndex(int animIndex, SerializedProperty animations)
        {
            int spriteIndex = 0;
            for (int i = 0; i < animIndex; i++)
            {
                spriteIndex += animations.GetArrayElementAtIndex(i).FindPropertyRelative("_length").intValue;
            }
            return spriteIndex;
        }

        private void WriteFile(SerializedProperty animations, int index)
        {
            SerializedProperty animInfo = animations.GetArrayElementAtIndex(index);
            string filePath = EditorUtility.SaveFilePanelInProject("Choose where to save your AnimationClip assets", animInfo.FindPropertyRelative("_name").stringValue, "anim", "Save this animation");
            if (!string.IsNullOrEmpty(filePath))
            {
                int spriteCount = 0;
                for (int i = 0; i < index; i++)
                {
                    spriteCount += animations.GetArrayElementAtIndex(i).FindPropertyRelative("_length").intValue;
                }
                Sprite[] spr = new Sprite[animInfo.FindPropertyRelative("_length").intValue];
                _sprites.CopyTo(spriteCount, spr, 0, animInfo.FindPropertyRelative("_length").intValue);
                AnimationClip clip = CreateAnimationClip(spr, animInfo, _internalPath);
                AssetDatabase.CreateAsset(clip, filePath);
            }
        }

        private void WriteFiles(SerializedProperty animations)
        {
            string folderPath = EditorUtility.OpenFolderPanel("Choose a folder to create your AnimationClip assets", EditorPrefs.HasKey("AnimationGeneratorOutputFolderPath") ? EditorPrefs.GetString("AnimationGeneratorOutputFolderPath") : "Assets", "");
            EditorPrefs.SetString("AnimationGeneratorOutputFolderPath", folderPath);
            int assetsStringIndex = folderPath.IndexOf("Assets");
            if (assetsStringIndex == -1)
            {
                return;
            }
            folderPath = folderPath.Substring(folderPath.IndexOf("Assets"));
            int spriteCount = 0;
            for (int i = 0; i < animations.arraySize; i++)
            {
                SerializedProperty animInfo = animations.GetArrayElementAtIndex(i);
                if (spriteCount + animInfo.FindPropertyRelative("_length").intValue > _sprites.Count)
                {
                    break;
                }
                Sprite[] spr = new Sprite[animInfo.FindPropertyRelative("_length").intValue];
                _sprites.CopyTo(spriteCount, spr, 0, animInfo.FindPropertyRelative("_length").intValue);
                AnimationClip clip = CreateAnimationClip(spr, animInfo, _internalPath);
                AssetDatabase.CreateAsset(clip, folderPath + "/" + animInfo.FindPropertyRelative("_name").stringValue + ".anim");
                spriteCount += animInfo.FindPropertyRelative("_length").intValue;
            }
        }

        private static AnimationClip CreateAnimationClip(Sprite[] sprites, SerializedProperty animInfo, string path)
        {
            AnimationClip clip = new AnimationClip();
            clip.frameRate = animInfo.FindPropertyRelative("_frameRate").intValue;

            ObjectReferenceKeyframe[] keys = new ObjectReferenceKeyframe[animInfo.FindPropertyRelative("_length").intValue];
            for (int i = 0; i < animInfo.FindPropertyRelative("_length").intValue; i++)
            {
                keys[i] = new ObjectReferenceKeyframe
                {
                    time = i / clip.frameRate,
                    value = sprites[i]
                };
            }
            EditorCurveBinding curveBinding = new EditorCurveBinding();
            curveBinding.propertyName = "m_Sprite";
            curveBinding.path = path;
            curveBinding.type = typeof(SpriteRenderer);
            AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keys);
            AnimationClipSettings clipSetting = new AnimationClipSettings
            {
                loopTime = animInfo.FindPropertyRelative("_loop").boolValue,
                stopTime = 1f
            };
            AnimationUtility.SetAnimationClipSettings(clip, clipSetting);
            return clip;
        }
        private string MakeNameUnique(SerializedProperty animations, string newName, int newAnimIndex)
        {
            while (!IsNameUnique(animations, newName, newAnimIndex))
            {
                newName += "_";
            }
            return newName;
        }

        private bool IsNameUnique(SerializedProperty animations, string newName, int newAnimIndex)
        {
            for (int i = 0; i < animations.arraySize; i++)
            {
                if (newAnimIndex == i)
                {
                    continue;
                }
                SerializedProperty animInfo = animations.GetArrayElementAtIndex(i);
                if (animInfo.FindPropertyRelative("_name").stringValue == newName)
                {
                    return false;
                }
            }
            return true;
        }
    }
}