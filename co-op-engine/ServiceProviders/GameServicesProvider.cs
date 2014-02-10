using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.ServiceProviders
{
    class GameServicesProvider
    {
        private static Game1 game;

        public static void Install(Game1 theGame)
        {
            game = theGame;
        }

        public static void AddService(Type type, Object provider)
        {
            game.Services.AddService(typeof(IActorInformationProvider), provider);
        }

        public static object GetService(Type type)
        { 
            return game.Services.GetService(type);
        }

    }
}
