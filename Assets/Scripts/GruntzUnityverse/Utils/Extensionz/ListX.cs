using System.Collections.Generic;

namespace GruntzUnityverse.Utils.Extensionz {
public static class ListX {
	public static void UniqueAdd<T>(this List<T> list, T item) {
		if (!list.Contains(item)) {
			list.Add(item);
		}
	}

	public static void CheckNullAdd<T>(this List<T> list, T item) where T : class {
		if (item != null) {
			list.Add(item);
		}
	}

	public static void InitializeListAdd<T>(this List<T> list, T item) {
		list = new List<T> {
			item,
		};
	}

	public static void IfNotContainsAdd<T>(this List<T> list, T item) {
		if (!list.Contains(item)) {
			list.Add(item);
		}
	}

	public static void RemoveIfContains<T>(this List<T> list, T item) {
		if (list.Contains(item)) {
			list.Remove(item);
		}
	}

	public static T GetRandom<T>(this List<T> list) {
		return list[UnityEngine.Random.Range(0, list.Count)];
	}
}
}
