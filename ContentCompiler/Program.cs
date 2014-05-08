using ContentCompiler.ContentCompilation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 2)
            {
                string inputDirectory = args[0];
                string outputDirectory = args[1];

                ContentBuilder builder = new ContentBuilder();
                builder.Clear();
                builder.OnOutput += WriteLine;
                builder.BuildAssets(inputDirectory, outputDirectory, false);
                builder.OnOutput -= WriteLine;
            }
            else
            {
                Console.WriteLine("Usage: ContentCompiler.exe [ ? | inputDirectory outputDirectory ]");
            }
        }

        private static void WriteLine(object sender, ContentBuildEventArgs e)
        {
            Console.WriteLine(e.Text);
        }
    }
}
