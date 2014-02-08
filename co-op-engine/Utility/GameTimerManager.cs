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

        public GameTimer SetTimer(int frames, GameTimerCallback callback, Components.GameObject owner)
        {
            var newTimer = new GameTimer(frames, callback, owner);
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
                if (timersToUpdate[i].Finished)
                {
                    timersToUpdate[i].InitiateCallback();
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
        GameTimerCallback Callback;

        public bool ShouldDelete = false;

        public bool Finished { get { return TimeLeft <= TimeSpan.Zero; } }

        public GameTimer(int timeSeed, GameTimerCallback callback, Components.GameObject owner)
        {
            TimeLeft = TimeSpan.FromMilliseconds(timeSeed);
            owner.OnDeath += HandleOwnerDeath;
            Callback = callback;
        }

        public void Update(GameTime gameTime)
        {
            TimeLeft -= gameTime.ElapsedGameTime;
        }

        public void InitiateCallback()
        {
            Callback(null);
        }

        private void HandleOwnerDeath(object sender, EventArgs e)
        {
            MarkForDeletion();
        }

        public void MarkForDeletion()
        {
            ShouldDelete = true;
        }
    }
}
