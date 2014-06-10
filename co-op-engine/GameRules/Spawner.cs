using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.GameRules
{
    class Spawner
    {
        //used by
        //director, directly(spawn a boss now) or setup and go(do this equation until I say stop)
        //listens for stop spawning command

        //uses
        //factories to construct objects and set them out

        //internal:
        //contians an equation to direct spawning (thinking area under curve as inverse max time, most likely sinus in nature combined with other spawners to have harmonic spawns
        //containes a timer of when to stop spawning for wave completion? or listens for stop command
        //this would be a location to construct squads and setup their intercommunication
    }
}
