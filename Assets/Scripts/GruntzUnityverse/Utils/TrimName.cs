#if UNITY_EDITOR
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace GruntzUnityverse.Utils {
public class TrimName : MonoBehaviour {
	private void OnValidate() {
		if (Regex.IsMatch(gameObject.name.Split("_").Last(), @"^\d+$")) {
			gameObject.name = gameObject.name.Remove(gameObject.name.Length - gameObject.name.Split("_").Last().Length - 1);
		}
	}
}
}
#endif
