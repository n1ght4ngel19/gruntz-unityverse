using UnityEngine;
#if UNITY_EDITOR
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Reflection;
using System.Reflection.Emit;
using UnityEditor;
using Type = System.Type;
using static VInspector.Libs.VUtils;
using static VInspector.Libs.VGUI;

#endif



namespace VInspector
{
    public class VInspectorData : ScriptableObject
    {
#if UNITY_EDITOR

        public void Setup(Object target)
        {
            void buttons()
            {
                var membersWithButtonAttributes = new List<MemberInfo>();


                void findMembersWithButtonAttributes(Type type)
                {
                    membersWithButtonAttributes.AddRange(type.GetMembers(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                                                             .Where(r => r.GetCustomAttribute<ButtonAttribute>() is ButtonAttribute _)
                                                             .OrderBy(r => r is MethodInfo));

                    if (type == typeof(MonoBehaviour)) return;
                    if (type == typeof(ScriptableObject)) return;
                    if (type == null) return;
                    if (type.BaseType == null) return;

                    findMembersWithButtonAttributes(type.BaseType);

                }
                void createButton(MemberInfo member, ButtonAttribute buttonAttribute)
                {
                    var button = new Button();


                    if (member.GetCustomAttribute<ButtonSizeAttribute>() is ButtonSizeAttribute sizeAttribute)
                        button.size = sizeAttribute.size;

                    if (member.GetCustomAttribute<ButtonSpaceAttribute>() is ButtonSpaceAttribute spaceAttribute)
                        button.space = spaceAttribute.space;

                    if (member.GetCustomAttribute<TabAttribute>() is TabAttribute tabAttribute)
                        button.tab = tabAttribute.name;

                    if (member.GetCustomAttribute<IfAttribute>() is IfAttribute ifAttribute)
                        button.ifAttribute = ifAttribute;


                    if (member is FieldInfo field && field.FieldType == typeof(bool))
                    {
                        var o = field.IsStatic ? null : target;

                        button.isPressed = () => (bool)field.GetValue(o);
                        button.action = () => field.SetValue(o, !(bool)field.GetValue(o));
                        button.name = buttonAttribute.name != "" ? buttonAttribute.name : field.Name.PrettifyVarName(false);

                    }

                    if (member is MethodInfo method && !method.GetParameters().Any())
                    {
                        var o = method.IsStatic ? null : target;

                        button.isPressed = () => false;
                        button.action = () => method.Invoke(o, null);
                        button.name = buttonAttribute.name != "" ? buttonAttribute.name : method.Name.PrettifyVarName(false);

                    }


                    if (button.action != null)
                        this.buttons.Add(button);

                }


                this.buttons = new List<Button>();

                findMembersWithButtonAttributes(target.GetType());

                foreach (var member in membersWithButtonAttributes)
                    createButton(member, member.GetCustomAttribute<ButtonAttribute>());

            }
            void tabs()
            {
                var tabAttributes = new List<TabAttribute>();

                void createRootTab()
                {
                    if (rootTab != null) return;

                    rootTab = new Tab("");

                }
                void findTabAttributes(Type type)
                {
                    tabAttributes.AddRange(type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                                               .Select(r => r.GetCustomAttribute<TabAttribute>())
                                               .OfType<TabAttribute>());

                    if (type == typeof(MonoBehaviour)) return;
                    if (type == typeof(ScriptableObject)) return;
                    if (type == null) return;
                    if (type.BaseType == null) return;

                    findTabAttributes(type.BaseType);

                }
                void setupTab(Tab tab, IEnumerable<string> allSubtabPaths)
                {
                    void repair()
                    {
                        if (tab.subtabs == null)
                            tab.subtabs = new List<Tab>();

                        tab.subtabs.RemoveAll(r => r == null);

                    }
                    void refreshSubtabs()
                    {
                        var names = allSubtabPaths.Select(r => r.Split('/').First()).ToList();

                        foreach (var name in names)
                            if (tab.subtabs.None(r => r.name == name))
                                tab.subtabs.Add(new Tab(name));

                        foreach (var subtab in tab.subtabs.ToList())
                            if (names.None(r => r == subtab.name))
                                tab.subtabs.Remove(subtab);

                        tab.subtabs.SortBy(r => names.IndexOf(r.name));

                    }
                    void setupSubtabs()
                    {
                        foreach (var subtab in tab.subtabs)
                            setupTab(subtab, allSubtabPaths.Where(r => r.StartsWith(subtab.name + "/")).Select(r => r.Remove(subtab.name + "/")).ToList());
                    }

                    repair();
                    refreshSubtabs();
                    setupSubtabs();

                }

                createRootTab();
                findTabAttributes(target.GetType());
                setupTab(rootTab, tabAttributes.Select(r => r.name));

            }
            void foldouts()
            {
                var foldoutAttributes = new List<FoldoutAttribute>();

                void createRootFoldout()
                {
                    if (rootFoldout != null) return;

                    rootFoldout = new Foldout(true);

                }
                void findTabAttributes(Type type)
                {
                    foldoutAttributes.AddRange(type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                                                   .Select(r => r.GetCustomAttribute<FoldoutAttribute>())
                                                   .OfType<FoldoutAttribute>());

                    if (type == typeof(MonoBehaviour)) return;
                    if (type == typeof(ScriptableObject)) return;
                    if (type == null) return;
                    if (type.BaseType == null) return;

                    findTabAttributes(type.BaseType);

                }
                void setupFoldout(Foldout foldout, IEnumerable<string> allSubfoldoutPaths)
                {
                    void repair()
                    {
                        if (foldout.subfoldouts == null)
                            foldout.subfoldouts = new List<Foldout>();

                        foldout.subfoldouts.RemoveAll(r => r == null);

                    }
                    void refreshSubfoldouts()
                    {
                        var names = allSubfoldoutPaths.Select(r => r.Split('/').First()).ToList();

                        foreach (var name in names)
                            if (foldout.subfoldouts.Find(r => r.name == name) == null)
                                foldout.subfoldouts.Add(new Foldout(name));

                        foreach (var subtab in foldout.subfoldouts.ToList())
                            if (names.Find(r => r == subtab.name) == null)
                                foldout.subfoldouts.Remove(subtab);

                        foldout.subfoldouts.SortBy(r => names.IndexOf(r.name));

                    }
                    void setupSubfoldouts()
                    {
                        foreach (var subtab in foldout.subfoldouts)
                            setupFoldout(subtab, allSubfoldoutPaths.Where(r => r.StartsWith(subtab.name + "/")).Select(r => r.Remove(subtab.name + "/")).ToList());
                    }

                    repair();
                    refreshSubfoldouts();
                    setupSubfoldouts();

                }

                createRootFoldout();
                findTabAttributes(target.GetType());
                setupFoldout(rootFoldout, foldoutAttributes.Select(r => r.name));

            }

            buttons();
            tabs();
            foldouts();

        }

        public List<Button> buttons = new List<Button>();
        public Tab rootTab = new Tab("");
        public Foldout rootFoldout = new Foldout(true);

        public bool isIntact => buttons != null && rootTab?.subtabs != null && rootFoldout?.subfoldouts != null;


        public static Dictionary<Object, VInspectorData> datasByTarget = new Dictionary<Object, VInspectorData>();


        [System.Serializable]
        public class Button
        {
            public string name;
            public string tab = "";
            public float size = 30;
            public float space = 0;
            public IfAttribute ifAttribute;
            public System.Action action;
            public System.Func<bool> isPressed;

        }

        [System.Serializable]
        public class Tab
        {
            public string name;
            public int selectedSubtabIndex;

            [SerializeReference]
            public List<Tab> subtabs = new List<Tab>();

            public bool subtabsDrawn;
            public Tab selectedSubtab { get => selectedSubtabIndex.IsInRange(0, subtabs.Count - 1) ? subtabs[selectedSubtabIndex] : null; set => selectedSubtabIndex = subtabs.IndexOf(value); }

            public void ResetSubtabsDrawn()
            {
                subtabsDrawn = false;

                foreach (var r in subtabs)
                    r.ResetSubtabsDrawn();

            }

            public Tab(string name) => this.name = name;

        }

        [System.Serializable]
        public class Foldout
        {
            public string name;
            public bool expanded;

            [SerializeReference]
            public List<Foldout> subfoldouts = new List<Foldout>();

            public Foldout GetSubfoldout(string path)
            {
                if (path == "")
                    return this;
                else if (!path.Contains('/'))
                    return subfoldouts.Find(r => r.name == path);
                else
                    return subfoldouts.Find(r => r.name == path.Split('/').First()).GetSubfoldout(path.Substring(path.IndexOf('/') + 1));

            }
            public bool IsSubfoldoutContentVisible(string path)
            {
                if (path == "")
                    return expanded;
                else if (!path.Contains('/'))
                    return expanded && subfoldouts.Find(r => r.name == path).expanded;
                else
                    return expanded && subfoldouts.Find(r => r.name == path.Split('/').First()).IsSubfoldoutContentVisible(path.Substring(path.IndexOf('/') + 1));

            }

            public Foldout(string name) => this.name = name;
            public Foldout(bool expanded) => this.expanded = expanded;

        }

#endif
    }
}
