using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

namespace KevinCastejon.AnimationGenerator
{
    /// <summary>
    /// Configuration asset for generating AnimationClip assets from a Texture2D spritesheet asset into AnimationGenerator window.
    /// </summary>
    [CreateAssetMenu(menuName = "Animation Spritesheet Configuration", order = 400)]
    internal class SpriteSheetConfiguration : ScriptableObject
    {
        [SerializeField] private List<AnimationDetails> _animations = new List<AnimationDetails>();
        internal List<AnimationDetails> Animations { get => _animations; }

        [OnOpenAsset]
        private static bool OpenSpriteSheet(int instanceID, int line)
        {
            SpriteSheetConfiguration config = EditorUtility.InstanceIDToObject(instanceID) as SpriteSheetConfiguration;
            if (config)
            {
                AnimationGeneratorWindow window = EditorWindow.GetWindow(typeof(AnimationGeneratorWindow)) as AnimationGeneratorWindow;
                window.Config = (SpriteSheetConfiguration)EditorUtility.InstanceIDToObject(instanceID);
                return true;
            }
            return false;
        }
    }
}