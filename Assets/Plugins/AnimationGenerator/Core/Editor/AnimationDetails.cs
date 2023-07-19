using System;
using UnityEngine;

namespace KevinCastejon.AnimationGenerator
{
    [Serializable]
    internal class AnimationDetails
    {
        [SerializeField] private string _name;
        [SerializeField] private int _length;
        [SerializeField] private bool _loop;
        [SerializeField] private bool _autoFramerate;
        [SerializeField] private int _frameRate;

        internal AnimationDetails()
        {
            _name = "AnimationClipName";
            _length = 1;
            _loop = true;
            _autoFramerate = true;
            _frameRate = 60;
        }

        internal string Name { get => _name; }
        internal int Length { get => _length; }
        internal bool Loop { get => _loop; }
        internal bool AutoFramerate { get => _autoFramerate; }
        internal int FrameRate { get => _frameRate; }
    }
}