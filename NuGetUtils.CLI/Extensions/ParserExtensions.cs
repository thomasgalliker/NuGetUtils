using System;
using System.CommandLine.Parsing;
using System.Threading.Tasks;

namespace NuGetUtils.CLI.Extensions
{
    internal static class ParserExtensions
    {
        internal static async Task<int> RunInteractiveMode(this Parser parser)
        {
            int result = 0;

            while (true)
            {
                Console.Write("\n> ");
                var cmd = Console.ReadLine();
                if (cmd == "exit")
                {
                    break;
                }

                if (cmd != "clear")
                {
                    if (cmd is null)
                    {
                        continue;
                    }

                    result = await parser.InvokeAsync(cmd);
                }
                else
                {
                    Console.Clear();
                    continue;
                }
            }

            return result;
        }

    }
}
