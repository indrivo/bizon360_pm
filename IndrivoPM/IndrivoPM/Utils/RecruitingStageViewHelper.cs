namespace Bizon360.Utils
{
    public class RecruitingStageViewHelper
    {
        public static string Collapsed(bool condition)
        {
            return condition ? "collapsed" : string.Empty;
        }

        public static string Show(bool condition)
        {
            return condition ? "show" : string.Empty;
        }
    }
}
