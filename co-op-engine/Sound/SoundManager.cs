using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;

namespace co_op_engine.Sound
{
    class SoundManager
    {
        //should be ok to have a lazy sound manager
        private static SoundManager _mgr;
        private static SoundManager instance
        {
            get { return _mgr ?? (_mgr = new SoundManager()); }
        }

        //keeping this lightweight and highly visible
        private List<SoundEffectInstance> runningSoundEffects;
        private SoundEffectInstance activeMusic;
        private SoundEffectInstance deactivatingMusic;
        private int crossfadeTimerMilli;
        private TimeSpan crossfadeTimer;

        private TimeSpan updateTimer;
        private int updateTimerMilli;

        private SoundManager()
        {
            updateTimerMilli = 100;
            runningSoundEffects = new List<SoundEffectInstance>();
        }

        public static void Update(GameTime gameTime)
        {
            //TODO this is debug stuff, remove it eventually
            if (InputHandler.KeyPressed(Microsoft.Xna.Framework.Input.Keys.M))
            {
                //SoundManager.CrossfadeMusic(5000, AssetRepository.Instance.Music2);
            }

            //instance predicate got that annoying
            instance.UpdateInternal(gameTime);
        }
        private void UpdateInternal(GameTime gameTime)
        {
            //remove done effects
            for (int i = 0; i < runningSoundEffects.Count; ++i)
            {
                if (runningSoundEffects[i].State == SoundState.Stopped)
                {
                    runningSoundEffects[i].Dispose();
                    runningSoundEffects.Remove(runningSoundEffects[i]);
                }
            }

            //update crossfade
            if (crossfadeTimer > TimeSpan.Zero)
            {
                float elapsedRatio = (float)(crossfadeTimer.TotalMilliseconds / (float)crossfadeTimerMilli);
                if (deactivatingMusic != null && !deactivatingMusic.IsDisposed)
                {
                    deactivatingMusic.Volume = elapsedRatio;
                }
                activeMusic.Volume = 1 - elapsedRatio;

                crossfadeTimer -= gameTime.ElapsedGameTime;
                if (crossfadeTimer <= TimeSpan.Zero)
                {
                    if (deactivatingMusic != null && !deactivatingMusic.IsDisposed)
                    {
                        deactivatingMusic.Stop();
                        deactivatingMusic.Dispose();
                    }
                    activeMusic.Volume = 1f;
                }
            }
        }

        //done
        public static void CrossfadeMusic(int fadeMilli, SoundEffect music)
        {
            instance.CrossfadeMusicInst(fadeMilli, music);
        }
        private void CrossfadeMusicInst(int fadeMilli, SoundEffect music)
        {
            SoundEffectInstance song = music.CreateInstance();
            
            if (activeMusic != null && !activeMusic.IsDisposed)
            {
                if (deactivatingMusic!= null && !deactivatingMusic.IsDisposed)
                {
                    deactivatingMusic.Dispose();
                }
                deactivatingMusic = activeMusic;
            }

            song.Play();
            activeMusic = song;
            activeMusic.Volume = 0f;
            crossfadeTimerMilli = fadeMilli;
            crossfadeTimer = TimeSpan.FromMilliseconds(crossfadeTimerMilli);
        }

        public static void PauseMusic()
        {
            instance.activeMusic.Pause();
        }

        public static void PlayMusic()
        {
            instance.activeMusic.Play();
        }

        public static void PlaySoundEffect(SoundEffect effect, float volume = 1)
        {
            instance.PlaySoundEffectInst(effect, volume);
        }
        private void PlaySoundEffectInst(SoundEffect effect, float volume)
        {
            SoundEffectInstance effectInstance = effect.CreateInstance();
            effectInstance.Play();

            runningSoundEffects.Add(effectInstance);
        }

        public static void MuteAll()
        {
            instance.activeMusic.Volume = 0f;

            foreach (var effect in instance.runningSoundEffects)
            {
                effect.Volume = 0;
            }
        }

        //TODO this just sets them to volume 1, we need something to record previous volume in muteall and restore it, needs data object
        public static void UnMuteAll()
        {
            instance.activeMusic.Volume = 1f;

            foreach (var effect in instance.runningSoundEffects)
            {
                effect.Volume = 1f;
            }
        }
    }
}
