using UnityEngine;
using UnityEditor;
using static MaskMapWizard.Core.MaskMapPreset;

namespace MaskMapWizard.Core
{
    public static class MaskMapBuilder
    {
        public static void Generate(MaskMapPreset preset, Texture2D metallic, Texture2D ao, Texture2D detail, Texture2D smoothness)
        {
            // 1. Résolution (prend la plus grande texture non null)
            int width = GetWidth(metallic, ao, detail, smoothness);
            int height = GetHeight(metallic, ao, detail, smoothness);

            if (width == 0 || height == 0)
            {
                Debug.LogError("No valid texture provided.");
                return;
            }

            // 2. Fallback noir
            metallic = metallic ?? CreateBlackTexture(width, height);
            ao = ao ?? CreateBlackTexture(width, height);
            detail = detail ?? CreateBlackTexture(width, height);
            smoothness = smoothness ?? CreateBlackTexture(width, height);

            // 3. Nouvelle texture
            Texture2D maskMap = new Texture2D(width, height, TextureFormat.RGBA32, false);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixel = new Color(
                        GetChannelValue(preset.red, x, y, metallic, ao, detail, smoothness, preset),
                        GetChannelValue(preset.green, x, y, metallic, ao, detail, smoothness, preset),
                        GetChannelValue(preset.blue, x, y, metallic, ao, detail, smoothness, preset),
                        GetChannelValue(preset.alpha, x, y, metallic, ao, detail, smoothness, preset)
                    );
                    maskMap.SetPixel(x, y, pixel);
                }
            }

            maskMap.Apply();

            // 4. Sauvegarde
            SaveTexture(maskMap);
        }

        static float GetChannelValue(ChannelSource source, int x, int y, Texture2D metallic, Texture2D ao, Texture2D detail, Texture2D smooth, MaskMapPreset preset)
        {
            switch (source)
            {
                case ChannelSource.Metallic: return metallic.GetPixel(x, y).r;
                case ChannelSource.AO: return ao.GetPixel(x, y).r;
                case ChannelSource.Detail: return detail.GetPixel(x, y).r;
                case ChannelSource.Smoothness: return smooth.GetPixel(x, y).r;
                case ChannelSource.Roughness:
                    float val = smooth.GetPixel(x, y).r;
                    return preset.invertSmoothness ? 1f - val : val;
                case ChannelSource.None: return 0f;
                default: return 0f;
            }
        }

        static int GetWidth(params Texture2D[] textures)
        {
            foreach (var tex in textures)
                if (tex != null) return tex.width;
            return 0;
        }

        static int GetHeight(params Texture2D[] textures)
        {
            foreach (var tex in textures)
                if (tex != null) return tex.height;
            return 0;
        }

        static Texture2D CreateBlackTexture(int width, int height)
        {
            Texture2D black = new Texture2D(width, height, TextureFormat.RGBA32, false);
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < pixels.Length; i++) pixels[i] = Color.black;
            black.SetPixels(pixels);
            black.Apply();
            return black;
        }

        static void SaveTexture(Texture2D texture)
        {
            string path = EditorUtility.SaveFilePanel("Save MaskMap", "Assets/", "maskmap", "png");
            if (string.IsNullOrEmpty(path)) return;

            byte[] pngData = texture.EncodeToPNG();
            System.IO.File.WriteAllBytes(path, pngData);
            AssetDatabase.Refresh();
            Debug.Log("MaskMap saved to: " + path);
        }
    }
}
