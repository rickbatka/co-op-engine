using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTools.Model
{
    internal static class MetaFileAnimationManager
    {

        /* * * * * * * * * * * * * * * 
         * File Format
         * 
         * action number;direction;
         * frametime<SourceRectangle>{PhysicsRectangle}(DamangeDots)
         * 
         * there can be only 1 physics rectangle data
         * there can be many damage dots
         */

        public static Dictionary<int, LightAnimation[]> BuildAnimationsFromFile(string[] fileLines)
        {
            Dictionary<int, LightAnimation[]> animations = new Dictionary<int, LightAnimation[]>();

            int animationIndex = 0;
            int directionIndex = 0;

            List<string> currentlyBuildingAnimationLines = new List<string>();
            foreach (var line in fileLines)
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

            return animations;
        }

        private static LightAnimation BuildFromDataLines(string[] lineData, float scale)
        {
            List<LightFrame> frameList = new List<LightFrame>();

            for (int i = 0; i < lineData.Length; ++i)
            {
                frameList.Add(BuildFromDataLine(lineData[i], scale));
            }

            return new LightAnimation(frameList);
        }

        public static string[] BuildFileLinesForNewMetaData(Rectangle textureDimensions)
        {
            List<string> lines = new List<string>();

            for (int i = 0; i < 10; ++i)
            {
                for (int j = 0; j < 4; ++j)
                {
                    lines.Add(";" + i + ";" + j);
                    lines.Add("50<" + textureDimensions.X + "," + textureDimensions.Y + "," + textureDimensions.Width + "," + textureDimensions.Height + ">");
                }
            }

            return lines.ToArray();
        }

        public static string[] BuildFileLinesFromAnimation(Dictionary<int, LightAnimation[]> animations)
        {
            List<string> lines = new List<string>();

            //our read system is far to complicated, lets simplify the write at least

            //iterate over each key
            //  iterate over each animation
            //      ;key;animation
            //      iterate over each frame
            //          serialize frame to our format
            //          time<rect>{phys}
            //          foreach dot
            //              (dot)

            foreach (KeyValuePair<int, LightAnimation[]> action in animations)
            {
                for (int directionIndex = 0; directionIndex < 4; ++directionIndex)// LightAnimation animation in action.Value)
                {
                    lines.Add(";" + action.Key + ";" + directionIndex);
                    foreach (LightFrame frame in action.Value[directionIndex].frames)
                    {
                        string line = frame.FrameTime + "<" + GetRectangleCSV(frame.SourceRectangle) + ">";
                        if (frame.PhysicsRectangle != null && frame.PhysicsRectangle != Rectangle.Empty)
                        {
                            line += "{" + GetRectangleCSV(frame.PhysicsRectangle) + "}";
                        }
                        foreach (Rectangle damageDot in frame.DamageDots)
                        {
                            line += "(" + GetRectangleCSV(damageDot) + ")";
                        }
                        lines.Add(line);
                    }
                }
            }

            return lines.ToArray();
        }

        public static string GetRectangleCSV(Rectangle rectangle)
        {
            return rectangle.X + "," + rectangle.Y + "," + rectangle.Width + "," + rectangle.Height;
        }

        private static LightFrame BuildFromDataLine(string lineData, float scale)
        {
            Rectangle sourceRectangle = readRectangles(lineData, '<', '>').First();

            Rectangle drawRectangle;

            drawRectangle = new Rectangle(0, 0, sourceRectangle.Width, sourceRectangle.Height);
            drawRectangle.Width = (int)(sourceRectangle.Width * scale);
            drawRectangle.Height = (int)(sourceRectangle.Height * scale);
            //drawRectangle.X = drawRectangle.Width / 2;

            Rectangle physicsRectangle = readRectangles(lineData, '{', '}').FirstOrDefault();
            physicsRectangle = new Rectangle(
                (int)(physicsRectangle.X * scale),
                (int)(physicsRectangle.Y * scale),
                (int)(physicsRectangle.Width * scale),
                (int)(physicsRectangle.Width * scale)
            );

            List<Rectangle> damageDots = readRectangles(lineData, '(', ')');
            for (int i = 0; i < damageDots.Count; i++)
            {
                damageDots[i] = new Rectangle(
                    (int)(damageDots[i].X * scale),
                    (int)(damageDots[i].Y * scale),
                    1,
                    1
                );
            }

            int time = int.Parse(lineData.Substring(0, lineData.IndexOf('<')));

            return new LightFrame()
            {
                FrameTime = time,
                SourceRectangle = sourceRectangle,
                DrawRectangle = drawRectangle,
                PhysicsRectangle = physicsRectangle,
                DamageDots = damageDots.ToArray()
            };
        }

        private static List<Rectangle> readRectangles(string lineData, char leftSep, char rightSep)
        {
            int rectCount = lineData.Count(c => c == leftSep);
            List<Rectangle> rectList = new List<Rectangle>();
            int currentIndex = 0;
            for (int j = 0; j < rectCount; ++j)
            {
                lineData = lineData.Replace(" ", "").Replace("\t", "");
                var data = lineData.Substring(lineData.IndexOf(leftSep, currentIndex) + 1, lineData.IndexOf(rightSep, currentIndex) - lineData.IndexOf(leftSep, currentIndex) - 1).Split(',');
                rectList.Add(new Rectangle(int.Parse(data[0]), int.Parse(data[1]), int.Parse(data[2]), int.Parse(data[3])));
                currentIndex = lineData.IndexOf(rightSep, currentIndex) + 1;
            }
            return rectList;
        }

    }
}
