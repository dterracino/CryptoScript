using System;

namespace CryptoScript.Core
{
    class Exceptions
    {
        public const string UserInputOnlyToVar = "You can only use readln to assign text to variables.";
        public const string AssignmentNeedsEqual = "You need to use '=' to assign values.";

        public static void Print(string exception)
        {
            Console.WriteLine(exception);
            Environment.Exit(0);
        }
    }
}
