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
        Random rand;
        string compiledName;

        public Dictionary<int, LightAnimation[]> animations;
        public int CurrentDirectionIndex;
        public int CurrentAnimationIndex;

        public float Timescale { get; set; }
        public string FileName { get; set; }
        public int GridSize { get; set; }

        private LightAnimation[] CurrentDirection
        {
            get { return animations[CurrentAnimationIndex]; }
        }

        private LightAnimation CurrentAnimation
        {
            get { return animations[CurrentAnimationIndex][CurrentDirectionIndex]; }
        }

        private bool isLoaded = false;
        private DateTime lastHit;
        private Texture2D currentTexture;
        private Texture2D debugTex;
        private Texture2D rectangle;
        private FileSystemWatcher watcher;

        private ContentManager Content;
        private GraphicsDevice Device;
        bool hasLoadedContentBefore = false;

        public AnimationToolSystem()
        {
            rand = new Random();
            lastHit = DateTime.Now;
            animations = new Dictionary<int, LightAnimation[]>();
            CurrentAnimationIndex = 0;
            CurrentDirectionIndex = 0;
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
            if (currentTexture != null && animations[CurrentAnimationIndex] != null)
            {
                animations[CurrentAnimationIndex][CurrentDirectionIndex].DrawAndUpdate(spriteBatch, elapsedTime, currentTexture, Timescale, rectangle);
            }
        }

        internal void DrawPhysics(SpriteBatch spriteBatch)
        {
            animations[CurrentAnimationIndex][CurrentDirectionIndex].DrawPhysics(spriteBatch, debugTex, GetRandColor());
        }

        internal void DrawDamageDots(SpriteBatch spriteBatch)
        {
            animations[CurrentAnimationIndex][CurrentDirectionIndex].DrawDamageDots(spriteBatch, debugTex, GetRandColor());
        }

        internal void DrawSelection(SpriteBatch spriteBatch, Rectangle CurrentSelection)
        {
            spriteBatch.Draw(debugTex, CurrentSelection, Color.Red);
        }

        private Color GetRandColor()
        {
            return new Color(rand.Next(1, 255), rand.Next(1, 255), rand.Next(1, 255));
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

            rectangle = content.Load<Texture2D>("grid");

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
            if (currentTexture != null && animations[CurrentAnimationIndex] != null)
            {
                return animations[CurrentAnimationIndex][CurrentDirectionIndex].FrameCount;
            }
            else
            {
                return 0;
            }
        }

        internal int GetCurrentFrameIndex()
        {
            if (currentTexture != null && animations[CurrentAnimationIndex] != null)
            {
                return animations[CurrentAnimationIndex][CurrentDirectionIndex].currentFrameIndex;
            }
            else return 0;
        }

        internal LightFrame GetCurrentFrame()
        {
            return animations[CurrentAnimationIndex][CurrentDirectionIndex].CurrentFrame;
        }

        internal void SetFrameIndex(int value)
        {
            animations[CurrentAnimationIndex][CurrentDirectionIndex].SetIndex(value);
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

        internal void CreateNewFrame()
        {
            LightFrame newFrame = new LightFrame()
            {
                DamageDots = new Rectangle[0],
                DrawRectangle = currentTexture.Bounds,
                FrameTime = 50,
                PhysicsRectangle = Rectangle.Empty,
                SourceRectangle = currentTexture.Bounds,
            };

            CurrentAnimation.frames.Insert(GetCurrentFrameIndex(), newFrame);
        }

        internal void RemoveFrame()
        {
            if(CurrentAnimation.frames.Count > 1)
            {
                CurrentAnimation.frames.Remove(CurrentAnimation.CurrentFrame);
            }
        }

    }
}
