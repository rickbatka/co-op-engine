using co_op_engine.Utility;
using co_op_engine.Utility.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Particles
{
    public class ParticleEngine
    {
        const int Max = 5000;
        IParticle[] Pool; 
        int NumAliveParticles;
        List<Emitter> Emitters = new List<Emitter>();

        private static ParticleEngine _instance;
        public static ParticleEngine Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ParticleEngine();
                }
                return _instance;
            }
        }

        private ParticleEngine()
        {
            Pool = new IParticle[Max];
            NumAliveParticles = 0;
        }

        public void AddEmitter(Emitter emitter)
        {
            Emitters.Add(emitter);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < NumAliveParticles; i++)
            {
                Pool[i].Draw(spriteBatch);
            }

            //DEBUG drawing
            spriteBatch.DrawString(
                spriteFont: AssetRepository.Instance.Arial,
                text: "Particles: alive: " + NumAliveParticles,
                position: new Vector2(Camera.Instance.ViewBoundsRectangle.Right - 250, Camera.Instance.ViewBoundsRectangle.Top + 25),
                color: Color.White,
                rotation: 0f,
                origin: Vector2.Zero,
                scale: 1f,
                effects: SpriteEffects.None,
                depth: 1f
            );
        }

        public void Add(IParticle particle)
        {
            int index = GetAvailableParticleIndex();
            Pool[index] = particle;
            Pool[index].Begin();
        }

        public int GetAvailableParticleIndex()
        {
            int availableParticleIndex = -1;
            if(NumAliveParticles >= Max)
            {
                // all slots used - need to reclaim one (first in list?)
                availableParticleIndex = 0;
            }
            else
            { 
                //open slots available - make one and claim the slot
                availableParticleIndex = NumAliveParticles;
                NumAliveParticles++;
            }

            return availableParticleIndex;
        }

        public void Update(GameTime gameTime)
        {
            // update emitters
            foreach (var emitter in Emitters)
            {
                 emitter.Update(gameTime);
            }
            var emittersToRemove = Emitters.Where(e => !e.IsAlive).ToArray();
            int del = emittersToRemove.Count();
            for (int j = 0; j < del; j++)
            {
                Emitters.Remove(emittersToRemove[j]);
            }

            // update particles
            int i = 0;
            while (i < NumAliveParticles)
            {
                Pool[i].Update(gameTime);
                if (!Pool[i].IsAlive)
                {
                    Pool[i].End();
                    Swap(i, (NumAliveParticles - 1));
                    NumAliveParticles--;
                    Pool[NumAliveParticles] = null;
                }

                i++;
            }
        }

        private void Swap(int left, int right)
        {
            IParticle swap = Pool[left];
            Pool[left] = Pool[right];
            Pool[right] = swap;
        }

    }
}
