namespace Neurophotometrics.Design.V1
{
    public static class KeyFilteringHelpers
    {
        public static bool IsIntegerNumeric(char keyChar)
        {
            if (char.IsDigit(keyChar) || char.IsControl(keyChar))
                return true;
            else
                return false;
        }

        public static bool IsDecimalNumeric(char keyChar)
        {
            if (char.IsDigit(keyChar) || char.IsControl(keyChar) || keyChar == '.')
                return true;
            else
                return false;
        }
    }
}