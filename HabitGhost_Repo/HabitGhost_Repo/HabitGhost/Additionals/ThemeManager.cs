using System;

namespace HabitGhost.Additionals
{
    public static class ThemeManager
    {
        public static event EventHandler ThemeChanged;
        private static ColorScheme _currentTheme = ColorScheme.Light;

        public static ColorScheme CurrentTheme
        {
            get => _currentTheme;
            set
            {
                if (_currentTheme != value)
                {
                    _currentTheme = value;
                    ThemeChanged?.Invoke(null, EventArgs.Empty);
                }
            }
        }
    }
}
