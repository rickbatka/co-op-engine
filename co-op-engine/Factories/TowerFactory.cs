using co_op_engine.Components;
using co_op_engine.Components.Brains.TowerBrains;
using co_op_engine.Components.Combat;
using co_op_engine.Components.Input;
using co_op_engine.Components.Physics;
using co_op_engine.Components.Rendering;
using co_op_engine.GameStates;
using co_op_engine.Networking;
using co_op_engine.Networking.Commands;
using co_op_engine.Utility;
using Microsoft.Xna.Framework;

namespace co_op_engine.Factories
{
    class TowerFactory
    {
        public static TowerFactory Instance;
        private GamePlay gameRef;

        private TowerFactory(GamePlay gameRef)
        {
            this.gameRef = gameRef;
        }

        public static void Initialize(GamePlay gameRef)
        {
            Instance = new TowerFactory(gameRef);
        }

        public GameObject GetDoNothingTower(bool fromNetwork = false, int id = -1)
        {
            var tower = new GameObject();
            tower.ConstructionStamp = "Tower";
            tower.ID = id == -1 ? MechanicSingleton.Instance.GetNextObjectCountValue() : id;
            tower.CurrentFrame = AssetRepository.Instance.TowerAnimations.CurrentAnimatedRectangle.CurrentFrame;

            tower.UnShovable = true;
            tower.SetPhysics(new CollidingPhysics(tower));
            tower.SetRenderer(new RenderBase(tower, AssetRepository.Instance.TowerTexture, AssetRepository.Instance.TowerAnimations));
            tower.SetBrain(new BasicTowerBrain(tower,
                AssetRepository.Instance.PlainWhiteTexture,
                new TowerPlacingInput(gameRef, tower.BoundingBox)));

            if (fromNetwork)
            {
                tower.CurrentState = Constants.ACTOR_STATE_IDLE;
            }
            else
            {
                tower.CurrentState = Constants.ACTOR_STATE_PLACING;
            }

            tower.SetCombat(new CombatBase(tower));

            gameRef.container.AddObject(tower);

            if (!fromNetwork)
            {
                NetCommander.CreatedObject(tower);
            }

            return tower;
        }
    }
}
