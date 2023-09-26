using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using GruntzUnityverse.Managerz;
using GruntzUnityverse.MapObjectz.BaseClasses;

namespace GruntzUnityverse.MapObjectz.Hazardz {
  public class Spikez : MapObject {
    public int damage;
    private Grunt _targetGrunt;
    private bool _isRunning;

    protected override void Start() {
      base.Start();

      damage = 2;
    }
    // -------------------------------------------------------------------------------- //

    private void Update() {
      if (_targetGrunt is not null && _targetGrunt.navigator.ownNode != ownNode) {
        CancelInvoke(nameof(DamageGrunt));

        _targetGrunt = null;
        _isRunning = false;
      }

      _targetGrunt = GameManager.Instance.currentLevelManager.allGruntz.FirstOrDefault(grunt => grunt.navigator.ownNode == ownNode);

      if (_targetGrunt != null && _targetGrunt.AtNode(ownNode)) {
        if (!_isRunning) {
          InvokeRepeating(nameof(DamageGrunt), 0, 1f);
        }

        _isRunning = true;
      }
    }
    // -------------------------------------------------------------------------------- //

    public void DamageGrunt() {
      _targetGrunt.health -= damage;

      if (_targetGrunt.health <= 0) {
        StartCoroutine(_targetGrunt.Die(DeathName.Default));

        return;
      }

      _targetGrunt.healthBar.spriteRenderer.sprite = _targetGrunt.healthBar.frames[_targetGrunt.health];
    }
  }
}
