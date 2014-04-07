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

    public enum GameObjectComponentType
    {
        Brain,
        Physics,
        Renderer,
        Combat
    }

    [Serializable]
    public struct GameObjectCommand
    {
        public int ID;
        public GameObjectComponentType ReceivingComponent;
        public GameObjectCommandType CommandType;
        public object Parameters;
    }

    [Serializable]
    public struct S_Vector2
    {
        float x, y;
        public static implicit operator S_Vector2(Vector2 vect)
        {
            return new S_Vector2() { x = vect.X, y = vect.Y };
        }
        public static implicit operator Vector2(S_Vector2 imp)
        {
            return new Vector2(imp.x, imp.y);
        }
    }
    
    [Serializable]
    public struct CreateParameters
    {
        public int ID;
        public string ConstructorId; //could be a tag on the factories for what object to create
        public S_Vector2 Position;
    }
}
