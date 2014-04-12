using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace co_op_engine.Utility
{
    public delegate void GameTimerCallback(object state);
    class GameTimerManager
    {
        private List<GameTimer> Timers = new List<GameTimer>();

        private static GameTimerManager instance;
        private GameTimerManager() {}

        public static GameTimerManager Instance
       {
          get 
          {
             if (instance == null)
             {
                instance = new GameTimerManager();
             }
             return instance;
          }
       }

        public GameTimer SetTimer(int time, GameTimerCallback updateCallback, GameTimerCallback endCallback)
        {
            var newTimer = new GameTimer(time, updateCallback, endCallback);
            Timers.Add(newTimer);
            return newTimer;
        }

        public void Update(GameTime gameTime)
        {
            var timersToUpdate = Timers.Where(t => !t.ShouldDelete).ToArray();
            int upd = timersToUpdate.Count();
            for (int i = 0; i < upd; i++)
            {
                timersToUpdate[i].Update(gameTime);
                timersToUpdate[i].DoUpdateCallback();

                if (timersToUpdate[i].Finished)
                {
                    timersToUpdate[i].DoEndCallback();
                    timersToUpdate[i].MarkForDeletion();
                }
            }

            HandleDeletions();
        }

        private void HandleDeletions()
        {
            var timersToRemove = Timers.Where(t => t.ShouldDelete).ToArray();
            int del = timersToRemove.Count();
            for (int i = 0; i < del; i++)
            {
                Timers.Remove(timersToRemove[i]);
            }
        }
    }

    class GameTimer
    {
        TimeSpan TimeLeft;
        GameTimerCallback UpdateCallback;
        GameTimerCallback EndCallback;

        public bool ShouldDelete = false;

        public bool Finished { get { return TimeLeft <= TimeSpan.Zero; } }

        public GameTimer(int timeSeed,GameTimerCallback updateCallback, GameTimerCallback endCallback)
        {
            TimeLeft = TimeSpan.FromMilliseconds(timeSeed);
            UpdateCallback = updateCallback;
            EndCallback = endCallback;
        }

        public void Update(GameTime gameTime)
        {
            TimeLeft -= gameTime.ElapsedGameTime;
        }

        public void DoUpdateCallback()
        {
            UpdateCallback(null);
        }

        public void DoEndCallback()
        {
            EndCallback(null);
        }

        public void MarkForDeletion()
        {
            ShouldDelete = true;
        }
    }
}
