using UnityEditor;
using UnityEngine;

namespace MaskMapWizard.Core
{
    public static class TextureUtils
    {
        public static void WithReadable(Texture2D tex, System.Action action)
        {
            if (tex == null)
            {
                action?.Invoke();
                return;
            }

            string path = AssetDatabase.GetAssetPath(tex);
            if (string.IsNullOrEmpty(path)) return;

            var importer = (TextureImporter)AssetImporter.GetAtPath(path);
            if (importer == null) return;

            bool wasReadable = importer.isReadable;
            if (!wasReadable)
            {
                importer.isReadable = true;
                importer.SaveAndReimport();
            }

            action?.Invoke();

            if (!wasReadable)
            {
                importer.isReadable = false;
                importer.SaveAndReimport();
            }
        }
    }
}
