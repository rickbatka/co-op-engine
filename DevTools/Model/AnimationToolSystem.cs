using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace DevTools.Model
{
    class AnimationToolSystem
    {
        //current file
        string spritesheetFilename;
        string metadataFilename;

        public Dictionary<int, LightAnimation[]> animations;
        public int CurrentDirection;
        public int CurrentAnimation;

        public float Timescale { get; set; }
        public string FileName { get; set; }

        private bool isLoaded = false;
        private DateTime lastHit;
        private Texture2D currentTexture;

        public AnimationToolSystem()
        {
            lastHit = DateTime.Now;
            animations = new Dictionary<int, LightAnimation[]>();
            CurrentAnimation = 0;
            CurrentDirection = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            TimeSpan elapsedTime = HitAndGetInterval();
            if (currentTexture != null && animations[CurrentAnimation] != null)
            {
                animations[CurrentAnimation][CurrentDirection].DrawAndUpdate(spriteBatch, elapsedTime, currentTexture);
            }
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

        internal void LoadTexture(string filename, ContentManager content)
        {
            //should be UI enforced
            FileInfo fileInfo = new FileInfo(filename);
            string baseName = fileInfo.Name.Replace(fileInfo.Extension, "");
            currentTexture = content.Load<Texture2D>(baseName);

            string dataName = "Content/" + baseName + "Data.txt";
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
    }
}
