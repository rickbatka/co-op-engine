using co_op_engine.World.Level;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Factories
{
    public class LevelFactory
    {
        private static LevelFactory _instance;
        public static LevelFactory Instance
        {
            get
            {
                if (_instance == null) { _instance = new LevelFactory(); }
                return _instance;
            }
        }

        private LevelFactory() { }

        public Level GetLevel1()
        {
            var level1 = new Level();
            level1.SetGameDirector(new GameDirectorBase());
            return level1;
        }

        public Level GetLevel2()
        {
            var level2 = new Level();

            return level2;
        }
    }
}
