using System.Linq;
using GruntzUnityverse.Actorz;
using GruntzUnityverse.Managerz;
using UnityEngine;

namespace GruntzUnityverse.Objectz.Hazardz {
  public class Spikez : MapObject {
    public int Damage { get; set; }
    private Grunt TargetGrunt { get; set; }
    private bool isRunning;

    protected override void Start() {
      base.Start();

      Damage = 2;
    }

    private void Update() {
      if (TargetGrunt is not null && !TargetGrunt.IsOnLocation(OwnLocation)) {
        CancelInvoke(nameof(DamageGrunt));

        TargetGrunt = null;
        isRunning = false;
      }

      TargetGrunt = LevelManager.Instance.AllGruntz.FirstOrDefault(grunt => grunt.IsOnLocation(OwnLocation));

      if (TargetGrunt is not null && TargetGrunt.IsOnLocation(OwnLocation)) {
        // foreach (Grunt grunt in LevelManager.Instance.AllGruntz.Where(grunt => grunt.IsOnLocation(OwnLocation))) {
        //   TargetGrunt = grunt;

        // StartCoroutine(DamageGrunt(grunt));
        if (!isRunning) {
          InvokeRepeating(nameof(DamageGrunt), 0, 1f);
        }

        isRunning = true;
      }
    }

    public void DamageGrunt() {
      TargetGrunt.Health -= Damage;

      if (TargetGrunt.Health <= 0) {
        StartCoroutine(TargetGrunt.Death());

        return;
      }

      TargetGrunt.HealthBar.Renderer.sprite = TargetGrunt.HealthBar.Frames[TargetGrunt.Health];
    }
  }
}
