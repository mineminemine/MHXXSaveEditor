using System;
using System.Drawing;

namespace MHXXSaveEditor.Util
{
    class ColorBrightness
    {
        public int PerceivedBrightness(Color c)
        {
            return (int)Math.Sqrt(
            c.R * c.R * .241 +
            c.G * c.G * .691 +
            c.B * c.B * .068);
        }
    }
}
