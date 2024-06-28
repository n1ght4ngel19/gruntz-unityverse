using System.Collections.Generic;
using GruntzUnityverse.Utils.Extensionz;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

namespace GruntzUnityverse.Animation {
[CreateAssetMenu(fileName = "New Animation Pack", menuName = "Gruntz Unityverse/Animation Pack")]
public class AnimationPack : ScriptableObject {
	public string tool;

	public AnimationClip deathAnimation;
	public Animationz8Way idle;
	public Animationz8Way hostileIdle;
	public Animationz8Way walk;
	public Animationz8Way attack;
	public Animationz8Way interact;

	#if UNITY_EDITOR
	public void LoadAnimationz() {
		Clear();

		Addressables.LoadAssetAsync<AnimationClip>($"{tool}Grunt_Death").Completed += handle => deathAnimation = handle.Result;

		LoadAnimationz8Way(idle, "Idle");
		LoadHostileIdleAnimationz(hostileIdle);
		LoadSingularAnimationz(walk, "Walk");
		LoadAnimationz8Way(attack, "Attack");
		LoadSingularAnimationz(interact, "Item");

		EditorUtility.SetDirty(this);
	}
	#endif

	public void Clear() {
		deathAnimation = null;

		idle.up.SafeClear();
		idle.upRight.SafeClear();
		idle.right.SafeClear();
		idle.downRight.SafeClear();
		idle.down.SafeClear();
		idle.downLeft.SafeClear();
		idle.left.SafeClear();
		idle.upLeft.SafeClear();

		hostileIdle.up.SafeClear();
		hostileIdle.upRight.SafeClear();
		hostileIdle.right.SafeClear();
		hostileIdle.downRight.SafeClear();
		hostileIdle.down.SafeClear();
		hostileIdle.downLeft.SafeClear();
		hostileIdle.left.SafeClear();
		hostileIdle.upLeft.SafeClear();

		walk.up.SafeClear();
		walk.upRight.SafeClear();
		walk.right.SafeClear();
		walk.downRight.SafeClear();
		walk.down.SafeClear();
		walk.downLeft.SafeClear();
		walk.left.SafeClear();
		walk.upLeft.SafeClear();

		attack.up.SafeClear();
		attack.upRight.SafeClear();
		attack.right.SafeClear();
		attack.downRight.SafeClear();
		attack.down.SafeClear();
		attack.downLeft.SafeClear();
		attack.left.SafeClear();
		attack.upLeft.SafeClear();

		interact.up.SafeClear();
		interact.upRight.SafeClear();
		interact.right.SafeClear();
		interact.downRight.SafeClear();
		interact.down.SafeClear();
		interact.downLeft.SafeClear();
		interact.left.SafeClear();
		interact.upLeft.SafeClear();
	}

	private void LoadSingularAnimationz(Animationz8Way pack, string type) {
		LoadClipInto(pack.up, $"{tool}Grunt_{type}_North");
		LoadClipInto(pack.upRight, $"{tool}Grunt_{type}_Northeast");
		LoadClipInto(pack.right, $"{tool}Grunt_{type}_East");
		LoadClipInto(pack.downRight, $"{tool}Grunt_{type}_Southeast");
		LoadClipInto(pack.down, $"{tool}Grunt_{type}_South");
		LoadClipInto(pack.downLeft, $"{tool}Grunt_{type}_Southwest");
		LoadClipInto(pack.left, $"{tool}Grunt_{type}_West");
		LoadClipInto(pack.upLeft, $"{tool}Grunt_{type}_Northwest");
	}

	private void LoadHostileIdleAnimationz(Animationz8Way pack) {
		LoadClipInto(pack.up, $"{tool}Grunt_HostileIdle_North");
		LoadClipInto(pack.upRight, $"{tool}Grunt_HostileIdle_Northeast");
		LoadClipInto(pack.right, $"{tool}Grunt_HostileIdle_East");
		LoadClipInto(pack.downRight, $"{tool}Grunt_HostileIdle_Southeast");
		LoadClipInto(pack.down, $"{tool}Grunt_HostileIdle_South");
		LoadClipInto(pack.downLeft, $"{tool}Grunt_HostileIdle_Southwest");
		LoadClipInto(pack.left, $"{tool}Grunt_HostileIdle_West");
		LoadClipInto(pack.upLeft, $"{tool}Grunt_HostileIdle_Northwest");
	}

	private void LoadAnimationz8Way(Animationz8Way pack, string packName) {
		LoadClipInto(pack.up, $"{tool}Grunt_{packName}_North_01");
		LoadClipInto(pack.up, $"{tool}Grunt_{packName}_North_02");
		LoadClipInto(pack.upRight, $"{tool}Grunt_{packName}_Northeast_01");
		LoadClipInto(pack.right, $"{tool}Grunt_{packName}_East_01");
		LoadClipInto(pack.right, $"{tool}Grunt_{packName}_East_02");
		LoadClipInto(pack.downRight, $"{tool}Grunt_{packName}_Southeast_01");
		LoadClipInto(pack.down, $"{tool}Grunt_{packName}_South_01");
		LoadClipInto(pack.down, $"{tool}Grunt_{packName}_South_02");
		LoadClipInto(pack.downLeft, $"{tool}Grunt_{packName}_Southwest_01");
		LoadClipInto(pack.left, $"{tool}Grunt_{packName}_West_01");
		LoadClipInto(pack.left, $"{tool}Grunt_{packName}_West_02");
		LoadClipInto(pack.upLeft, $"{tool}Grunt_{packName}_Northwest_01");

		if (!packName.Equals("Idle")) {
			LoadClipInto(pack.upRight, $"{tool}Grunt_{packName}_Northeast_02");
			LoadClipInto(pack.downRight, $"{tool}Grunt_{packName}_Southeast_02");
			LoadClipInto(pack.downLeft, $"{tool}Grunt_{packName}_Southwest_02");
			LoadClipInto(pack.upLeft, $"{tool}Grunt_{packName}_Northwest_02");
		} else {
			LoadClipInto(pack.up, $"{tool}Grunt_{packName}_North_03");
			LoadClipInto(pack.right, $"{tool}Grunt_{packName}_East_03");
			LoadClipInto(pack.down, $"{tool}Grunt_{packName}_South_03");
			LoadClipInto(pack.left, $"{tool}Grunt_{packName}_West_03");
		}
	}

	public static AnimationClip GetRandomClip(Direction direction, Animationz8Way animz) {
		return direction switch {
			// Temporary solution until random idle animations are implemented
			Direction.Up => animz.up[Random.Range(0, 1)],
			Direction.UpRight => animz.upRight[Random.Range(0, 1)],
			Direction.Right => animz.right[Random.Range(0, 1)],
			Direction.DownRight => animz.downRight[Random.Range(0, 1)],
			Direction.Down => animz.down[Random.Range(0, 1)],
			Direction.DownLeft => animz.downLeft[Random.Range(0, 1)],
			Direction.Left => animz.left[Random.Range(0, 1)],
			Direction.UpLeft => animz.upLeft[Random.Range(0, 1)],
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
[CustomEditor(typeof(AnimationPack))]
public class AnimationPackV2Editor : UnityEditor.Editor {
	public override void OnInspectorGUI() {
		AnimationPack animationPack = (AnimationPack)target;

		if (GUILayout.Button("Load Animationz")) {
			animationPack.LoadAnimationz();
		}

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
