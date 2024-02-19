using GruntzUnityverse.V2.Grunt;
using UnityEngine;

namespace GruntzUnityverse.V2.Itemz.Toolz {
[CreateAssetMenu(fileName = "New Equipped Tool", menuName = "Gruntz Unityverse/Equipped Tool")]
public class EquippedTool : ScriptableObject {
	public string toolName;
	public string description;

	[Range(0, 20)]
	public int damage;

	[Range(0, 10)]
	public int range;

	[Range(0, 5)]
	public float moveSpeed;

	public AnimationPackV2 animationPack;
}
}
