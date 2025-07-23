namespace MaskMapWizard.Core
{
    public class MaskMapPreset
    {
        public string name;
        public ChannelSource red;
        public ChannelSource green;
        public ChannelSource blue;
        public ChannelSource alpha;
        public bool invertSmoothness;

        public enum ChannelSource
        {
            None,
            Metallic,
            AO,
            Detail,
            Smoothness,
            Roughness // → converti en 1 - roughness si `invertSmoothness` = true
        }

        public MaskMapPreset(string name, ChannelSource r, ChannelSource g, ChannelSource b, ChannelSource a, bool invert = false)
        {
            this.name = name;
            red = r;
            green = g;
            blue = b;
            alpha = a;
            invertSmoothness = invert;
        }
    }
}
