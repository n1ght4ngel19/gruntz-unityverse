#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;

namespace Verpha.HierarchyDesigner
{
    internal static class HierarchyDesigner_Shared_TextureLoader
    {
        #region Properties
        private static Dictionary<string, Font> fontCache = new Dictionary<string, Font>();

        private static Dictionary<string, Texture2D> textureCache = new Dictionary<string, Texture2D>();
        #endregion

        #region Methods
        public static Font LoadFont(string fontName)
        {
            if (!fontCache.TryGetValue(fontName, out Font font))
            {
                font = Resources.Load<Font>(fontName);
                if (font != null)
                {
                    fontCache[fontName] = font;
                }
                else
                {
                    font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                }
            }
            return font;
        }

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
            Debug.Log("<color=#FFCE74>HierarchyDesigner_Shared_ImportReload.cs</color> is <color=#FF7674>obsolete</color>. Please <color=#FF7674>delete</color> it from the Hierarchy Designer folder (<color=#FF7674>DELETE: Assets/.../Hierarchy Designer/Editor/Scripts/HierarchyDesigner_Shared_ImportReload.cs</color>).");
            return true;
        }
        #endregion
    }
}
#endif