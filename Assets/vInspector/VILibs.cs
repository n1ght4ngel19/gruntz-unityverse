#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;
using UnityEditor;
using Type = System.Type;
using static VInspector.Libs.VUtils;


namespace VInspector.Libs
{
    public static class VUtils
    {
        #region Text

        public static string FormatDistance(float meters)
        {
            int m = (int)meters;

            if (m < 1000) return m + " m";
            else return (m / 1000) + "." + (m / 100) % 10 + " km";
        }
        public static string FormatLong(long l) => System.String.Format("{0:n0}", l);
        public static string FormatInt(int l) => FormatLong((long)l);
        public static string FormatSize(long bytes, bool sizeUnknownIfNotMoreThanZero = false)
        {
            if (sizeUnknownIfNotMoreThanZero && bytes == 0) return "Size unknown";

            var ss = new[] { "B", "KB", "MB", "GB", "TB" };
            var bprev = bytes;
            int i = 0;
            while (bytes >= 1024 && i++ < ss.Length - 1) bytes = (bprev = bytes) / 1024;

            if (bytes < 0) return "? B";
            if (i < 3) return string.Format("{0:0.#} ", bytes) + ss[i];
            return string.Format("{0:0.##} ", bytes) + ss[i];
        }
        public static string FormatTime(long ms, bool includeMs = false)
        {
            System.TimeSpan t = System.TimeSpan.FromMilliseconds(ms);
            var s = "";
            if (t.Hours != 0) s += " " + t.Hours + " hour" + CountSuffix(t.Hours);
            if (t.Minutes != 0) s += " " + t.Minutes + " minute" + CountSuffix(t.Minutes);
            if (t.Seconds != 0) s += " " + t.Seconds + " second" + CountSuffix(t.Seconds);
            if (t.Milliseconds != 0 && includeMs) s += " " + t.Milliseconds + " millisecond" + CountSuffix(t.Milliseconds);

            if (s == "")
                if (includeMs) s = "0 milliseconds";
                else s = "0 seconds";

            return s.Trim();
        }
        static string CountSuffix(long c) => c % 10 != 1 ? "s" : "";
        public static string Remove(this string s, string toRemove)
        {
            if (toRemove == "") return s;
            return s.Replace(toRemove, "");
        }

        public static string LoremIpsum(int words = 2)
        {
            var s = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur Excepteur sint occaecat cupidatat non proident sunt in culpa qui officia deserunt mollit anim id est laborum";
            var ws = s.Split(' ').Select(r => r.ToLower().Trim(new[] { ',', '.' }));
            ws = ws.OrderBy(r => UnityEngine.Random.Range(0, 1232)).Take(words);
            var ss = string.Join(" ", ws);
            return char.ToUpper(ss[0]) + ss.Substring(1);
        }

