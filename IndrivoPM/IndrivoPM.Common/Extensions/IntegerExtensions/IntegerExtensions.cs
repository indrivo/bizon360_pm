namespace Gear.Common.Extensions.IntegerExtensions
{
    public static class IntegerExtensions
    {
        public static ColorByValue GetProgressBarColor(this int value)
        {
            if (value == 100)
                return ColorByValue.Green;
            else if (value > 60)
                return ColorByValue.Yellow;
            else if (value > 30)
                return ColorByValue.Orange;
            else return ColorByValue.Red;
        }
    }

    public enum ColorByValue
    {
        Red,
        Orange,
        Yellow,
        Green
    }
}
