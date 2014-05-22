using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ContentCompiler.ContentCompilation
{
    public class ContentBuilder : IDisposable
    {
        public event EventHandler<ContentBuildEventArgs> OnOutput;

        static string[] pipelineAssemblies =
        {
            @"Microsoft.Xna.Framework.Content.Pipeline.EffectImporter, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553, processorArchitecture=MSIL",
            @"Microsoft.Xna.Framework.Content.Pipeline.FBXImporter, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553, processorArchitecture=MSIL",
            @"Microsoft.Xna.Framework.Content.Pipeline.TextureImporter, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553, processorArchitecture=MSIL",
            @"Microsoft.Xna.Framework.Content.Pipeline.XImporter, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553, processorArchitecture=MSIL",
            @"Microsoft.Xna.Framework.Content.Pipeline.AudioImporters, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553, processorArchitecture=MSIL",
            @"Microsoft.Xna.Framework.Content.Pipeline.VideoImporters, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553, processorArchitecture=MSIL",
        };

        Project buildProject;
        ProjectRootElement projectRootElement;
        BuildParameters buildParameters;
        List<ProjectItem> projectItems = new List<ProjectItem>();
        ErrorLogger errorLogger;

        string buildDirectory;
        string processDirectory;
        string baseDirectory;

        static int directorySalt;

        bool isDisposed;

        public string OutputDirectory
        {
            get { return Path.Combine(buildDirectory, "bin/Content"); }
        }

        public ContentBuilder()
        {
            CreateTempDirectory();
            CreateBuildProject();
        }

        ~ContentBuilder()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                isDisposed = true;

                DeleteTempDirectory();
            }
        }

        void CreateBuildProject()
        {
            string projectPath = Path.Combine(buildDirectory, "content.contentproj");
            string outputPath = Path.Combine(buildDirectory, "bin");

            projectRootElement = ProjectRootElement.Create(projectPath);
            //projectRootElement.AddImport(@"$(MSBuildExtensionsPath)\Microsoft\XNA Game Studio\$(XnaFrameworkVersion)\Microsoft.Xna.GameStudio.ContentPipeline.targets");
            projectRootElement.AddImport("$(MSBuildExtensionsPath)\\Microsoft\\XNA Game Studio\\" +
                                         "v4.0\\Microsoft.Xna.GameStudio.ContentPipeline.targets");
            buildProject = new Project(projectRootElement);
            buildProject.SetProperty("XnaPlatform", "Windows");
            buildProject.SetProperty("XnaProfile", "Reach");
            //buildProject.SetProperty("Platform", "x86");
            buildProject.SetProperty("XnaFrameworkVersion", "v4.0");
            buildProject.SetProperty("TargetFrameworkVersion", "v4.0");
            buildProject.SetProperty("Configuration", "Release");
            buildProject.SetProperty("OutputPath", outputPath);

            foreach (string pipelineAssembly in pipelineAssemblies)
            {
                buildProject.AddItem("Reference", pipelineAssembly);
            }

            errorLogger = new ErrorLogger();
            buildParameters = new BuildParameters(ProjectCollection.GlobalProjectCollection);
            buildParameters.Loggers = new ILogger[] { errorLogger };
        }

        public void Add(string filename, string name, string importer, string processor)
        {
            ProjectItem item = buildProject.AddItem("Compile", filename)[0];

            item.SetMetadataValue("Link", Path.GetFileName(filename));
            item.SetMetadataValue("Name", name);

            if (!string.IsNullOrEmpty(importer))
                item.SetMetadataValue("Importer", importer);

            if (!string.IsNullOrEmpty(processor))
                item.SetMetadataValue("Processor", processor);

            //File.Copy(filename, item.Metadata

            projectItems.Add(item);
        }

        public void Clear()
        {
            buildProject.RemoveItems(projectItems);

            projectItems.Clear();
        }

        public string Build()
        {
            errorLogger.Errors.Clear();

            BuildManager.DefaultBuildManager.BeginBuild(buildParameters);

            BuildRequestData request = new BuildRequestData(buildProject.CreateProjectInstance(), new string[0]);
            BuildSubmission submission = BuildManager.DefaultBuildManager.PendBuildRequest(request);

            submission.ExecuteAsync(null, null);
            submission.WaitHandle.WaitOne();
            BuildManager.DefaultBuildManager.EndBuild();

            if (submission.BuildResult.OverallResult == BuildResultCode.Failure)
            {
                return string.Join("\n", errorLogger.Errors.ToArray());
            }
            return null;
        }

        void CreateTempDirectory()
        {
            baseDirectory = Path.Combine(Path.GetTempPath(), GetType().FullName);
            int processId = Process.GetCurrentProcess().Id;
            processDirectory = Path.Combine(baseDirectory, processId.ToString());
            directorySalt++;
            buildDirectory = Path.Combine(processDirectory, directorySalt.ToString());
            Directory.CreateDirectory(buildDirectory);
            PurgeStaleTempDirectories();
        }

        void DeleteTempDirectory()
        {
            Directory.Delete(buildDirectory, true);
            if (Directory.GetDirectories(processDirectory).Length == 0)
            {
                Directory.Delete(processDirectory);
                if (Directory.GetDirectories(baseDirectory).Length == 0)
                {
                    Directory.Delete(baseDirectory);
                }
            }
        }

        private void PurgeStaleTempDirectories()
        {
            foreach (string directory in Directory.GetDirectories(baseDirectory))
            {
                int processId;
                if (int.TryParse(Path.GetFileName(directory), out processId))
                {
                    try
                    {
                        Process.GetProcessById(processId);
                    }
                    catch (ArgumentException)
                    {
                        Directory.Delete(directory, true);
                    }
                }
            }
        }

        public void ClearDirectory(string outputDirectory)
        {
                DirectoryInfo clearDirectoryInfo = new DirectoryInfo(outputDirectory);
                try
                {
                    FileInfo[] clearingFiles = clearDirectoryInfo.GetFiles("*.xnb");

                    foreach (FileInfo file in clearingFiles)
                    {
                        file.Delete();
                    }
                }
                catch
                { }
        }

        public void GatherInputFiles(string inputDirectory)
        {
            Clear();

            DirectoryInfo dinfo = new DirectoryInfo(inputDirectory);
            FileInfo[] textureInfos = dinfo.GetFiles("*.png"); //there should be an expression for this, but explorer was being an ass
            textureInfos.ToList().AddRange(dinfo.GetFiles("*.jpg"));
            textureInfos.ToList().AddRange(dinfo.GetFiles("*.tif"));
            textureInfos.ToList().AddRange(dinfo.GetFiles("*.bmp"));

            foreach (FileInfo finfo in textureInfos)
            {
                TriggerOutput(finfo.FullName);
                Add(finfo.FullName, finfo.Name.Replace(finfo.Extension, ""), "TextureImporter", "TextureProcessor");
            }

            FileInfo[] audioEffect = dinfo.GetFiles("*.wav");

            foreach (FileInfo finfo in audioEffect)
            {
                TriggerOutput(finfo.FullName);
                Add(finfo.FullName, finfo.Name.Replace(finfo.Extension, ""), "WavImporter", "SoundEffectProcessor");
            }

            //I have distain for them making the distinction between song and soundeffect as song uses winmediaplayer backend to play it and it's got a gross api
            FileInfo[] songs = dinfo.GetFiles("*.mp3");

            foreach (FileInfo finfo in songs)
            {
                TriggerOutput(finfo.FullName);
                Add(finfo.FullName, finfo.Name.Replace(finfo.Extension, ""), "Mp3Importer", "SongProcessor");
            }
        }

        private void MoveBuiltFilesToOutputDirectory(string outputDirectory)
        {
            DirectoryInfo builtDirectoryInfo = new DirectoryInfo(buildDirectory);
            FileInfo[] builtFiles = builtDirectoryInfo.GetFiles("*.xnb", SearchOption.AllDirectories);

            foreach (FileInfo builtFile in builtFiles)
            {
                bool fileExists = File.Exists(outputDirectory + "\\" + builtFile.Name);// builtFile.Exists;
                builtFile.CopyTo(outputDirectory + "\\" + builtFile.Name, true);
                TriggerOutput(builtFile.Name + (fileExists == true ? " was overwritten" : string.Empty));
            }
        }

        public void BuildSingleAsset(string filename)
        {
            Clear();
            Add(filename, "temp", "TextureImporter", "TextureProcessor");
            string output = string.Empty;

            if (Build() == null)
            {
                FileInfo info = new FileInfo(buildDirectory + "\\temp.xnb");
                output = info.FullName;
            }
            else
            {
                throw new Exception("error");
            }

            FileInfo inputFileInfo = new FileInfo(filename);

            MoveBuiltFilesToOutputDirectory(inputFileInfo.DirectoryName);
        }
        
        public void BuildAssets(string inputDirectory, string outputDirectory, bool clear)
        {
            ClearDirectory(OutputDirectory);
            TriggerOutput("Gathering Files...");

            GatherInputFiles(inputDirectory);

            string builtText = Build();

            if (builtText == null)
            {
                TriggerOutput("\n - Done Building - \n\nCopying Files to:\n" + outputDirectory + "\n");

                MoveBuiltFilesToOutputDirectory(outputDirectory);
            }
            else
            {
                foreach (string error in errorLogger.Errors)
                {
                    TriggerOutput(error);
                }
            }
        }

        private void TriggerOutput(string text)
        {
            if (OnOutput != null)
            {
                OnOutput.Invoke(this, new ContentBuildEventArgs(text));
            }
        }
    }

    public class ContentBuildEventArgs : EventArgs
    {
        public string Text { get; private set; }

        public ContentBuildEventArgs(string outputText)
        {
            Text = outputText;
        }
    }
}
