using GruntzUnityverse.Actorz;
using GruntzUnityverse.Enumz;
using UnityEditor;
using UnityEngine;

namespace GruntzUnityverse.Editor {
  [CustomEditor(typeof(RollingBall), true), CanEditMultipleObjects]
  public class RollingBallEditor : UnityEditor.Editor {
    public override void OnInspectorGUI() {
      RollingBall inspected = (RollingBall)target;
      
      EditorGUILayout.Space(10);
      inspected.speed = EditorGUILayout.Slider("Speed", inspected.speed, 0, 5f);
      EditorGUILayout.Space(10);
      
      // --------------------------------------------------
      GUILayout.BeginVertical();
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();

      string inactiveNW = inspected.moveDirection == Direction.Northwest ? "" : "_Inactive";

      if (MakeButton($"Assets/EditorAssets/Arrow_1W_Northwest{inactiveNW}.png")) {
        inspected.moveDirection = Direction.Northwest;
      }

      string inactiveN = inspected.moveDirection == Direction.North ? "" : "_Inactive";

      if (MakeButton($"Assets/EditorAssets/Arrow_1W_North{inactiveN}.png")) {
        inspected.moveDirection = Direction.North;
      }

      string inactiveNE = inspected.moveDirection == Direction.Northeast ? "" : "_Inactive";

      if (MakeButton($"Assets/EditorAssets/Arrow_1W_Northeast{inactiveNE}.png")) {
        inspected.moveDirection = Direction.Northeast;
      }

      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      // --------------------------------------------------
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();

      string inactiveW = inspected.moveDirection == Direction.West ? "" : "_Inactive";

      if (MakeButton($"Assets/EditorAssets/Arrow_1W_West{inactiveW}.png")) {
        inspected.moveDirection = Direction.West;
      }

      MakeBlankButtonOfSize(48);

      string inactiveE = inspected.moveDirection == Direction.East ? "" : "_Inactive";

      if (MakeButton($"Assets/EditorAssets/Arrow_1W_East{inactiveE}.png")) {
        inspected.moveDirection = Direction.East;
      }

      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      // --------------------------------------------------
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();

      string inactiveSW = inspected.moveDirection == Direction.Southwest ? "" : "_Inactive";

      if (MakeButton($"Assets/EditorAssets/Arrow_1W_Southwest{inactiveSW}.png")) {
        inspected.moveDirection = Direction.Southwest;
      }

      string inactiveS = inspected.moveDirection == Direction.South ? "" : "_Inactive";

      if (MakeButton($"Assets/EditorAssets/Arrow_1W_South{inactiveS}.png")) {
        inspected.moveDirection = Direction.South;
      }

      string inactiveSE = inspected.moveDirection == Direction.Southeast ? "" : "_Inactive";

      if (MakeButton($"Assets/EditorAssets/Arrow_1W_Southeast{inactiveSE}.png")) {
        inspected.moveDirection = Direction.Southeast;
      }


      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
      // --------------------------------------------------
      GUILayout.EndVertical();
    }

    private bool MakeButton(string imagePath) {
      GUILayoutOption[] options = {
        GUILayout.Width(48),
        GUILayout.Height(48),
      };

      return GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>(imagePath), options);
    }

    private bool MakeBlankButtonOfSize(int size) {
      GUILayoutOption[] options = {
        GUILayout.Width(size),
        GUILayout.Height(size),
      };

      return GUILayout.Button(AssetDatabase.LoadAssetAtPath<Texture>("Assets/EditorAssets/Blank.png"), options);
    }
  }
}
