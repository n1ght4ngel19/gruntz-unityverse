using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.V2.Grunt {
[CreateAssetMenu(fileName = "New Animation Pack", menuName = "Gruntz Unityverse/Animation Pack")]
public class AnimationPackV2 : ScriptableObject {
	public string tool;

	public AnimationClip deathAnimation;
	public Animationz8Way idle;
	public Animationz8Way hostileIdle;
	public Animationz8Way walk;
	public Animationz8Way attack;
	public Animationz8Way interact;

	public void LoadAnimationz() {
		Addressables.LoadAssetAsync<AnimationClip>($"Assets/Animationz/Gruntz/{tool}Grunt/Death/Clipz/{tool}Grunt_Death.anim")
			.Completed += handle => {
			deathAnimation = handle.Result;
		};

		LoadAnimationz8Way(idle, "Idle");
		LoadHostileIdleAnimationz(hostileIdle);
		LoadSingularAnimationz(walk, "Walk");
		LoadAnimationz8Way(attack, "Attack");
		LoadSingularAnimationz(interact, "Item");
	}

	private void LoadSingularAnimationz(Animationz8Way pack, string type) {
		LoadClipInto(pack.up, $"Assets/Animationz/Gruntz/{tool}Grunt/{type}/Clipz/{tool}Grunt_{type}_North.anim");
		LoadClipInto(pack.upRight, $"Assets/Animationz/Gruntz/{tool}Grunt/{type}/Clipz/{tool}Grunt_{type}_Northeast.anim");
		LoadClipInto(pack.right, $"Assets/Animationz/Gruntz/{tool}Grunt/{type}/Clipz/{tool}Grunt_{type}_East.anim");
		LoadClipInto(pack.downRight, $"Assets/Animationz/Gruntz/{tool}Grunt/{type}/Clipz/{tool}Grunt_{type}_Southeast.anim");
		LoadClipInto(pack.down, $"Assets/Animationz/Gruntz/{tool}Grunt/{type}/Clipz/{tool}Grunt_{type}_South.anim");
		LoadClipInto(pack.downLeft, $"Assets/Animationz/Gruntz/{tool}Grunt/{type}/Clipz/{tool}Grunt_{type}_Southwest.anim");
		LoadClipInto(pack.left, $"Assets/Animationz/Gruntz/{tool}Grunt/{type}/Clipz/{tool}Grunt_{type}_West.anim");
		LoadClipInto(pack.upLeft, $"Assets/Animationz/Gruntz/{tool}Grunt/{type}/Clipz/{tool}Grunt_{type}_Northwest.anim");
	}

	private void LoadHostileIdleAnimationz(Animationz8Way pack) {
		LoadClipInto(pack.up, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_North_Idle.anim");
		LoadClipInto(pack.upRight, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_Northeast_Idle.anim");
		LoadClipInto(pack.right, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_East_Idle.anim");
		LoadClipInto(pack.downRight, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_Southeast_Idle.anim");
		LoadClipInto(pack.down, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_South_Idle.anim");
		LoadClipInto(pack.downLeft, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_Southwest_Idle.anim");
		LoadClipInto(pack.left, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_West_Idle.anim");
		LoadClipInto(pack.upLeft, $"Assets/Animationz/Gruntz/{tool}Grunt/Attack/Clipz/{tool}Grunt_Attack_Northwest_Idle.anim");
	}

	private void LoadAnimationz8Way(Animationz8Way pack, string packName) {
		LoadClipInto(pack.up, $"Assets/Animationz/Gruntz/{tool}Grunt/{packName}/Clipz/{tool}Grunt_{packName}_North_01.anim");
		LoadClipInto(pack.up, $"Assets/Animationz/Gruntz/{tool}Grunt/{packName}/Clipz/{tool}Grunt_{packName}_North_02.anim");
		LoadClipInto(pack.upRight, $"Assets/Animationz/Gruntz/{tool}Grunt/{packName}/Clipz/{tool}Grunt_{packName}_Northeast_01.anim");
		LoadClipInto(pack.right, $"Assets/Animationz/Gruntz/{tool}Grunt/{packName}/Clipz/{tool}Grunt_{packName}_East_01.anim");
		LoadClipInto(pack.right, $"Assets/Animationz/Gruntz/{tool}Grunt/{packName}/Clipz/{tool}Grunt_{packName}_East_02.anim");
		LoadClipInto(pack.downRight, $"Assets/Animationz/Gruntz/{tool}Grunt/{packName}/Clipz/{tool}Grunt_{packName}_Southeast_01.anim");
		LoadClipInto(pack.down, $"Assets/Animationz/Gruntz/{tool}Grunt/{packName}/Clipz/{tool}Grunt_{packName}_South_01.anim");
		LoadClipInto(pack.down, $"Assets/Animationz/Gruntz/{tool}Grunt/{packName}/Clipz/{tool}Grunt_{packName}_South_02.anim");
		LoadClipInto(pack.downLeft, $"Assets/Animationz/Gruntz/{tool}Grunt/{packName}/Clipz/{tool}Grunt_{packName}_Southwest_01.anim");
		LoadClipInto(pack.left, $"Assets/Animationz/Gruntz/{tool}Grunt/{packName}/Clipz/{tool}Grunt_{packName}_West_01.anim");
		LoadClipInto(pack.left, $"Assets/Animationz/Gruntz/{tool}Grunt/{packName}/Clipz/{tool}Grunt_{packName}_West_02.anim");
		LoadClipInto(pack.upLeft, $"Assets/Animationz/Gruntz/{tool}Grunt/{packName}/Clipz/{tool}Grunt_{packName}_Northwest_01.anim");

		if (!packName.Equals("Idle")) {
			LoadClipInto(pack.upRight, $"Assets/Animationz/Gruntz/{tool}Grunt/{packName}/Clipz/{tool}Grunt_{packName}_Northeast_02.anim");
			LoadClipInto(pack.downRight, $"Assets/Animationz/Gruntz/{tool}Grunt/{packName}/Clipz/{tool}Grunt_{packName}_Southeast_02.anim");
			LoadClipInto(pack.downLeft, $"Assets/Animationz/Gruntz/{tool}Grunt/{packName}/Clipz/{tool}Grunt_{packName}_Southwest_02.anim");
			LoadClipInto(pack.upLeft, $"Assets/Animationz/Gruntz/{tool}Grunt/{packName}/Clipz/{tool}Grunt_{packName}_Northwest_02.anim");
		}
	}

	public static AnimationClip GetRandomClip(DirectionV2 direction, Animationz8Way animz) {
		return direction switch {
			// Temporary solution until random idle animations are implemented
			DirectionV2.Up => animz.up[Random.Range(0, 1)],
			DirectionV2.UpRight => animz.upRight[Random.Range(0, 1)],
			DirectionV2.Right => animz.right[Random.Range(0, 1)],
			DirectionV2.DownRight => animz.downRight[Random.Range(0, 1)],
			DirectionV2.Down => animz.down[Random.Range(0, 1)],
			DirectionV2.DownLeft => animz.downLeft[Random.Range(0, 1)],
			DirectionV2.Left => animz.left[Random.Range(0, 1)],
			DirectionV2.UpLeft => animz.upLeft[Random.Range(0, 1)],
			_ => null,
		};
	}

	private static void LoadClipInto(List<AnimationClip> list, string address) {
		Addressables.LoadAssetAsync<AnimationClip>(address).Completed += handle => {
			list.Add(handle.Result);
		};
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(AnimationPackV2))]
public class AnimationPackV2Editor : UnityEditor.Editor {
	public override void OnInspectorGUI() {
		AnimationPackV2 animationPack = (AnimationPackV2)target;

		GUILayout.BeginHorizontal();

		if (GUILayout.Button("Load Animationz")) {
			animationPack.LoadAnimationz();
		}

		GUILayout.Space(10);

		if (GUILayout.Button("Clear")) {
			animationPack.deathAnimation = null;

			animationPack.idle.up.Clear();
			animationPack.idle.upRight.Clear();
			animationPack.idle.right.Clear();
			animationPack.idle.downRight.Clear();
			animationPack.idle.down.Clear();
			animationPack.idle.downLeft.Clear();
			animationPack.idle.left.Clear();
			animationPack.idle.upLeft.Clear();

			animationPack.hostileIdle.up.Clear();
			animationPack.hostileIdle.upRight.Clear();
			animationPack.hostileIdle.right.Clear();
			animationPack.hostileIdle.downRight.Clear();
			animationPack.hostileIdle.down.Clear();
			animationPack.hostileIdle.downLeft.Clear();
			animationPack.hostileIdle.left.Clear();
			animationPack.hostileIdle.upLeft.Clear();

			animationPack.walk.up.Clear();
			animationPack.walk.upRight.Clear();
			animationPack.walk.right.Clear();
			animationPack.walk.downRight.Clear();
			animationPack.walk.down.Clear();
			animationPack.walk.downLeft.Clear();
			animationPack.walk.left.Clear();
			animationPack.walk.upLeft.Clear();

			animationPack.attack.up.Clear();
			animationPack.attack.upRight.Clear();
			animationPack.attack.right.Clear();
			animationPack.attack.downRight.Clear();
			animationPack.attack.down.Clear();
			animationPack.attack.downLeft.Clear();
			animationPack.attack.left.Clear();
			animationPack.attack.upLeft.Clear();

			animationPack.interact.up.Clear();
			animationPack.interact.upRight.Clear();
			animationPack.interact.right.Clear();
			animationPack.interact.downRight.Clear();
			animationPack.interact.down.Clear();
			animationPack.interact.downLeft.Clear();
			animationPack.interact.left.Clear();
			animationPack.interact.upLeft.Clear();
		}

		GUILayout.EndHorizontal();

		GUILayout.Space(10);

		base.OnInspectorGUI();
	}
}
#endif

[System.Serializable]
public struct Animationz8Way {
	public List<AnimationClip> up;
	public List<AnimationClip> upRight;
	public List<AnimationClip> right;
	public List<AnimationClip> downRight;
	public List<AnimationClip> down;
	public List<AnimationClip> downLeft;
	public List<AnimationClip> left;
	public List<AnimationClip> upLeft;
}
}
