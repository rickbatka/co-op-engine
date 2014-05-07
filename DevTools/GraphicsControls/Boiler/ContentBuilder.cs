﻿using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace DevTools.GraphicsControls.Boiler
{
    class ContentBuilder : IDisposable
    {
        const string xnaVersion = ", Version=4.0.0.0, PublicKeyToken=842cf8be1de50553";

        static string[] pipelineAssemblies =
        {
            //"Microsoft.Xna.Framework.Content.Pipeline.FBXImporter" + xnaVersion,
            //"Microsoft.Xna.Framework.Content.Pipeline.XImporter" + xnaVersion,
            //"Microsoft.Xna.Framework.Content.Pipeline.TextureImporter" + xnaVersion,
            //"Microsoft.Xna.Framework.Content.Pipeline.EffectImporter" + xnaVersion,
            @"Microsoft.Xna.Framework.Content.Pipeline.TextureImporter, Version=4.0.0.0, Culture=neutral, PublicKeyToken=842cf8be1de50553, processorArchitecture=MSIL",
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
            buildProject.SetProperty("XnaProfile", "HiDef");
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
            else
            {
                return OutputDirectory;
            }
            
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

        void PurgeStaleTempDirectories()
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
    }
}
