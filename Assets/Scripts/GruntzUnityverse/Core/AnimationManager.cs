using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.Core {
public class AnimationManager : MonoBehaviour {
	public static AnimationManager instance { get; private set; }

	private void Awake() {
		instance = this;
	}

	[Header("Cursor Animationz")]
	public AnimationClip cursorDefault;

	public AnimationClip cursorBareHandz;
	public AnimationClip cursorBomb;
	public AnimationClip cursorBoomerang;
	public AnimationClip cursorBrickLayer;
	public AnimationClip cursorClub;
	public AnimationClip cursorGauntletz;
	public AnimationClip cursorFlailingGrunt;
	public AnimationClip cursorGooberStraw;
	public AnimationClip cursorGravityBootz;
	public AnimationClip cursorNerfGun;
	public AnimationClip cursorRockz;
	public AnimationClip cursorShield;
	public AnimationClip cursorShovel;
	public AnimationClip cursorSpring;
	public AnimationClip cursorSpyGear;
	public AnimationClip cursorSword;
	public AnimationClip cursorTimeBombz;
	public AnimationClip cursorToob;
	public AnimationClip cursorWarpstone;
	public AnimationClip cursorWelderKit;
	public AnimationClip cursorWingz;

	public AnimationClip cursorBabyWalker;
	public AnimationClip cursorBeachball;
	public AnimationClip cursorGoKart;
	public AnimationClip cursorJackInTheBox;
	public AnimationClip cursorJumprope;
	public AnimationClip cursorMonsterWheelz;
	public AnimationClip cursorPogoStick;
	public AnimationClip cursorSqueakToy;
	public AnimationClip cursorYoYo;

	[Header("Death Animationz")]
	public AnimationClip burnDeathAnimation;

	public AnimationClip electrocuteDeathAnimation;
	public AnimationClip explodeDeathAnimation;
	public AnimationClip fallDeathAnimation;
	public AnimationClip freezeDeathAnimation;
	public AnimationClip holeDeathAnimation;
	public AnimationClip karaokeDeathAnimation;
	public AnimationClip meltDeathAnimation;
	public AnimationClip sinkDeathAnimation;
	public AnimationClip squashDeathAnimation;

	[Header("Grunt Warp Animationz")]
	public AnimationClip gruntWarpEnterAnim;

	public List<AnimationClip> gruntVictoryAnimz;

	public AnimationClip gruntFallingEntranceAnim;

	[Header("Warp Animationz")]
	public AnimationClip warpAppearAnim;

	public AnimationClip warpDisappearAnim;

	public AnimationClip warpSwirlingAnim;

	public AnimationClip timeBombTickingAnim;

	[Header("Effectz")]
	public AnimationClip explosionAnim1;

	public AnimationClip explosionAnim2;

	public AnimationClip explosionAnim3;

	public AnimationClip dirtEffect;

	public void Setup() {
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_Default").Completed += handle => cursorDefault = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_BareHandz").Completed += handle => cursorBareHandz = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_Bomb").Completed += handle => cursorBomb = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_Boomerang").Completed += handle => cursorBoomerang = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_BrickLayer").Completed += handle => cursorBrickLayer = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_Club").Completed += handle => cursorClub = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_FlailingGrunt").Completed += handle => cursorFlailingGrunt = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_Gauntletz").Completed += handle => cursorGauntletz = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_GooberStraw").Completed += handle => cursorGooberStraw = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_GravityBootz").Completed += handle => cursorGravityBootz = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_NerfGun").Completed += handle => cursorNerfGun = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_Rockz").Completed += handle => cursorRockz = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_Shield").Completed += handle => cursorShield = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_Shovel").Completed += handle => cursorShovel = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_Spring").Completed += handle => cursorSpring = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_SpyGear").Completed += handle => cursorSpyGear = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_Sword").Completed += handle => cursorSword = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_TimeBombz").Completed += handle => cursorTimeBombz = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_Toob").Completed += handle => cursorToob = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_Warpstone").Completed += handle => cursorWarpstone = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_WelderKit").Completed += handle => cursorWelderKit = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_Wingz").Completed += handle => cursorWingz = handle.Result;

		Addressables.LoadAssetAsync<AnimationClip>("Cursor_BabyWalker").Completed += handle => cursorBabyWalker = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_Beachball").Completed += handle => cursorBeachball = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_GoKart").Completed += handle => cursorGoKart = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_JackInTheBox").Completed += handle => cursorJackInTheBox = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_Jumprope").Completed += handle => cursorJumprope = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_MonsterWheelz").Completed += handle => cursorMonsterWheelz = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_PogoStick").Completed += handle => cursorPogoStick = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_SqueakToy").Completed += handle => cursorSqueakToy = handle.Result;
		Addressables.LoadAssetAsync<AnimationClip>("Cursor_YoYo").Completed += handle => cursorYoYo = handle.Result;
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(AnimationManager))]
public class AnimationManagerEditor : UnityEditor.Editor {
	public override void OnInspectorGUI() {
		base.OnInspectorGUI();

		if (GUILayout.Button("Load Animationz")) {
			((AnimationManager)target).Setup();
		}
	}
}
#endif
}
