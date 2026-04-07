using System.Drawing;

namespace HabitGhost.Additionals
{
    public class ColorScheme
    {
        public static ColorScheme Dark => new ColorScheme
        {
            Background = Color.FromArgb(33, 33, 33),
            Text = Color.Gainsboro,
            Accent = Color.DodgerBlue,
            Secondary = Color.FromArgb(55, 55, 55)
        };

        public static ColorScheme Light => new ColorScheme
        {
            Background = Color.WhiteSmoke,
            Text = Color.Black,
            Accent = Color.CornflowerBlue,
            Secondary = Color.LightGray
        };

        public Color Background { get; set; }
        public Color Text { get; set; }
        public Color Accent { get; set; }
        public Color Secondary { get; set; }
        public Color ControlBackground { get; set; }
    }
}
