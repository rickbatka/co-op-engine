using co_op_engine.Components;
using co_op_engine.Components.Brains.TowerBrains;
using co_op_engine.Components.Combat;
using co_op_engine.Components.Input;
using co_op_engine.Components.Physics;
using co_op_engine.Components.Rendering;
using co_op_engine.Components.Skills;
using co_op_engine.Components.Skills.Weapons;
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

        //in the tower factory???
        public GameObject GetInvisibleWall(bool fromNetwork = false)
        {
            var tower = new GameObject();
            tower.ConstructionStamp = "InvisibleWallTall";
            tower.Team = -1;
            tower.ID = MechanicSingleton.Instance.GetNextObjectCountValue();
            tower.CurrentFrame = AssetRepository.Instance.TowerAnimations(tower.Scale).CurrentAnimatedRectangle.CurrentFrame;

            tower.UsedInPathing = true;
            tower.SetPhysics(new CollidingPhysics(tower, gameRef.Level.Bounds));
            tower.SetRenderer(new RenderBase(tower, AssetRepository.Instance.TowerTexture, AssetRepository.Instance.TowerAnimations(tower.Scale)));
            tower.CurrentState = Constants.ACTOR_STATE_IDLE;

            gameRef.container.AddObject(tower);

            if (!fromNetwork)
            {
                NetCommander.CreatedObject(tower);
            }

            return tower;
        }

        public GameObject GetFriendlyAOEHealingTower(bool fromNetwork = false, int id = -1)
        {
            var tower = new GameObject();
            tower.ConstructionStamp = "FriendlyAOEHealingTower";
            tower.Team = 0;
            tower.ID = id == -1 ? MechanicSingleton.Instance.GetNextObjectCountValue() : id;
            tower.CurrentFrame = AssetRepository.Instance.TowerAnimations(tower.Scale).CurrentAnimatedRectangle.CurrentFrame;

            tower.UsedInPathing = true;
            tower.SetPhysics(new CollidingPhysics(tower, gameRef.Level.Bounds));
            tower.SetRenderer(new RenderBase(tower, AssetRepository.Instance.TowerTexture, AssetRepository.Instance.TowerAnimations(tower.Scale)));
            tower.SetBrain(new HealingAOETowerBrain(tower,
                new TowerPlacingInput(gameRef, tower.PhysicsCollisionBox)));

            tower.SetSkills(new SkillsComponent(tower));

            //DOTHIS this weapon needs to be tower specific, doing the healing on it's own
            var healingAOEWeapon = new WeaponBase(tower.Skills, tower);
            
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

        public GameObject GetArrowTower(bool fromNetwork = false, int id = -1)
        {
            var tower = new GameObject();
            tower.ConstructionStamp = "ArrowTower";
            tower.Team = 0;
            tower.ID = id == -1 ? MechanicSingleton.Instance.GetNextObjectCountValue() : id;
            tower.CurrentFrame = AssetRepository.Instance.TowerAnimations(tower.Scale).CurrentAnimatedRectangle.CurrentFrame;

            tower.UsedInPathing = true;
            tower.SetPhysics(new CollidingPhysics(tower, gameRef.Level.Bounds));
            tower.SetRenderer(new RenderBase(tower, AssetRepository.Instance.TowerTexture, AssetRepository.Instance.TowerAnimations(tower.Scale)));
            tower.SetBrain(new ArrowTowerBrain(tower,
                new TowerPlacingInput(gameRef, tower.PhysicsCollisionBox)));
            tower.SetSkills(new SkillsComponent(tower));

            var emptyWeapon = new WeaponBase(tower.Skills, tower);
            tower.EquipWeapon(emptyWeapon);

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
