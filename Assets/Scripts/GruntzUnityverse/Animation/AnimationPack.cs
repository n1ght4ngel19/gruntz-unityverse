﻿using System.Collections.Generic;
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

	private void OnValidate() {
		LoadAnimationz();
	}

	public void LoadAnimationz() {
		Clear();

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

	public void Clear() {
		deathAnimation = null;

		idle.up.Clear();
		idle.upRight.Clear();
		idle.right.Clear();
		idle.downRight.Clear();
		idle.down.Clear();
		idle.downLeft.Clear();
		idle.left.Clear();
		idle.upLeft.Clear();

		hostileIdle.up.Clear();
		hostileIdle.upRight.Clear();
		hostileIdle.right.Clear();
		hostileIdle.downRight.Clear();
		hostileIdle.down.Clear();
		hostileIdle.downLeft.Clear();
		hostileIdle.left.Clear();
		hostileIdle.upLeft.Clear();

		walk.up.Clear();
		walk.upRight.Clear();
		walk.right.Clear();
		walk.downRight.Clear();
		walk.down.Clear();
		walk.downLeft.Clear();
		walk.left.Clear();
		walk.upLeft.Clear();

		attack.up.Clear();
		attack.upRight.Clear();
		attack.right.Clear();
		attack.downRight.Clear();
		attack.down.Clear();
		attack.downLeft.Clear();
		attack.left.Clear();
		attack.upLeft.Clear();

		interact.up.Clear();
		interact.upRight.Clear();
		interact.right.Clear();
		interact.downRight.Clear();
		interact.down.Clear();
		interact.downLeft.Clear();
		interact.left.Clear();
		interact.upLeft.Clear();
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