using System.Collections.Generic;
using static MaskMapWizard.Core.MaskMapPreset;

namespace MaskMapWizard.Core
{
    public static class PresetDatabase
    {
        public static List<MaskMapPreset> Presets = new List<MaskMapPreset>()
        {
            new MaskMapPreset(
                name: "HDRP",
                r: ChannelSource.Metallic,
                g: ChannelSource.AO,
                b: ChannelSource.Detail,
                a: ChannelSource.Smoothness,
                invert: false
            ),
            new MaskMapPreset(
                name: "URP",
                r: ChannelSource.Metallic,
                g: ChannelSource.None,
                b: ChannelSource.None,
                a: ChannelSource.Smoothness,
                invert: false
            )
        };

        public static MaskMapPreset GetByIndex(int index)
        {
            if (index < 0 || index >= Presets.Count) return null;
            return Presets[index];
        }

        public static string[] GetPresetNames()
        {
            return Presets.ConvertAll(p => p.name).ToArray();
        }
    }
}
