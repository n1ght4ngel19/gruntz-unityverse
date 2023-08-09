using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;

namespace GruntzUnityverse.Objectz.Hazardz {
  public class Spikez : MapObject {
    public int damage;
    private Grunt _targetGrunt;
    private bool _isRunning;

    protected override void Start() {
      base.Start();

      damage = 2;
    }

    private void Update() {
      if (_targetGrunt is not null && !_targetGrunt.AtNode(ownNode)) {
        CancelInvoke(nameof(DamageGrunt));

        _targetGrunt = null;
        _isRunning = false;
      }

      _targetGrunt = LevelManager.Instance.allGruntz.FirstOrDefault(grunt => grunt.AtNode(ownNode));

      if (_targetGrunt is not null && _targetGrunt.AtNode(ownNode)) {
        if (!_isRunning) {
          InvokeRepeating(nameof(DamageGrunt), 0, 1f);
        }

        _isRunning = true;
      }
    }

    public void DamageGrunt() {
      _targetGrunt.health -= damage;

      if (_targetGrunt.health <= 0) {
        StartCoroutine(_targetGrunt.Death());

        return;
      }

      _targetGrunt.healthBar.spriteRenderer.sprite = _targetGrunt.healthBar.frames[_targetGrunt.health];
    }
  }
}
