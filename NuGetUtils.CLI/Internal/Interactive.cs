using System;
using System.Threading.Tasks;

namespace NuGetUtils.CLI.Internal
{
    /// <summary>
    /// Source: https://github.com/dotnet/command-line-api/issues/711
    /// </summary>
    internal class Interactive
    {
        internal static async Task<bool> Confirmation(string message, string positiveText, string negativeText)
        {
            bool repeat = true;
            while (repeat)
            {
                await Task.Delay(100);
                Console.WriteLine($"{message} ({positiveText}/{negativeText})");
                string output = Console.ReadLine();
                if (output == null)
                {
                    return false;
                }

                output = output.Trim().ToLower();
                if (output.Length <= 0)
                {
                    continue;
                }
                else
                {
                    return output == positiveText || output == positiveText[0].ToString();
                }
            }

            return false;
        }
    }
}
