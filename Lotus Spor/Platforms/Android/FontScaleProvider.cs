using Android.Content.Res;
using Lotus_Spor;
using Lotus_Spor.Services;

namespace Lotus_Spor.Platforms.Android
{
    public class FontScaleProvider : IFontScaleProvider
    {
        public float GetFontScale()
        {
            return Resources.System.Configuration.FontScale;
        }
    }
}
