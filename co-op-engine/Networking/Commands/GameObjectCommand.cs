using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace co_op_engine.Networking.Commands
{
    public enum GameObjectCommandType
    {
        Create,
        Update,
        Delete
    };//assuming we wont need a query

    [Serializable]
    public struct GameObjectCommand
    {
        //I could do a switch off of the typeof object passed, but I like
        //this better it's less complex but more fragmented.... ahh tradeoffs
        public GameObjectCommandType CommandType; 
        public object Parameters;
    }

    [Serializable]
    public struct UpdateParameters
    {
        public int ID;
        public ImplicitHack Velocity;
        public ImplicitHack Position;
        public float RotationTowardFacingDirectionRadians;
        public int CurrentActorState;
    };

    [Serializable]
    public struct ImplicitHack
    {
        float x, y;
        public static implicit operator ImplicitHack(Vector2 vect)
        {
            return new ImplicitHack() { x = vect.X, y = vect.Y };
        }
        public static implicit operator Vector2(ImplicitHack imp)
        {
            return new Vector2(imp.x, imp.y);
        }
    }

    [Serializable]
    public struct CreateParameters
    {
        public int ID;
        public string ObjectTypeEnumerationPossiblyChangeThisLaterLetsTalkAboutIt; //could be a tag on the factories for what object to create

        public Type Brain;
        public Type Renderer;
        public Type Physics;
        public Type Weapon;
        public Type Combat;
    }

    [Serializable]
    public struct DeleteParameters
    {
        public int ID;
    }
}
