using System.Collections;
using Animancer;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

namespace GruntzUnityverse.Actorz {
  public class King : MonoBehaviour {
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public AnimancerComponent animancer;
    [HideInInspector] public AudioSource audioSource;
    private Animator _animator;
    private AnimationClip _movingAnim;
    private AnimationClip _idleAnim;
    private AnimationClip _deathAnim;
    private AnimationClip _joyAnim;
    private AnimationClip _battlecryAnim;
    private AnimationClip _panicAnim;

    private void Start() {
      spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
      _animator = gameObject.GetComponent<Animator>();
      animancer = gameObject.GetComponent<AnimancerComponent>();
      animancer.Animator = _animator;
      audioSource = gameObject.GetComponent<AudioSource>();

      Addressables.LoadAssetAsync<AnimationClip>("Warlord_King_Moving.anim").Completed += handle => {
        _movingAnim = handle.Result;

        StartCoroutine(Move());
      };

      Addressables.LoadAssetAsync<AnimationClip>("Warlord_King_Idle.anim").Completed += handle => {
        _idleAnim = handle.Result;
      };

      Addressables.LoadAssetAsync<AnimationClip>("Warlord_King_Death.anim").Completed += handle => {
        _deathAnim = handle.Result;
      };

      Addressables.LoadAssetAsync<AnimationClip>("Warlord_King_Joy.anim").Completed += handle => {
        _joyAnim = handle.Result;
      };

      Addressables.LoadAssetAsync<AnimationClip>("Warlord_King_Battlecry.anim").Completed += handle => {
        _battlecryAnim = handle.Result;
      };

      Addressables.LoadAssetAsync<AnimationClip>("Warlord_King_Panic.anim").Completed += handle => {
        _panicAnim = handle.Result;
      };
    }

    public IEnumerator Move() {
      animancer.Play(_movingAnim);

      yield return null;
    }

    public IEnumerator Joy() {
      enabled = false;
      PlayRandomJoyVoice();
      animancer.Play(_joyAnim);

      yield return new WaitForSeconds(7f);

      StartCoroutine(Move());
    }

    private void PlayRandomJoyVoice() {
      int randIdx = Random.Range(1, 9);

      Addressables.LoadAssetAsync<AudioClip>($"King_Joy_0{randIdx}.wav").Completed += handle => {
        audioSource.PlayOneShot(handle.Result);
      };
    }

    private IEnumerator RandomIdle() {
      int randIdx = Random.Range(1, 3);

      // Todo: Random idle animation
      animancer.Play(_idleAnim);

      yield return new WaitForSeconds(7.75f);

      StartCoroutine(Move());
    }
  }
}
