using System.Collections.Generic;

namespace GruntzUnityverse.V2.Utils {
  public static class ListX {
    // public static T GetRemove<T>(this List<T> list, int index) {
    //   T item = list[index];
    //   list.RemoveAt(index);
    //
    //   return item;
    // }

    public static void SafeAdd<T>(this List<T> list, T item) {
      list = new List<T>();

      list.Add(item);
    }
  }
}
