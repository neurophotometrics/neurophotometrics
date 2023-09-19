using System;

namespace Neurophotometrics.Design
{
    public static class ConsoleLogger
    {
        public static void LogError(Exception ex)
        {
            Console.WriteLine($"\nError: {ex.GetType()}\nTrace: {ex.StackTrace}\nMessage: {ex.Message}");
        }

        internal static void SuppressError(ObjectDisposedException ex)
        {
            // Method intentionally left empty.
        }
    }
}