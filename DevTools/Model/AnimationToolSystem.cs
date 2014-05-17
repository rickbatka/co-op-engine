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

        public AnimationToolSystem()
        {
            lastHit = DateTime.Now;
            animations = new Dictionary<int, LightAnimation[]>();
            CurrentAnimation = 0;
            CurrentDirection = 0;
            Timescale = 1;
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

        public void RecompileReload(ContentManager content, GraphicsDevice device)
        {
            //unload
            content.Unload();
            //delete
            File.Delete(compiledName);
            //recompile
            BuildAndLoad(FileName, content, device);
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
        }

        public void SaveMetaData()
        {
            string[] lines = MetaFileAnimationManager.BuildFileLinesFromAnimation(animations);

            string metaFileName = GetMetaFileNameFromTextureFile(new FileInfo(FileName));

            File.Copy(metaFileName, FileName + "_bak", true);
            File.WriteAllLines(metaFileName, lines);
        }

        internal void LoadTexture(string filename, ContentManager content, GraphicsDevice device)
        {
            FileName = filename;
            BuildAndLoad(filename, content, device);

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

        internal void CreateNewMetaData(FileInfo info)
        {
            string baseName = info.Name.Replace(info.Extension, "");
            string dataName = info.DirectoryName + "\\" + baseName + "Data.txt";

            //we are building it from the one in memory (UI assumption)

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
            //didn't quite finish tonight... D:
        }
    }
}