        public static string Decamelcase(this string str) => Regex.Replace(Regex.Replace(str, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2"), @"(\p{Ll})(\P{Ll})", "$1 $2");
        public static string PrettifyVarName(this string str, bool lowercaseFollowingWords = true) => string.Join(" ", str.Decamelcase().Split(' ').Select(r => new[] { "", "and", "or", "with", "without", "by", "from" }.Contains(r.ToLower()) || (lowercaseFollowingWords && !str.Trim().StartsWith(r)) ? r.ToLower() : r.Substring(0, 1).ToUpper() + r.Substring(1))).Trim(' ');
        // public static string RemoveParenthesized(this string s) => Regex.Replace(s, @"/\().*?\) /g", "").Trim();



        #endregion

        #region IEnumerables

        public static T AddAt<T>(this List<T> l, int i, T r)
        {
            if (i < 0) i = 0;
            if (i >= l.Count)
                l.Add(r);
            else
                l.Insert(i, r);
            return r;
        }
        public static T RemoveLast<T>(this List<T> l)
        {
            if (!l.Any()) return default;

            var r = l.Last();

            l.RemoveAt(l.Count - 1);

            return r;
        }

        public static void Add<T>(this List<T> list, params T[] items)
        {
            foreach (var r in items)
                list.Add(r);
        }

        public static int LastIndex<T>(this List<T> l) => l.Count - 1;

        public static T GetAtWrapped<T>(this List<T> list, int i)
        {
            while (i < 0) i += list.Count;
            while (i >= list.Count) i -= list.Count;

            return list[i];
        }

        #endregion

        #region Linq

        public static T NextTo<T>(this IEnumerable<T> e, T to) => e.SkipWhile(r => !r.Equals(to)).Skip(1).FirstOrDefault();
        public static T PreviousTo<T>(this IEnumerable<T> e, T to) => e.Reverse().SkipWhile(r => !r.Equals(to)).Skip(1).FirstOrDefault();

        public static Dictionary<T1, T2> MergeDictionaries<T1, T2>(IEnumerable<Dictionary<T1, T2>> dicts)
        {
            if (dicts.Count() == 0) return null;
            if (dicts.Count() == 1) return dicts.First();

            var mergedDict = new Dictionary<T1, T2>(dicts.First());

            foreach (var dict in dicts.Skip(1))
                foreach (var r in dict)
                    if (!mergedDict.ContainsKey(r.Key))
                        mergedDict.Add(r.Key, r.Value);

            return mergedDict;
        }

        public static IEnumerable<T> InsertFirst<T>(this IEnumerable<T> ie, T t) => new[] { t }.Concat(ie);

        public static bool None<T>(this IEnumerable<T> ie, System.Func<T, bool> f) => !ie.Any(f);


        public static void SortBy<T, T2>(this List<T> list, System.Func<T, T2> keySelector) where T2 : System.IComparable => list.Sort((q, w) => keySelector(q).CompareTo(keySelector(w)));

        #endregion

        #region Reflection

        public static object GetFieldValue(this object o, string fieldName, bool exceptionIfNotFound = true)
        {
            var type = (o as Type) ?? o.GetType();
            var target = o is Type ? null : o;


            if (type.GetFieldInfo(fieldName) is FieldInfo fieldInfo)
                return fieldInfo.GetValue(target);


            if (exceptionIfNotFound)
                throw new System.Exception($"Field '{fieldName}' not found in '{type.Name}' type and its parent types");

            return null;

        }
        public static object GetPropertyValue(this object o, string propertyName, bool exceptionIfNotFound = true)
        {
            var type = (o as Type) ?? o.GetType();
            var target = o is Type ? null : o;


            if (type.GetPropertyInfo(propertyName) is PropertyInfo propertyInfo)
                return propertyInfo.GetValue(target);


            if (exceptionIfNotFound)
                throw new System.Exception($"Property '{propertyName}' not found in '{type.Name}' type and its parent types");

            return null;

        }
        public static object GetMemberValue(this object o, string memberName, bool exceptionIfNotFound = true)
        {
            var type = (o as Type) ?? o.GetType();
            var target = o is Type ? null : o;


            if (type.GetFieldInfo(memberName) is FieldInfo fieldInfo)
                return fieldInfo.GetValue(target);

            if (type.GetPropertyInfo(memberName) is PropertyInfo propertyInfo)
                return propertyInfo.GetValue(target);


            if (exceptionIfNotFound)
                throw new System.Exception($"Member '{memberName}' not found in '{type.Name}' type and its parent types");

            return null;

        }

        public static void SetFieldValue(this object o, string fieldName, object value, bool exceptionIfNotFound = true)
        {
            var type = (o as Type) ?? o.GetType();
            var target = o is Type ? null : o;


            if (type.GetFieldInfo(fieldName) is FieldInfo fieldInfo)
                fieldInfo.SetValue(target, value);


            else if (exceptionIfNotFound)
                throw new System.Exception($"Field '{fieldName}' not found in '{type.Name}' type and its parent types");

        }
        public static void SetPropertyValue(this object o, string propertyName, object value, bool exceptionIfNotFound = true)
        {
            var type = (o as Type) ?? o.GetType();
            var target = o is Type ? null : o;


            if (type.GetPropertyInfo(propertyName) is PropertyInfo propertyInfo)
                propertyInfo.SetValue(target, value);


            else if (exceptionIfNotFound)
                throw new System.Exception($"Property '{propertyName}' not found in '{type.Name}' type and its parent types");

        }
        public static void SetMemberValue(this object o, string memberName, object value, bool exceptionIfNotFound = true)
        {
            var type = (o as Type) ?? o.GetType();
            var target = o is Type ? null : o;


            if (type.GetFieldInfo(memberName) is FieldInfo fieldInfo)
                fieldInfo.SetValue(target, value);

            else if (type.GetPropertyInfo(memberName) is PropertyInfo propertyInfo)
                propertyInfo.SetValue(target, value);


            else if (exceptionIfNotFound)
                throw new System.Exception($"Member '{memberName}' not found in '{type.Name}' type and its parent types");

        }

        public static object InvokeMethod(this object o, string methodName, params object[] parameters) // todo handle null params (can't get their type)
        {
            var type = (o as Type) ?? o.GetType();
            var target = o is Type ? null : o;


            if (type.GetMethodInfo(methodName, parameters.Select(r => r.GetType()).ToArray()) is MethodInfo methodInfo)
                return methodInfo.Invoke(target, parameters);


            throw new System.Exception($"Method '{methodName}' not found in '{type.Name}' type, its parent types and interfaces");

        }



        static FieldInfo GetFieldInfo(this Type type, string fieldName)
        {
            if (fieldInfoCache.TryGetValue(type, out var fieldInfosByNames))
                if (fieldInfosByNames.TryGetValue(fieldName, out var fieldInfo))
                    return fieldInfo;


            if (!fieldInfoCache.ContainsKey(type))
                fieldInfoCache[type] = new Dictionary<string, FieldInfo>();

            for (var curType = type; curType != null; curType = curType.BaseType)
                if (curType.GetField(fieldName, maxBindingFlags) is FieldInfo fieldInfo)
                    return fieldInfoCache[type][fieldName] = fieldInfo;


            return fieldInfoCache[type][fieldName] = null;

        }
        static Dictionary<Type, Dictionary<string, FieldInfo>> fieldInfoCache = new Dictionary<Type, Dictionary<string, FieldInfo>>();

        static PropertyInfo GetPropertyInfo(this Type type, string propertyName)
        {
            if (propertyInfoCache.TryGetValue(type, out var propertyInfosByNames))
                if (propertyInfosByNames.TryGetValue(propertyName, out var propertyInfo))
                    return propertyInfo;


            if (!propertyInfoCache.ContainsKey(type))
                propertyInfoCache[type] = new Dictionary<string, PropertyInfo>();

            for (var curType = type; curType != null; curType = curType.BaseType)
                if (curType.GetProperty(propertyName, maxBindingFlags) is PropertyInfo propertyInfo)
                    return propertyInfoCache[type][propertyName] = propertyInfo;


            return propertyInfoCache[type][propertyName] = null;

        }
        static Dictionary<Type, Dictionary<string, PropertyInfo>> propertyInfoCache = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

        static MethodInfo GetMethodInfo(this Type type, string methodName, params Type[] argumentTypes)
        {
            var methodHash = methodName.GetHashCode() ^ argumentTypes.Aggregate(0, (hash, r) => hash ^= r.GetHashCode());


            if (methodInfoCache.TryGetValue(type, out var methodInfosByHashes))
                if (methodInfosByHashes.TryGetValue(methodHash, out var methodInfo))
                    return methodInfo;



            if (!methodInfoCache.ContainsKey(type))
                methodInfoCache[type] = new Dictionary<int, MethodInfo>();

            for (var curType = type; curType != null; curType = curType.BaseType)
                if (curType.GetMethod(methodName, maxBindingFlags, null, argumentTypes, null) is MethodInfo methodInfo)
                    return methodInfoCache[type][methodHash] = methodInfo;

            foreach (var interfaceType in type.GetInterfaces())
                if (interfaceType.GetMethod(methodName, maxBindingFlags, null, argumentTypes, null) is MethodInfo methodInfo)
                    return methodInfoCache[type][methodHash] = methodInfo;



            return methodInfoCache[type][methodHash] = null;

        }
        static Dictionary<Type, Dictionary<int, MethodInfo>> methodInfoCache = new Dictionary<Type, Dictionary<int, MethodInfo>>();



        public static T GetFieldValue<T>(this object o, string fieldName, bool exceptionIfNotFound = true) => (T)o.GetFieldValue(fieldName, exceptionIfNotFound);
        public static T GetPropertyValue<T>(this object o, string propertyName, bool exceptionIfNotFound = true) => (T)o.GetPropertyValue(propertyName, exceptionIfNotFound);
        public static T GetMemberValue<T>(this object o, string memberName, bool exceptionIfNotFound = true) => (T)o.GetMemberValue(memberName, exceptionIfNotFound);
        public static T InvokeMethod<T>(this object o, string methodName, params object[] parameters) => (T)o.InvokeMethod(methodName, parameters);






        public static List<Type> GetSubclasses(this Type t) => t.Assembly.GetTypes().Where(type => type.IsSubclassOf(t)).ToList();

        public static object GetDefaultValue(this FieldInfo f, params object[] constructorVars) => f.GetValue(System.Activator.CreateInstance(((MemberInfo)f).ReflectedType, constructorVars));

        public static object GetDefaultValue(this FieldInfo f) => f.GetValue(System.Activator.CreateInstance(((MemberInfo)f).ReflectedType));


        public static IEnumerable<FieldInfo> GetFieldsWithoutBase(this Type t) => t.GetFields().Where(r => !t.BaseType.GetFields().Any(rr => rr.Name == r.Name));

        public static IEnumerable<PropertyInfo> GetPropertiesWithoutBase(this Type t) => t.GetProperties().Where(r => !t.BaseType.GetProperties().Any(rr => rr.Name == r.Name));


        public const BindingFlags maxBindingFlags = (BindingFlags)62;







        #endregion

        #region Math

        public static bool Approx(this float f1, float f2) => Mathf.Approximately(f1, f2);
        public static bool CloseTo(this float f1, float f2, float distance) => f1.DistTo(f2) <= distance;
        public static float DistTo(this float f1, float f2) => Mathf.Abs(f1 - f2);
        public static float Dist(float f1, float f2) => Mathf.Abs(f1 - f2);
        public static float Avg(float f1, float f2) => (f1 + f2) / 2;
        public static float Abs(this float f) => Mathf.Abs(f);
        public static int Abs(this int f) => Mathf.Abs(f);
        public static float Sign(this float f) => Mathf.Sign(f);
        public static float Clamp(this float f, float f0, float f1) => Mathf.Clamp(f, f0, f1);
        public static float Clamp01(this float f) => Mathf.Clamp(f, 0, 1);
        public static Vector2 Clamp01(this Vector2 f) => new Vector2(f.x.Clamp01(), f.y.Clamp01());
        public static Vector3 Clamp01(this Vector3 f) => new Vector3(f.x.Clamp01(), f.y.Clamp01(), f.z.Clamp01());


        public static float Pow(this float f, float pow) => Mathf.Pow(f, pow);
        public static int Pow(this int f, int pow) => (int)Mathf.Pow(f, pow);

        public static float Round(this float f) => Mathf.Round(f);
        public static float Ceil(this float f) => Mathf.Ceil(f);
        public static float Floor(this float f) => Mathf.Floor(f);
        public static int RoundToInt(this float f) => Mathf.RoundToInt(f);
        public static int CeilToInt(this float f) => Mathf.CeilToInt(f);
        public static int FloorToInt(this float f) => Mathf.FloorToInt(f);


        public static float Sqrt(this float f) => Mathf.Sqrt(f);

        public static float Max(this float f, float ff) => Mathf.Max(f, ff);
        public static float Max(this int f, int ff) => Mathf.Max(f, ff);
        public static float Min(this float f, float ff) => Mathf.Min(f, ff);
        public static float Min(this int f, int ff) => Mathf.Min(f, ff);

        public static float Loop(this float f, float boundMin, float boundMax)
        {
            while (f < boundMin) f += boundMax - boundMin;
            while (f > boundMax) f -= boundMax - boundMin;
            return f;
        }
        public static float Loop(this float f, float boundMax) => f.Loop(0, boundMax);

        public static float TriangleArea(Vector2 A, Vector2 B, Vector2 C) => Vector3.Cross(A - B, A - C).z.Abs() / 2;
        public static Vector2 LineIntersection(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
        {
            var a1 = B.y - A.y;
            var b1 = A.x - B.x;
            var c1 = a1 * A.x + b1 * A.y;

            var a2 = D.y - C.y;
            var b2 = C.x - D.x;
            var c2 = a2 * C.x + b2 * C.y;

            var d = a1 * b2 - a2 * b1;

            var x = (b2 * c1 - b1 * c2) / d;
            var y = (a1 * c2 - a2 * c1) / d;

            return new Vector2(x, y);

        }

        public static float ProjectOn(this Vector2 v, Vector2 on) => Vector3.Project(v, on).magnitude;
        public static float AngleTo(this Vector2 v, Vector2 to) => Vector2.Angle(v, to);

        public static Vector2 Rotate(this Vector2 v, float deg) => Quaternion.AngleAxis(deg, Vector3.forward) * v;

        public static float Lerp(float f1, float f2, float t) => Mathf.LerpUnclamped(f1, f2, t);
        public static float Lerp(ref float f1, float f2, float t) => f1 = Lerp(f1, f2, t);
        public static Vector3 Lerp(Vector3 f1, Vector3 f2, float t) => Vector3.LerpUnclamped(f1, f2, t);

        public static float InverseLerp(this Vector2 v, Vector2 a, Vector2 b)
        {
            var ab = b - a;
            var av = v - a;
            return Vector2.Dot(av, ab) / Vector2.Dot(ab, ab);
        }

        public static bool IsOdd(this int i) => i % 2 == 1;
        public static bool IsEven(this int i) => i % 2 == 0;

        public static bool IsInRange(this int i, int a, int b) => i >= a && i <= b;
        public static bool IsInRange(this float i, float a, float b) => i >= a && i <= b;

        public static bool IsInRangeOf(this int i, IList list) => i.IsInRange(0, list.Count - 1);
        public static bool IsInRangeOf<T>(this int i, T[] array) => i.IsInRange(0, array.Length - 1);



        public static float LerpT(float lerpSpeed, float dt) => 1 - Mathf.Exp(-lerpSpeed * 18 * dt);
        public static float LerpT(float lerpSpeed) => LerpT(lerpSpeed, Time.unscaledDeltaTime);

        #endregion

        #region Colors

        public static Color HSLToRGB(float h, float s, float l)
        {
            float hue2Rgb(float v1, float v2, float vH)
            {
                if (vH < 0f)
                    vH += 1f;

                if (vH > 1f)
                    vH -= 1f;

                if (6f * vH < 1f)
                    return v1 + (v2 - v1) * 6f * vH;

                if (2f * vH < 1f)
                    return v2;

                if (3f * vH < 2f)
                    return v1 + (v2 - v1) * (2f / 3f - vH) * 6f;

                return v1;
            }

            if (s.Approx(0)) return new Color(l, l, l);

            float k1;

            if (l < .5f)
                k1 = l * (1f + s);
            else
                k1 = l + s - s * l;


            var k2 = 2f * l - k1;

            float r, g, b;
            r = hue2Rgb(k2, k1, h + 1f / 3);
            g = hue2Rgb(k2, k1, h);
            b = hue2Rgb(k2, k1, h - 1f / 3);

            return new Color(r, g, b);
        }

        public static Color Greyscale(float s, float a = 1) { var c = Color.white; c *= s; c.a = a; return c; }

        #endregion

        #region Rects

        public static Rect Resize(this Rect rect, float px) { rect.x += px; rect.y += px; rect.width -= px * 2; rect.height -= px * 2; return rect; }

        public static Rect SetPos(this Rect rect, Vector2 v) => rect.SetPos(v.x, v.y);
        public static Rect SetPos(this Rect rect, float x, float y) { rect.x = x; rect.y = y; return rect; }

        public static Rect SetX(this Rect rect, float x) => rect.SetPos(x, rect.y);
        public static Rect SetY(this Rect rect, float y) => rect.SetPos(rect.x, y);
        public static Rect SetXMax(this Rect rect, float xMax) { rect.xMax = xMax; return rect; }
        public static Rect SetYMax(this Rect rect, float yMax) { rect.yMax = yMax; return rect; }

        public static Rect SetMidPos(this Rect r, Vector2 v) => r.SetPos(v).MoveX(-r.width / 2).MoveY(-r.height / 2);

        public static Rect Move(this Rect rect, Vector2 v) { rect.position += v; return rect; }
        public static Rect Move(this Rect rect, float x, float y) { rect.x += x; rect.y += y; return rect; }
        public static Rect MoveX(this Rect rect, float px) { rect.x += px; return rect; }
        public static Rect MoveY(this Rect rect, float px) { rect.y += px; return rect; }

        public static Rect SetWidth(this Rect rect, float f) { rect.width = f; return rect; }
        public static Rect SetWidthFromMid(this Rect rect, float px) { rect.x += rect.width / 2; rect.width = px; rect.x -= rect.width / 2; return rect; }
        public static Rect SetWidthFromRight(this Rect rect, float px) { rect.x += rect.width; rect.width = px; rect.x -= rect.width; return rect; }

        public static Rect SetHeight(this Rect rect, float f) { rect.height = f; return rect; }
        public static Rect SetHeightFromMid(this Rect rect, float px) { rect.y += rect.height / 2; rect.height = px; rect.y -= rect.height / 2; return rect; }
        public static Rect SetHeightFromBottom(this Rect rect, float px) { rect.y += rect.height; rect.height = px; rect.y -= rect.height; return rect; }

        public static Rect AddWidth(this Rect rect, float f) => rect.SetWidth(rect.width + f);
        public static Rect AddWidthFromMid(this Rect rect, float f) => rect.SetWidthFromMid(rect.width + f);
        public static Rect AddWidthFromRight(this Rect rect, float f) => rect.SetWidthFromRight(rect.width + f);

        public static Rect AddHeight(this Rect rect, float f) => rect.SetHeight(rect.height + f);
        public static Rect AddHeightFromMid(this Rect rect, float f) => rect.SetHeightFromMid(rect.height + f);
        public static Rect AddHeightFromBottom(this Rect rect, float f) => rect.SetHeightFromBottom(rect.height + f);

        public static Rect SetSize(this Rect rect, Vector2 v) => rect.SetWidth(v.x).SetHeight(v.y);
        public static Rect SetSize(this Rect rect, float w, float h) => rect.SetWidth(w).SetHeight(h);
        public static Rect SetSize(this Rect rect, float f) { rect.height = rect.width = f; return rect; }

        public static Rect SetSizeFromMid(this Rect r, Vector2 v) => r.Move(r.size / 2).SetSize(v).Move(-v / 2);
        public static Rect SetSizeFromMid(this Rect r, float x, float y) => r.SetSizeFromMid(new Vector2(x, y));
        public static Rect SetSizeFromMid(this Rect r, float f) => r.SetSizeFromMid(new Vector2(f, f));




        #endregion

        #region AssetDatabase
#if UNITY_EDITOR

        public static string ToPath(this string guid) => AssetDatabase.GUIDToAssetPath(guid);
        public static List<string> ToPaths(this IEnumerable<string> guids) => guids.Select(r => r.ToPath()).ToList();


        public static string ToGuid(this string pathInProject) => AssetDatabase.AssetPathToGUID(pathInProject);
        public static List<string> ToGuids(this IEnumerable<string> pathsInProject) => pathsInProject.Select(r => r.ToGuid()).ToList();

        public static string GetPath(this Object o) => AssetDatabase.GetAssetPath(o);
        public static string GetGuid(this Object o) => AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(o));

        public static string GetScriptPath(string scriptName) => AssetDatabase.FindAssets("t: script " + scriptName, null).FirstOrDefault()?.ToPath() ?? "scirpt not found";


#endif


        #endregion

        #region Editor
#if UNITY_EDITOR

        public static void ToggleDefineDisabledInScript(Type scriptType)
        {
            var path = GetScriptPath(scriptType.Name);

            var lines = File.ReadAllLines(path);
            if (lines.First().StartsWith("#define DISABLED"))
                File.WriteAllLines(path, lines.Skip(1));
            else
                File.WriteAllLines(path, lines.Prepend("#define DISABLED    // this line was added by VUtils.ToggleDefineDisabledInScript"));

            AssetDatabase.ImportAsset(path);
        }
        public static bool ScriptHasDefineDisabled(Type scriptType) => File.ReadLines(GetScriptPath(scriptType.Name)).First().StartsWith("#define DISABLED");
        public static void SetDefineDisabledInScript(Type scriptType, bool defineDisabled)
        {
            if (ScriptHasDefineDisabled(scriptType) != defineDisabled)
                ToggleDefineDisabledInScript(scriptType);

        }

        public static int GetProjectId() => Application.dataPath.GetHashCode();

        public static void PingObject(Object o, bool select = false, bool focusProjectWindow = true)
        {
            if (select)
            {
                Selection.activeObject = null;
                Selection.activeObject = o;
            }
            if (focusProjectWindow) EditorUtility.FocusProjectWindow();
            EditorGUIUtility.PingObject(o);

        }
        public static void PingObject(string guid, bool select = false, bool focusProjectWindow = true) => PingObject(AssetDatabase.LoadAssetAtPath<Object>(guid.ToPath()));


        public static void OpenFolder(string path)
        {
            var folder = AssetDatabase.LoadAssetAtPath(path, typeof(Object));

            var t = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
            var w = (EditorWindow)t.GetField("s_LastInteractedProjectBrowser").GetValue(null);

            var m_ListAreaState = t.GetField("m_ListAreaState", maxBindingFlags).GetValue(w);

            m_ListAreaState.GetType().GetField("m_SelectedInstanceIDs").SetValue(m_ListAreaState, new List<int> { folder.GetInstanceID() });

            t.GetMethod("OpenSelectedFolders", maxBindingFlags).Invoke(null, null);

        }

        public static void Dirty(this Object o) => UnityEditor.EditorUtility.SetDirty(o);
        // public static void Save(this Object o) => AssetDatabase.SaveAssetIfDirty(o);
        public static void RecordUndo(this Object o)
        {
            // if (o is RenderTexture)
            // RenderTextureUndo.RecordUndo(o as RenderTexture);
            // else
            Undo.RecordObject(o, "");
        }
        // public static void RecordUndo(this RenderTexture renderTexture, Rect rect, System.Action onUndoRedo = null) => RenderTextureUndo.RecordUndo(renderTexture, rect, onUndoRedo);
        // public static void RecordUndo(this RenderTexture renderTexture, System.Action onUndoRedo = null) => renderTexture.RecordUndo(new Rect(0, 0, renderTexture.width, renderTexture.height), onUndoRedo);

#endif


        #endregion

    }

    public static class VGUI
    {
        #region Controls

        public static T Field<T>(Rect rect, string name, T cur)
        {
            if (typeof(Object).IsAssignableFrom(typeof(T)))
                return (T)(object)EditorGUI.ObjectField(rect, name, cur as Object, typeof(T), true);

            if (typeof(T) == typeof(float))
                return (T)(object)EditorGUI.FloatField(rect, name, (float)(object)cur);

            if (typeof(T) == typeof(int))
                return (T)(object)EditorGUI.IntField(rect, name, (int)(object)cur);

            if (typeof(T) == typeof(bool))
                return (T)(object)EditorGUI.Toggle(rect, name, (bool)(object)cur);

            if (typeof(T) == typeof(string))
                return (T)(object)EditorGUI.TextField(rect, name, (string)(object)cur);

            return default;
        }
        public static T Field<T>(string name, T cur) => Field(ExpandWidthLabelRect(), name, cur);
        public static void Field<T>(Rect rect, string name, ref T cur) => cur = Field(rect, name, cur);
        public static void Field<T>(string name, ref T cur) => cur = Field(name, cur);

        public static T Slider<T>(Rect rect, string name, T cur, T min, T max)
        {
            if (typeof(T) == typeof(float))
                return (T)(object)EditorGUI.Slider(rect, name, (float)(object)cur, (float)(object)min, (float)(object)max);

            if (typeof(T) == typeof(int))
                return (T)(object)EditorGUI.IntSlider(rect, name, (int)(object)cur, (int)(object)min, (int)(object)max);

            return default;
        }
        public static T Slider<T>(string name, T cur, T min, T max) => Slider(ExpandWidthLabelRect(), name, cur, min, max);
        public static void Slider<T>(Rect rect, string name, ref T cur, T min, T max) => cur = Slider(rect, name, cur, min, max);
        public static void Slider<T>(string name, ref T cur, T min, T max) => cur = Slider(name, cur, min, max);



        public static bool _ResetFieldButton(Rect rect, bool isObjectField = false)
        {
            var prev = GUI.color;
            GUI.color = Color.clear;
            var r = GUI.Button(rect.SetWidthFromRight(20).MoveX(isObjectField ? -18 : 0), "");
            GUI.color = prev;
            return r;
        }
        public static void _DrawResettableFieldCrossIcon(Rect rect, bool isObjectField = false, float brightness = .36f)
        {
            var iconRect = rect.SetWidthFromRight(20).SetSizeFromMid(15).MoveX(isObjectField ? -18 : 0).MoveY(.5f);

            if (iconRect.Resize(-3).IsHovered())
                brightness = EditorGUIUtility.isProSkin ? .8f : .65f;

            if (isObjectField)
            {
                var fieldBg = EditorGUIUtility.isProSkin ? Greyscale(.152f) : Greyscale(.93f);
                iconRect.MoveX(-2).SetWidth(19).Draw(fieldBg);
            }

            SetGUIColor(Greyscale(brightness));
            GUI.Label(iconRect, EditorGUIUtility.IconContent("CrossIcon"));
            ResetGUIColor();

        }

        public static T ResettableField<T>(Rect rect, string name, T cur, T resetTo = default)
        {
            var isObjectField = typeof(Object).IsAssignableFrom(typeof(T));
            var reset = _ResetFieldButton(rect, isObjectField);

            cur = Field(rect, name, cur);

            if (!object.Equals(cur, resetTo))
                _DrawResettableFieldCrossIcon(rect, isObjectField);

            return reset ? resetTo : cur;
        }
        public static T ResettableField<T>(string name, T cur, T resetTo = default) => ResettableField(ExpandWidthLabelRect(), name, cur, resetTo);
        public static void ResettableField<T>(Rect rect, string name, ref T cur, T resetTo = default) => cur = ResettableField(rect, name, cur, resetTo);
        public static void ResettableField<T>(string name, ref T cur, T resetTo = default) => cur = ResettableField(name, cur, resetTo);

        public static T ResettableSlider<T>(Rect rect, string name, T cur, T min, T max, T resetTo = default)
        {
            var reset = _ResetFieldButton(lastRect);

            cur = Slider(rect, name, cur, min, max);

            if (!object.Equals(cur, resetTo))
                _DrawResettableFieldCrossIcon(lastRect);

            return reset ? resetTo : cur;
        }
        public static T ResettableSlider<T>(string name, T cur, T min, T max, T resetTo = default) => ResettableSlider(ExpandWidthLabelRect(), name, cur, min, max, resetTo);
        public static void ResettableSlider<T>(Rect rect, string name, ref T cur, T min, T max, T resetTo = default) => cur = ResettableSlider(rect, name, cur, min, max, resetTo);
        public static void ResettableSlider<T>(string name, ref T cur, T min, T max, T resetTo = default) => cur = ResettableSlider(name, cur, min, max, resetTo);





        public static string Tabs(string current, bool equalButtonSizes = true, float rowHeight = 24, params string[] variants)
        {
            GUILayout.BeginHorizontal();

            for (int i = 0; i < variants.Length; i++)
            {
                GUI.backgroundColor = variants[i] == current ? pressedButtonCol : Color.white;
                bool b;

                if (variants[i] == "Settings" && i == variants.Length - 1)
                    b = GUILayout.Button(EditorGUIUtility.IconContent("Settings"), headerHeight, GUILayout.Width(26));

                else if (variants[i] == "More" && i == variants.Length - 1)
                    b = GUILayout.Button(EditorGUIUtility.IconContent("more"), headerHeight, GUILayout.Width(28));

                else if (equalButtonSizes)
                    b = ButtonFixedSize(variants[i], rowHeight);
                else
                    b = Button(variants[i], rowHeight);

                if (b) current = variants[i];

                GUI.backgroundColor = Color.white;


                if (i != variants.Length - 1) GUILayout.Space(-6f);

            }
            GUILayout.EndHorizontal();

            return current;
        }
        public static string TabsMultiRow(string current, bool equalButtonSizes = true, float rowHeight = 24, params string[] variants)
        {
            float textWidth(string text)
            {
                if (text == "Settings" || text == "More")
                    return 22;
                else
                    return text.GetLabelWidth();
            }

            var spaceBetweenTexts = 5;

            var maxWidth = GetCurrentInspectorWidth() - 22;
            var totalWidth = variants.Sum(r => textWidth(r) + spaceBetweenTexts);
            var rowsN = (totalWidth / maxWidth).CeilToInt();
            var rowWidth = totalWidth / rowsN;

            var rows = new List<List<string>>();
            var curRow = new List<string>();
            rows.Add(curRow);

            var curRowWidth = 0f;

            for (int i = 0; i < variants.Length; i++)
            {
                void nextRow()
                {
                    curRowWidth = 0;
                    curRow = new List<string>();
                    rows.Add(curRow);
                }


                var widthToAdd = textWidth(variants[i]) + spaceBetweenTexts;

                if (curRowWidth + widthToAdd > maxWidth && curRow.Any())
                    nextRow();

                curRow.Add(variants[i]);
                curRowWidth += widthToAdd;

                if (curRowWidth > rowWidth && i != variants.Length - 1)
                    nextRow();

            }


            foreach (var row in rows)
            {
                current = Tabs(current, equalButtonSizes, rowHeight, row.ToArray());
                Space(-3);
            }

            Space(3);


            return current;

        }

        public static bool Button(string text = "") => GUILayout.Button(text);
        public static bool Button(string text, float height = headerHeightF) => GUILayout.Button(text, GUILayout.Height(height));
        public static bool ButtonFixedSize(string text, float height = headerHeightF)
        {
            GUILayout.Label("", GUILayout.Height(height));

            var b = GUI.Button(lastRect, "");

            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.Label(lastRect, text);
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;

            return b;
        }
        public static bool SettingsButton(float height = headerHeightF) => GUILayout.Button(EditorGUIUtility.IconContent("Settings"), headerHeight, GUILayout.Width(26));


        public static bool Foldout(string name, bool val)
        {
            EditorGUILayout.PrefixLabel(name);
            val = EditorGUI.Foldout(lastRect, val, "");

            if (lastRect.IsHovered() && e.mouseDown())
                val = !val;

            return val;
        }



        #endregion

        #region Colors

        public static Color accentColor => Color.white * .81f;
        public static Color dividerColor => EditorGUIUtility.isProSkin ? Color.white * .42f : Color.white * .72f;
        public static Color treeViewBG => EditorGUIUtility.isProSkin ? new Color(.22f, .22f, .22f, .56f) : new Color(.7f, .7f, .7f, .1f);
        public static Color greyedOutColor = Color.white * .72f;

        public static Color objFieldCol = Color.black + Color.white * .16f;
        public static Color objFieldClearCrossCol = Color.white * .5f;
        public static Color objPieckerCol = Color.black + Color.white * .21f;

        public static Color dragndropTintHovered = Greyscale(.8f, .07f);

        public static Color footerCol = Greyscale(.26f);

        // public static Color pressedCol = new Color(.4f, .7f, 1f, .4f) * 1.65f;
        public static Color pressedCol = new Color(.35f, .57f, 1f, 1f) * 1.25f;// * 1.65f;
        public static Color pressedButtonCol => EditorGUIUtility.isProSkin ? new Color(.48f, .76f, 1f, 1f) * 1.4f : new Color(.48f, .7f, 1f, 1f) * 1.2f;

        public static Color hoveredCol = Greyscale(.5f, .15f);

        public static Color rowCol = Greyscale(.28f, .28f);

        public static Color backgroundCol => EditorGUIUtility.isProSkin ? Greyscale(.22f) : Greyscale(.78f);
        public static Color backgroundLightCol = Greyscale(.255f);

        public static Color selectedCol = new Color(.15f, .4f, 1f, .7f);// * 1.65f;

        public static Color buttonCol = Greyscale(.33f);




        #endregion

        #region Events

        public struct WrappedEvent
        {
            public Event e;

            public bool isNull => e == null;
            public bool isRepaint => isNull ? default : e.type == EventType.Repaint;
            public bool isLayout => isNull ? default : e.type == EventType.Layout;
            public bool isUsed => isNull ? default : e.type == EventType.Used;
            public bool isMouseLeaveWindow => isNull ? default : e.type == EventType.MouseLeaveWindow;
            public bool isMouseEnterWindow => isNull ? default : e.type == EventType.MouseEnterWindow;
            public bool isContextClick => isNull ? default : e.type == EventType.ContextClick;

            public bool isKeyDown => isNull ? default : e.type == EventType.KeyDown;
            public bool isKeyUp => isNull ? default : e.type == EventType.KeyUp;
            public KeyCode keyCode => isNull ? default : e.keyCode;
            public char characted => isNull ? default : e.character;

            public bool isExecuteCommand => isNull ? default : e.type == EventType.ExecuteCommand;
            public string commandName => isNull ? default : e.commandName;

            public bool isMouse => isNull ? default : e.isMouse;
            public bool isMouseDown => isNull ? default : e.type == EventType.MouseDown;
            public bool isMouseUp => isNull ? default : e.type == EventType.MouseUp;
            public bool isMouseDrag => isNull ? default : e.type == EventType.MouseDrag;
            public bool isMouseMove => isNull ? default : e.type == EventType.MouseMove;
            public bool isScroll => isNull ? default : e.type == EventType.ScrollWheel;
            public int mouseButton => isNull ? default : e.button;
            public int clickCount => isNull ? default : e.clickCount;
            public Vector2 mousePosition => isNull ? default : e.mousePosition;
            public Vector2 mousePosition_screenSpace => isNull ? default : GUIUtility.GUIToScreenPoint(e.mousePosition);
            public Vector2 mouseDelta => isNull ? default : e.delta;

            public bool isDragUpdate => isNull ? default : e.type == EventType.DragUpdated;
            public bool isDragPerform => isNull ? default : e.type == EventType.DragPerform;
            public bool isDragExit => isNull ? default : e.type == EventType.DragExited;

            public EventModifiers modifiers => isNull ? default : e.modifiers;
            public bool holdingAnyModifierKey => modifiers != EventModifiers.None;

            public bool holdingAlt => isNull ? default : e.alt;
            public bool holdingShift => isNull ? default : e.shift;
            public bool holdingCtrl => isNull ? default : e.control;
            public bool holdingCmd => isNull ? default : e.command;
            public bool holdingCmdOrCtrl => isNull ? default : e.command || e.control;

            public bool holdingAltOnly => isNull ? default : e.modifiers == EventModifiers.Alt;        // in some sessions FunctionKey is always pressed?
            public bool holdingShiftOnly => isNull ? default : e.modifiers == EventModifiers.Shift;        // in some sessions FunctionKey is always pressed?
            public bool holdingCtrlOnly => isNull ? default : e.modifiers == EventModifiers.Control;
            public bool holdingCmdOnly => isNull ? default : e.modifiers == EventModifiers.Command;
            public bool holdingCmdOrCtrlOnly => isNull ? default : (e.modifiers == EventModifiers.Command || e.modifiers == EventModifiers.Control);

            public EventType type => e.type;

            public void Use() => e?.Use();


            public WrappedEvent(Event e) => this.e = e;

            public override string ToString() => e.ToString();

        }
        public static WrappedEvent Wrap(this Event e) => new WrappedEvent(e);
        public static WrappedEvent curEvent => (Event.current ?? _fi_s_Current.GetValue(null) as Event).Wrap(); // todo no reflection?
        static FieldInfo _fi_s_Current = typeof(Event).GetField("s_Current", maxBindingFlags);





        public static Event e => Event.current;
        public static bool ePresent => Event.current != null;
        public static UnityEngine.EventType eType => ePresent ? e.type : UnityEngine.EventType.Ignore;
        public static bool mouseDown(this Event e) => eType == EventType.MouseDown && e.button == 0;
        public static bool mouseUp(this Event e) => eType == EventType.MouseUp && e.button == 0;
        public static bool keyDown(this Event e) => eType == EventType.KeyDown;
        public static bool keyUp(this Event e) => eType == EventType.KeyUp;


        public static bool holdingAlt => ePresent && (e.alt);
        public static bool holdingCmd => ePresent && (e.command || e.control);
        public static bool holdingShift => ePresent && (e.shift);





        #endregion

        #region Shortcuts

        public static Rect lastRect => GUILayoutUtility.GetLastRect();

        public static float GetLabelWidth(this string s, bool isBold = false)
        {
            var prev = GUI.skin.label.fontStyle;

            GUI.skin.label.fontStyle = isBold ? FontStyle.Bold : FontStyle.Normal;

            var r = GUI.skin.label.CalcSize(new GUIContent(s)).x;

            GUI.skin.label.fontStyle = prev;

            return r;
        }
        public static float GetCurrentInspectorWidth() => typeof(EditorGUIUtility).GetPropertyValue<float>("contextWidth");

        public static bool IsHovered(this Rect r) => ePresent && r.Contains(e.mousePosition);


        public static void SetGUIEnabled(bool enabled) { _prevGuiEnabled = GUI.enabled; GUI.enabled = enabled; }
        public static void ResetGUIEnabled() => GUI.enabled = _prevGuiEnabled;
        static bool _prevGuiEnabled = true;


        public static void SetLabelSize(int size) => GUI.skin.label.fontSize = size;
        public static void SetLabelBold() => GUI.skin.label.fontStyle = FontStyle.Bold;
        public static void SetLabelAlignmentCenter() => GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        public static void ResetLabelStyle()
        {
            GUI.skin.label.fontSize = 0;
            GUI.skin.label.fontStyle = FontStyle.Normal;
            GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        }


        public static void SetGUIColor(Color c)
        {
            if (!_guiColorModified)
                _defaultGuiColor = GUI.color;

            _guiColorModified = true;

            GUI.color = _defaultGuiColor * c;

        }
        public static void ResetGUIColor()
        {
            GUI.color = _guiColorModified ? _defaultGuiColor : Color.white;

            _guiColorModified = false;

        }
        static bool _guiColorModified;
        static Color _defaultGuiColor;



        #endregion

        #region Layout

        public static void BeginIndent(float f)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(f);
            GUILayout.BeginVertical();

            _indentLabelWidthStack.Push(EditorGUIUtility.labelWidth);

            EditorGUIUtility.labelWidth -= f;
        }

        public static void EndIndent(float f = 0)
        {
            GUILayout.EndVertical();
            GUILayout.Space(f);
            GUILayout.EndHorizontal();

            EditorGUIUtility.labelWidth = _indentLabelWidthStack.Pop();
        }
        static Stack<float> _indentLabelWidthStack = new Stack<float>();


        public static void Horizontal() { if (__hor) GUILayout.EndHorizontal(); else GUILayout.BeginHorizontal(); __hor = !__hor; }
        public static void Vertical() { if (__v) GUILayout.EndVertical(); else GUILayout.BeginVertical(); __v = !__v; }
        public static void Area(Rect r) { if (__a) GUILayout.EndArea(); else GUILayout.BeginArea(r); __a = !__a; }
        public static void Area() { if (__a) GUILayout.EndArea(); __a = !__a; }
        public static void ResetUIBools() { __a = __hor = __v = false; _prevGuiEnabled = true; }
        static bool __hor, __a, __v;

        #endregion

        #region Rect Drawing

        public static void DrawRect() => EditorGUI.DrawRect(lastRect, Color.black);
        public static void DrawRect(Rect r) => EditorGUI.DrawRect(r, Color.black);
        public static Rect Draw(this Rect r) { EditorGUI.DrawRect(r, Color.black); return r; }
        public static Rect Draw(this Rect r, Color c) { EditorGUI.DrawRect(r, c); return r; }

        #endregion

        #region Spacing

        public static void Space(float px = 6) => GUILayout.Space(px);

        public static void Divider(float space = 15, float yOffset = 0)
        {
            GUILayout.Label("", GUILayout.Height(space), GUILayout.ExpandWidth(true));
            lastRect.SetHeightFromMid(1).SetWidthFromMid(lastRect.width - 16).MoveY(yOffset).Draw(dividerColor);
        }

        public static Rect ExpandSpace() { GUILayout.Label("", GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true)); return lastRect; }

        public static Rect ExpandWidthLabelRect() { GUILayout.Label(""/* , GUILayout.Height(0) */, GUILayout.ExpandWidth(true)); return lastRect; }
        public static Rect ExpandWidthLabelRect(float height) { GUILayout.Label("", GUILayout.Height(height), GUILayout.ExpandWidth(true)); return lastRect; }


        #endregion

        #region Consts

        public static GUILayoutOption headerHeight => GUILayout.Height(headerHeightF);
        public const float headerHeightF = 24;

        public static GUILayoutOption normalButtonHeight => GUILayout.Height(normalButtonHeightF);
        public const float normalButtonHeightF = 24;

        public static GUILayoutOption bigButtonHeight => GUILayout.Height(bigButtonHeightF);
        public const float bigButtonHeightF = 30;

        public static GUILayoutOption smallHeaderHeight => GUILayout.Height(smallHeaderHeightF);
        public const float smallHeaderHeightF = 20;

        public const int outlineThickness = 1;

        #endregion

    }

}
#endif