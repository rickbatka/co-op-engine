using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Utility
{
    public delegate void GameTimerCallback(object state);
    public class GameTimerManager
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

        public GameTimer SetTimer(int frames, GameTimerCallback callback)
        {
            var newTimer = new GameTimer(frames, callback);
            Timers.Add(newTimer);
            return newTimer;
        }

        public void Update()
        {
            var timersToUpdate = Timers.Where(t => !t.ShouldDelete).ToArray();
            int upd = timersToUpdate.Count();
            for (int i = 0; i < upd; i++)
            {
                timersToUpdate[i].Tick();
                if (timersToUpdate[i].Finished)
                {
                    timersToUpdate[i].initiateCallback();
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

    public class GameTimer
    {
        int Frames;
        GameTimerCallback Callback;

        public bool ShouldDelete = false;

        public bool Finished { get { return Frames <= 0; } }

        public GameTimer(int frames, GameTimerCallback callback)
        {
            Frames = frames;
            Callback = callback;
        }

        public void Tick()
        {
            Frames--;
        }

        public void initiateCallback()
        {
            Callback(null);
        }

        public void MarkForDeletion()
        {
            ShouldDelete = true;
        }
    }
}
