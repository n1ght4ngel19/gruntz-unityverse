#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Shared_TextureLoader
    {
        #region Properties
        private static Dictionary<string, Texture2D> textureCache = new Dictionary<string, Texture2D>();
        #endregion

        public static Texture2D LoadTexture(string textureName)
        {
            if (!textureCache.TryGetValue(textureName, out Texture2D texture))
            {
                texture = Resources.Load<Texture2D>(textureName);
                if (texture != null)
                {
                    textureCache[textureName] = texture;
                }
                else
                {
                    texture = CreateFallbackTexture(2, 2, Color.white);
                }
            }
            return texture;
        }

        private static Texture2D CreateFallbackTexture(int width, int height, Color color)
        {
            Texture2D texture = new Texture2D(width, height);

            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }

        public static bool IsTextureLoaded(string textureName)
        {
            return textureCache.ContainsKey(textureName) && textureCache[textureName] != null;
        }
    }
}
#endif