using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.IO;
using ContentCompiler.ContentCompilation;

namespace DevTools.Model
{
    class AnimationToolSystem
    {
        public event EventHandler OnModelChanged;

        //current file
        string compiledName;

        public Dictionary<int, LightAnimation[]> animations;
        public int CurrentDirection;
        public int CurrentAnimation;

        public float Timescale { get; set; }
        public string FileName { get; set; }

        private bool isLoaded = false;
        private DateTime lastHit;
        private Texture2D currentTexture;
        private Texture2D debugTex;
        private FileSystemWatcher watcher;

        private ContentManager Content;
        private GraphicsDevice Device;
        bool hasLoadedContentBefore = false;

        public AnimationToolSystem()
        {
            lastHit = DateTime.Now;
            animations = new Dictionary<int, LightAnimation[]>();
            CurrentAnimation = 0;
            CurrentDirection = 0;
            Timescale = 1;
        }

        public void LoadContent(ContentManager contentmgr, GraphicsDevice device)
        {
            Content = contentmgr;
            Device = device;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            TimeSpan elapsedTime = HitAndGetInterval();
            if (currentTexture != null && animations[CurrentAnimation] != null)
            {
                animations[CurrentAnimation][CurrentDirection].DrawAndUpdate(spriteBatch, elapsedTime, currentTexture, Timescale);
            }
        }

        internal void DrawPhysics(SpriteBatch spriteBatch)
        {
            animations[CurrentAnimation][CurrentDirection].DrawPhysics(spriteBatch, debugTex);
        }

        internal void DrawDamageDots(SpriteBatch spriteBatch)
        {
            animations[CurrentAnimation][CurrentDirection].DrawDamageDots(spriteBatch, debugTex);
        }

        internal void DrawSelection(SpriteBatch spriteBatch, Rectangle CurrentSelection)
        {
            spriteBatch.Draw(debugTex, CurrentSelection, Color.Red);
        }

        public void RecompileReload()
        {
            //unload
            Content.Unload();
            //delete
            File.Delete(compiledName);
            //recompile
            BuildAndLoad(FileName, Content, Device);
        }

        private void BuildAndLoad(string filename, ContentManager content, GraphicsDevice device)
        {
            content.Unload();

            if (File.Exists(compiledName))
            {
                File.Delete(compiledName);
            }

            ContentBuilder builder = new ContentBuilder();
            builder.BuildSingleAsset(FileName);

            FileInfo info = new FileInfo(filename);
            FileInfo compiledFileInfo = new FileInfo(info.Directory.FullName + "\\temp.xnb");

            compiledName = compiledFileInfo.FullName;

            content.RootDirectory = info.Directory.FullName;
            currentTexture = content.Load<Texture2D>("temp");
            debugTex = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
            uint[] white = new uint[1];
            white[0] = Color.White.PackedValue;
            debugTex.SetData<uint>(white);

            CreateFileWatcher(info);
        }

        public void SaveMetaData()
        {
            string[] lines = MetaFileAnimationManager.BuildFileLinesFromAnimation(animations);

            string metaFileName = GetMetaFileNameFromTextureFile(new FileInfo(FileName));

            File.Copy(metaFileName, FileName + "_bak", true);
            File.WriteAllLines(metaFileName, lines);
        }

        internal void LoadTexture(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);

            if (!hasLoadedContentBefore)
            {
                hasLoadedContentBefore = true;
                Content.RootDirectory = fileInfo.Directory.FullName;
            }

            FileName = fileInfo.FullName;
            BuildAndLoad(FileName, Content, Device);
        }

        internal void LoadMetaData(string filename)
        {
            FileInfo fileInfo = new FileInfo(filename);

            string dataName = GetMetaFileNameFromTextureFile(fileInfo);
            if (File.Exists(dataName))
            {
                string[] lines = File.ReadAllLines(dataName);
                animations = MetaFileAnimationManager.BuildAnimationsFromFile(lines);
            }
        }

        private string GetMetaFileNameFromTextureFile(FileInfo textureName)
        {
            return textureName.DirectoryName + "\\" + textureName.Name.Replace(textureName.Extension, "") + "Data.txt";
        }

        internal void CreateNewMetaData(string fileName)
        {
            throw new NotImplementedException();
            //FileInfo fileInfo = new FileInfo(fileName);

            //string baseName = fileInfo.Name.Replace(fileInfo.Extension, "");
            //string dataName = fileInfo.DirectoryName + "\\" + baseName + "Data.txt";

            ////we are building it from the one in memory (UI assumption)

        }

        private TimeSpan HitAndGetInterval()
        {
            DateTime swap = lastHit;
            lastHit = DateTime.Now;
            return lastHit - swap;
        }

        internal int GetAnimationLength()
        {
            if (currentTexture != null && animations[CurrentAnimation] != null)
            {
                return animations[CurrentAnimation][CurrentDirection].FrameCount;
            }
            else
            {
                return 0;
            }
        }

        internal int GetCurrentFrameIndex()
        {
            if (currentTexture != null && animations[CurrentAnimation] != null)
            {
                return animations[CurrentAnimation][CurrentDirection].currentFrameIndex;
            }
            else return 0;
        }

        internal void SetFrameIndex(int value)
        {
            animations[CurrentAnimation][CurrentDirection].SetIndex(value);
        }

        private void CreateFileWatcher(FileInfo file)
        {
            if (watcher != null)
            {
                watcher.Dispose();
            }

            watcher = new FileSystemWatcher();
            watcher.Path = file.DirectoryName;
            watcher.NotifyFilter = NotifyFilters.LastWrite; //can have more later | NotifyFilters.LastAccess | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = file.Name;

            watcher.Changed += new FileSystemEventHandler(OnChanged);

            watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
        }

        internal void AddFrame()
        {
            throw new NotImplementedException();
        }
    }
}
