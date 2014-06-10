using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.GameRules
{
    class Director
    {
        //things the director needs to be aware of:
        //hits
        //kills
        //damage
        //spawns
        //errors
        //network

        //actions:
        //spawning, directly or setting up systems and letting them go
        //resource monitoring (towers)
        //pausing
        //connection management(networking)
        //saving/loading maybe
        //evaluating win/lose and triggering correct response

        //things that talk to this:
        //effects doing damage/killing
        //players constructing towers
        //spawners notify of spawn
        //error recovery from EVERYWHERE? maybe this should be in a logging class
        //network conenction, add / remove, handle adjustment

    }
}
