using System.Collections;
using System.Linq;
using GruntzUnityverse.MapObjectz.MapItemz;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

namespace GruntzUnityverse.MapObjectz.Interactablez {
  public class Rock : MapObject, IBreakable {
    public AnimationClip BreakAnimation { get; set; }
    private Vector3 _brokenScale;
    private Quaternion _brokenRotation;
    public MapItem hiddenItem;
    private bool _isInitialized;
    // -------------------------------------------------------------------------------- //

    protected override void Start() {
      base.Start();

      isTargetable = true;

      GameManager.Instance.currentLevelManager.SetBlockedAt(location, true);
      GameManager.Instance.currentLevelManager.SetHardTurnAt(location, true);
      Addressables.LoadAssetAsync<AnimationClip>($"RockBreak_{abbreviatedArea}_01.anim").Completed += (handle) => {
        BreakAnimation = handle.Result;
      };

      transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
      _brokenScale = new Vector3(0.7f, 0.7f, 0.7f);
      _brokenRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
    }
    // -------------------------------------------------------------------------------- //

    private void Update() {
      if (!_isInitialized) {
        _isInitialized = true;

        hiddenItem = FindObjectsOfType<MapItem>()
          .FirstOrDefault(item =>
            item.ownNode == ownNode);

        hiddenItem?.SetRendererEnabled(false);
      }
    }

    public IEnumerator Break(float contactDelay) {
      yield return new WaitForSeconds(contactDelay);

      animancer.Play(BreakAnimation);

      Addressables.LoadAssetAsync<AudioClip>($"{abbreviatedArea}_RockBreak.wav").Completed += handle => {
        GameManager.Instance.audioSource.PlayOneShot(handle.Result);
      };

      transform.localScale = _brokenScale;
      transform.localRotation = _brokenRotation;

      GameManager.Instance.currentLevelManager.Rockz.Remove(this);
      GameManager.Instance.currentLevelManager.SetBlockedAt(location, false);
      GameManager.Instance.currentLevelManager.SetHardTurnAt(location, false);
      hiddenItem?.SetRendererEnabled(true);

      yield return new WaitForSeconds(BreakAnimation.length);

      spriteRenderer.sortingLayerName = "AlwaysBottom";
      spriteRenderer.sortingOrder = 15;
      enabled = false;
    }
  }
}
