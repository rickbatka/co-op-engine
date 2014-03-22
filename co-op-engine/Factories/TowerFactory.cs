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
        private NetworkBase netRef;


        private TowerFactory(GamePlay gameRef, NetworkBase netref)
        {
            this.gameRef = gameRef;
            this.netRef = netref;
        }

        public static void Initialize(GamePlay gameRef, NetworkBase netref)
        {
            Instance = new TowerFactory(gameRef, netref);
        }

        public GameObject GetDoNothingTower(bool fromNetwork = false, int id = -1)
        {
            var tower = new GameObject();
            tower.ID = id == -1 ? MechanicSingleton.Instance.GetNextObjectCountValue() : id;
            tower.CurrentFrame = AssetRepository.Instance.TowerAnimations.CurrentAnimatedRectangle.CurrentFrame;

            tower.UnShovable = true;
            tower.SetPhysics(new CollidingPhysics(tower));
            tower.SetRenderer(new RenderBase(tower, AssetRepository.Instance.TowerTexture));
            tower.SetBrain(new BasicTowerBrain(tower,
                AssetRepository.Instance.PlainWhiteTexture, 
                new KeyMouseTowerPlacingInput(gameRef, tower.BoundingBox),
                fromNetwork ? State.Built : State.Placing));
            
            tower.SetCombat(new CombatBase(tower));

            gameRef.container.AddObject(tower);

            if (!fromNetwork)
            {
                var parms = tower.BuildCreateParams();
                netRef.Input.Add(new CommandObject()
                {
                    ClientId = netRef.ClientId,
                    Command = new GameObjectCommand()
                    {
                        CommandType = GameObjectCommandType.Create,
                        Parameters = parms,
                    },
                });
            }

            return tower;
        }
    }
}
