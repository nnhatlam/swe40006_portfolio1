namespace Calculator
{
    public class CatHelper
    {
        public static string GetCatMood(int happinessLevel)
        {
            if (happinessLevel > 7)
            {
                return "Happy";
            }
            else if (happinessLevel > 4)
            {
                return "Content";
            }
            else
            {
                return "Grumpy";
            }
        }
    }
}
