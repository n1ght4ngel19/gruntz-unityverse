using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GruntzUnityverse.MapObjectz.BaseClasses;
using GruntzUnityverse.Pathfinding;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

namespace GruntzUnityverse.MapObjectz.Interactablez {
  public class GiantRock : MapObject, IBreakable {
    public AnimationClip BreakAnimation { get; set; }
    private Quaternion _brokenRotation;
    public MapItem hiddenItem;
    private bool _isInitialized;
    public GiantRockEdge giantRockEdge;
    public List<GiantRockEdge> edgez;
    // -------------------------------------------------------------------------------- //

    protected override void Start() {
      base.Start();

      isTargetable = true;

      foreach (Node node in ownNode.Neighbours) {
        GiantRockEdge edge = Instantiate(giantRockEdge, node.transform.position, Quaternion.identity);
        edge.mainRock = this;
        edge.BreakAnimation = BreakAnimation;
        edge.transform.parent = transform;
        edge.ownNode = node;
        edge.location = node.location;
        edge.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
        edgez.Add(edge);
      }

      _brokenRotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
      hiddenItem = FindObjectsOfType<MapItem>()
        .FirstOrDefault(item =>
          (item.transform.position.x - transform.position.x) < 0.1f
          && (item.transform.position.y - transform.position.y) < 0.1f);
    }
    // -------------------------------------------------------------------------------- //

    private void Update() {
      _isInitialized = true;

      hiddenItem = FindObjectsOfType<MapItem>()
        .FirstOrDefault(item =>
          item.ownNode == ownNode);

      hiddenItem?.SetRendererEnabled(false);
    }

    public IEnumerator Break(float contactDelay) {
      yield return new WaitForSeconds(contactDelay);

      foreach (GiantRockEdge edge in edgez) {
        GameManager.Instance.currentLevelManager.SetBlockedAt(edge.location, false);
        GameManager.Instance.currentLevelManager.SetHardTurnAt(edge.location, false);

        Destroy(edge.gameObject);
      }

      animancer.Play(BreakAnimation);

      Addressables.LoadAssetAsync<AudioClip>($"{abbreviatedArea}_RockBreak.wav").Completed += handle => {
        GameManager.Instance.audioSource.PlayOneShot(handle.Result);
      };


      transform.localRotation = _brokenRotation;
      hiddenItem = FindObjectsOfType<MapItem>().FirstOrDefault(item => (Vector2)item.transform.position == (Vector2)transform.position);

      GameManager.Instance.currentLevelManager.SetBlockedAt(location, false);
      GameManager.Instance.currentLevelManager.SetHardTurnAt(location, false);
      hiddenItem?.SetRendererEnabled(true);

      yield return new WaitForSeconds(BreakAnimation.length);

      spriteRenderer.sortingLayerName = "AlwaysBottom";
      spriteRenderer.sortingOrder = 15;
      enabled = false;
    }

    protected override void LoadAnimationz() {
      Addressables.LoadAssetAsync<AnimationClip>($"{abbreviatedArea}_RockBreak.anim").Completed += handle => {
        BreakAnimation = handle.Result;
      };
    }
  }
}
