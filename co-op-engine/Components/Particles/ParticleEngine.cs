using co_op_engine.Utility;
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
        Particle[] Pool; 
        int NumAliveParticles;
        List<Emitter> Emitters = new List<Emitter>();

        private Vector2 debugTextPosition = new Vector2(500, 100);

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
            Pool = new Particle[Max];
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
                spriteBatch.Draw(
                    texture: AssetRepository.Instance.PlainWhiteTexture,
                    rectangle: Pool[i].DrawRectangle,
                    color: Pool[i].DrawColor
                );
            }

            //DEBUG drawing
            spriteBatch.DrawString(
                spriteFont: AssetRepository.Instance.Arial,
                text: "Particles: alive: " + NumAliveParticles,
                position: debugTextPosition,
                color: Color.White
            );
        }

        public void Add(Particle particle)
        {
            int index = GetAvailableParticleIndex();
            Pool[index] = particle;
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
                    Swap(i, (NumAliveParticles - 1));
                    NumAliveParticles--;
                    Pool[NumAliveParticles] = null;
                }

                i++;
            }
        }

        private void Swap(int left, int right)
        {
            Particle swap = Pool[left];
            Pool[left] = Pool[right];
            Pool[right] = swap;
        }

    }
}
