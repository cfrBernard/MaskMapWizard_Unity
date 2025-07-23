using UnityEditor;
using UnityEngine;
using MaskMapWizard.Core;

namespace MaskMapWizard.UI
{
    public class MainWindow : EditorWindow
    {
        private int selectedPresetIndex = 0;

        private Texture2D metallicTex;
        private Texture2D aoTex;
        private Texture2D detailTex;
        private Texture2D smoothnessTex;

        [MenuItem("Tools/MaskMap Wizard")]
        public static void ShowWindow()
        {
            GetWindow<MainWindow>("MaskMap Wizard");
        }

        private void OnGUI()
        {
            GUILayout.Space(20);
            selectedPresetIndex = EditorGUILayout.Popup("Pipeline Preset", selectedPresetIndex, PresetDatabase.GetPresetNames());

            GUILayout.Space(50);
            metallicTex = (Texture2D)EditorGUILayout.ObjectField("Metallic", metallicTex, typeof(Texture2D), false);
            aoTex = (Texture2D)EditorGUILayout.ObjectField("Ambient Occlusion", aoTex, typeof(Texture2D), false);
            detailTex = (Texture2D)EditorGUILayout.ObjectField("Detail Mask", detailTex, typeof(Texture2D), false);
            smoothnessTex = (Texture2D)EditorGUILayout.ObjectField("Smoothness / Roughness", smoothnessTex, typeof(Texture2D), false);

            GUILayout.Space(20);

            if (GUILayout.Button("Build MaskMap"))
            {
                var preset = PresetDatabase.GetByIndex(selectedPresetIndex);
                if (preset != null)
                {
                    MaskMapBuilder.Generate(preset, metallicTex, aoTex, detailTex, smoothnessTex);
                }
            }
        }
    }
}
