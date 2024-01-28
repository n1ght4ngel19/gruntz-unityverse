using UnityEngine;

namespace HierarchyIcons
{
    public class HierarchyIcon : MonoBehaviour
    {
#if UNITY_EDITOR
        public Texture2D icon;
        
        [Range(-3, 5)]
        public int position;
        public Direction direction = Direction.RightToLeft;

        [TextArea]
        public string tooltip;
        
        public enum Direction
        {
            RightToLeft,
            LeftToRight
        }
#endif
    }
}