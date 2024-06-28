using UnityEngine;

namespace GruntzUnityverse {
[CreateAssetMenu(fileName = "LevelData", menuName = "Gruntz Unityverse/Level Data")]
public class LevelData : ScriptableObject {
	public string levelName;

	public Area area;
}
}
