using Microsoft.Extensions.FileProviders;
using System.Reflection;
using System.IO;

namespace KenneyAsteroids.Core
{
    public static class Version
    {
        public static string Current
        {
            get
            {
                var assembly = Assembly.GetExecutingAssembly();
                var embeddedProvider = new EmbeddedFileProvider(assembly, "KenneyAsteroids.Core");
                using (var stream = embeddedProvider.GetFileInfo(".version").CreateReadStream())
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadLine();
                }
            }
        }

        public static string Configuration
        {
            get
            {
#if DEBUG
                return "debug";
#elif ALPHA
                return "alpha";
#else
                return "release";
#endif
            }
        }
    }
}