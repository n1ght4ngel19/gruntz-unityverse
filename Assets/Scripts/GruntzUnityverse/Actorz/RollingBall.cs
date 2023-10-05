using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.MapObjectz;
using GruntzUnityverse.MapObjectz.BaseClasses;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GruntzUnityverse.Actorz {
  public class RollingBall : MapObject, IAudioSource {
    public float speed;
    public Direction moveDirection;
    private Dictionary<string, AnimationClip> _rollAnimSet;
    private Vector3 _brokenScale;
    private Quaternion _brokenRotation;
    private bool _hasStarted;
    public AudioSource AudioSource { get; set; }
    public AudioClip breakSound;

    private void Update() {
      if (!_hasStarted) {
        animancer.Play(_rollAnimSet[moveDirection.ToString()]);
        _hasStarted = true;

        Addressables.LoadAssetAsync<AudioClip>($"{abbreviatedArea}_RollingBall.wav").Completed += handle => {
          AudioSource.clip = handle.Result;
          AudioSource.Play();
        };

        return;
      }

      #region Destroying the Ball
      if (ownNode.isBlocked || (ownNode.isWater && ownNode.Neighbours.Any(node => !node.isWater))) {
        animancer.Play(_rollAnimSet["Explosion"]);

        AudioSource.Stop();
        AudioSource.PlayOneShot(breakSound);

        transform.localScale = _brokenScale;
        transform.localRotation = _brokenRotation;
        spriteRenderer.sortingLayerName = "AlwaysBottom";
        spriteRenderer.sortingOrder = 15;
        enabled = false;

        return;
      }

      if (ownNode.isWater || ownNode.isHole || ownNode.isVoid) {
        animancer.Play(_rollAnimSet["Sink"]);
        spriteRenderer.enabled = false;
        enabled = false;

        return;
      }
      #endregion

      Move();
    }

    // ------------------------------------------------------------ //
    // CLASS METHODS
    // ------------------------------------------------------------ //
    public void Move() {
      if (Vector2.Distance(ownNode.location, transform.position) > 0.9f) {
        ownNode = GameManager.Instance.currentLevelManager.NodeAt(Vector2Int.FloorToInt(transform.position));
      }

      if (moveDirection == Direction.North) {
        transform.position += Vector3.up * (speed * Time.deltaTime);
      } else if (moveDirection == Direction.East) {
        transform.position += Vector3.right * (speed * Time.deltaTime);
      } else if (moveDirection == Direction.South) {
        transform.position += Vector3.down * (speed * Time.deltaTime);
      } else if (moveDirection == Direction.West) {
        transform.position += Vector3.left * (speed * Time.deltaTime);
      }
    }

    public void ChangeDirection(Direction direction) {
      moveDirection = direction;
      animancer.Play(_rollAnimSet[moveDirection.ToString()]);
    }

    // ------------------------------------------------------------ //
    // OVERRIDES
    // ------------------------------------------------------------ //
    public override void Setup() {
      base.Setup();

      _brokenScale = new Vector3(0.7f, 0.7f, 0.7f);
      _brokenRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));

      AudioSource = gameObject.AddComponent<AudioSource>();
      AudioSource.loop = true;
      
      GameManager.Instance.currentLevelManager.rollingBallz.Add(this);

      Addressables.LoadAssetAsync<AudioClip>($"{abbreviatedArea}_RockBreak.wav").Completed += handle => {
        breakSound = handle.Result;
      };
    }

    protected override void LoadAnimationz() {
      _rollAnimSet = new Dictionary<string, AnimationClip>();

      string key = $"{abbreviatedArea}_RollingBall_{Direction.East}.anim";

      Addressables.LoadAssetAsync<AnimationClip>(key).Completed += handle => {
        _rollAnimSet.Add("East", handle.Result);
      };

      key = $"{abbreviatedArea}_RollingBall_{Direction.South}.anim";

      Addressables.LoadAssetAsync<AnimationClip>(key).Completed += handle => {
        _rollAnimSet.Add("South", handle.Result);
      };

      key = $"{abbreviatedArea}_RollingBall_{Direction.North}.anim";

      Addressables.LoadAssetAsync<AnimationClip>(key).Completed += handle => {
        _rollAnimSet.Add("North", handle.Result);
      };

      key = $"{abbreviatedArea}_RollingBall_{Direction.West}.anim";

      Addressables.LoadAssetAsync<AnimationClip>(key).Completed += handle => {
        _rollAnimSet.Add("West", handle.Result);
      };

      key = $"{abbreviatedArea}_RollingBall_Explosion.anim";

      Addressables.LoadAssetAsync<AnimationClip>(key).Completed += handle => {
        _rollAnimSet.Add("Explosion", handle.Result);
      };

      key = $"{abbreviatedArea}_RollingBall_Sink.anim";

      Addressables.LoadAssetAsync<AnimationClip>(key).Completed += handle => {
        _rollAnimSet.Add("Sink", handle.Result);
      };
    }
  }
}
