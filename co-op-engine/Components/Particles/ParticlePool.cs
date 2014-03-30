using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Particles
{
    /*
     * Basically, an array of Particles with all the active ones
     * at the front and dead ones at the end. Will recycle dead ones
     * when asked, or create new particles up until Max slots of living 
     * particles are taken. At that point, will just reclaim random ones
     * 
     * The weakness of this is that we never know where a specific particle is in
     * the array, and we never know where the oldest / newest particles might be,
     * we only know that all the alive particles are at the beginning.
     * */
    public class ParticlePool
    {
        //@TODO get rid
        const int PARTICLE_LIFETIME_MS = 750;

        const int Max = 1000;
        Particle[] Pool; 
        int NumAliveParticles;
        int NumDeadParticles;

        private Vector2 debugTextPosition = new Vector2(500, 100);

        public ParticlePool()
        {
            Pool = new Particle[Max];
            NumAliveParticles = 0;
            NumDeadParticles = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < NumAliveParticles; i++)
            { 
                spriteBatch.Draw(
                    texture: AssetRepository.Instance.PlainWhiteTexture,
                    rectangle: Pool[i].DrawRectangle,
                    color: (Pool[i].IsAlive) ? Color.Green : Color.Red
                );
            }

            //DEBUG drawing
            spriteBatch.DrawString(
                spriteFont: AssetRepository.Instance.Arial,
                text: "Particles: alive: " + NumAliveParticles + " dead: " + NumDeadParticles,
                position: debugTextPosition,
                color: Color.White
            );
        }

        public void Spawn()
        {
            int index = GetAvailableParticleIndex();
        }

        public int GetAvailableParticleIndex()
        {
            int availableParticleIndex = -1;
            if(NumAliveParticles >= Max)
            {
                // all slots used - need to reclaim one (first in list?)
                availableParticleIndex = 0;
            }
            else if (NumDeadParticles == 0)
            { 
                //open slots available, but not initialized yet - make one and claim the slot
                availableParticleIndex = NumAliveParticles;
                Pool[availableParticleIndex] = new Particle();
                NumAliveParticles++;
            }
            else 
            { 
                //slots containing dead particles exist, give them one of those.
                availableParticleIndex = NumAliveParticles;
                NumAliveParticles++;
                NumDeadParticles--;
            }

            Pool[availableParticleIndex].Initialize(PARTICLE_LIFETIME_MS);

            return availableParticleIndex;
        }

        public void Update(GameTime gameTime)
        {
            int i = 0;
            while (i < NumAliveParticles)
            {
                Pool[i].Update(gameTime);
                if (!Pool[i].IsAlive)
                {
                    Swap(i, (NumAliveParticles - 1));
                    NumAliveParticles--;
                    NumDeadParticles++;
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
