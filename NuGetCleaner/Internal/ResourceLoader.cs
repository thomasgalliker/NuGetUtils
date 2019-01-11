using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NuGetCleaner.Internal
{
    internal static class ResourceLoader
    {
        internal static byte[] GetEmbeddedResourceByteArray(Assembly assembly, string resourceFileName)
        {
            using (var memoryStream = new MemoryStream())
            {
                GetEmbeddedResourceStream(assembly, resourceFileName).CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        private static Stream GetEmbeddedResourceStream(Assembly assembly, string resourceFileName)
        {
            var resources = assembly.GetManifestResourceNames()
                .Where(str => str.EndsWith(resourceFileName, StringComparison.CurrentCultureIgnoreCase))
                .ToArray();

            return assembly.GetManifestResourceStream(resources.Single());
        }
    }
}