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

        public void LoadMetaData(string filename)
        {
            string[] animationData = File.ReadAllLines(filename);
            BuildFromAsset(animationData);


            isLoaded = true;
        }

        public void SaveMetaData()
        {
            throw new NotImplementedException();
        }

        internal void LoadTexture(string filename, ContentManager content, GraphicsDevice device)
        {
            FileName = filename;
            BuildAndLoad(filename, content, device);

            //should be UI enforced
            FileInfo fileInfo = new FileInfo(filename);

            string baseName = fileInfo.Name.Replace(fileInfo.Extension, "");

            string dataName = fileInfo.DirectoryName + "\\" + baseName + "Data.txt";
            if (File.Exists(dataName))
            {
                BuildFromAsset(File.ReadAllLines(dataName));
            }
        }

        private TimeSpan HitAndGetInterval()
        {
            DateTime swap = lastHit;
            lastHit = DateTime.Now;
            return lastHit - swap;
        }

        private void BuildFromAsset(string[] lines)
        {
            int animationIndex = 0;
            int directionIndex = 0;

            List<string> currentlyBuildingAnimationLines = new List<string>();
            foreach (var line in lines)
            {
                if (line.StartsWith(";"))
                {
                    if (currentlyBuildingAnimationLines.Count > 0)
                    {
                        if (!animations.ContainsKey(animationIndex))
                        {
                            animations.Add(animationIndex, new LightAnimation[4]);
                        }
                        animations[animationIndex][directionIndex] = BuildFromDataLines(currentlyBuildingAnimationLines.ToArray<string>(), 1f);
                    }
                    var indexes = line.Split(';');
                    animationIndex = int.Parse(indexes[1]);
                    directionIndex = int.Parse(indexes[2]);
                    currentlyBuildingAnimationLines = new List<string>();
                    continue;
                }
                currentlyBuildingAnimationLines.Add(line);
            }

            if (currentlyBuildingAnimationLines.Count > 0)
            {
                //dont forget the last animation! repeated code...
                if (!animations.ContainsKey(animationIndex))
                {
                    animations.Add(animationIndex, new LightAnimation[4]);
                }
                animations[animationIndex][directionIndex] = BuildFromDataLines(currentlyBuildingAnimationLines.ToArray<string>(), 1f);
            }
        }

        private LightAnimation BuildFromDataLines(string[] lineData, float scale)
        {
            List<LightFrame> frameList = new List<LightFrame>();

            for (int i = 0; i < lineData.Length; ++i)
            {
                frameList.Add(LightAnimationsDataReader.BuildFromDataLine(lineData[i], scale));
            }

            return new LightAnimation(frameList);
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
