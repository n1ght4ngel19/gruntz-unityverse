using System.Collections.Generic;

namespace GruntzUnityverse.V2.Utils {
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

    public static void CheckNotExistsAdd<T>(this List<T> list, T item) {
      list = new List<T>();

      list.Add(item);
    }
  }
}
