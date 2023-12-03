using System;

namespace Neurophotometrics
{
    public static class ConsoleLogger
    {
        public static void LogError(Exception ex)
        {
            Console.WriteLine($"Error: {ex.StackTrace}\nMessage: {ex.Message}");
        }
        internal static void SuppressError(Exception ex)
        {
            // Method intentionally left empty.
        }
    }
}