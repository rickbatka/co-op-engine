using ContentCompiler.ContentCompilation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentCompiler
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length >= 2)
            {
                string inputDirectory = args[0];
                string outputDirectory = args[1];

                if (!Directory.Exists(outputDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                    catch
                    {
                        return 1;
                    }
                }
                else if(Directory.Exists(inputDirectory))
                {
                    Console.WriteLine(string.Format("Compiling\n{0}\nto\n{1}",inputDirectory, outputDirectory));
                    ContentBuilder builder = new ContentBuilder();
                    builder.Clear();
                    builder.OnOutput += WriteLine;
                    builder.BuildAssets(inputDirectory, outputDirectory, false);
                    builder.OnOutput -= WriteLine;

                    DirectoryInfo inputDirectoryInfo = new DirectoryInfo(inputDirectory);
                    List<FileInfo> moveFiles = new List<FileInfo>();
                    moveFiles.AddRange(inputDirectoryInfo.GetFiles("*.spritefont"));
                    moveFiles.AddRange(inputDirectoryInfo.GetFiles("*.txt"));
                    moveFiles.AddRange(inputDirectoryInfo.GetFiles("*.xnb"));//hack, still can't compile spritefonts


                    foreach (FileInfo filename in moveFiles)
                    {
                        Console.WriteLine(filename.Name);
                        filename.CopyTo(outputDirectory + "\\" + filename.Name, true);
                    }
                }
                else
                {
                    Console.WriteLine("Input directory does not exist");
                    return 1;
                }
            }
            else
            {
                Console.WriteLine("Usage: ContentCompiler.exe [ ? | inputDirectory outputDirectory ]");
                return 1;
            }
            return 0;
        }

        private static void WriteLine(object sender, ContentBuildEventArgs e)
        {
            Console.WriteLine(e.Text);
        }
    }
}
