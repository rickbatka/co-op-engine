using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace co_op_engine.Components.Movement
{
    class MoverBase : IMovable
    {
        protected float friction = 0.5f;
        protected float speedLimit = 2000f;
        protected float accelerationModifier = 400f;
        protected float boostingModifier = 1.5f;

        protected GameObject owner;

        protected Vector2 velocity;
        public Vector2 Velocity { get { return velocity; } }

        protected Vector2 acceleration;
        public Vector2 Acceleration { get { return acceleration; } }
        
        protected Vector2 inputMovementVector;
        public Vector2 InputMovementVector { set { inputMovementVector = value; } }

        protected Vector2 position;
        public Vector2 Position { get { return position; } }

        protected int width;
        public int Width { get { return width; } }

        protected int height;
        public int Height { get { return height; } }

        protected bool isBoosting;
        public bool IsBoosting { set { isBoosting = value; } }

        public MoverBase(GameObject owner)
        {
            this.owner = owner;

            /////////////////////////////////////////
            //@TODO set these up in factory probably
            this.position = new Vector2(MechanicSingleton.Instance.rand.Next(1,500));
            this.width = 50;
            this.height = 50;
            //@END temp setup code
            /////////////////////////////////////////

        }

        public void Update(GameTime gameTime)
        {
            acceleration = (inputMovementVector * accelerationModifier * BoostLevel);

            velocity *= friction;

            if ((velocity + acceleration).Length() < speedLimit)
            {
                velocity += acceleration;
            }

            acceleration = Vector2.Zero;

            position += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch) { }

        private float BoostLevel
        {
            get
            {
                if (!isBoosting) return 1;
                return boostingModifier;
            }
        }
    }
}
