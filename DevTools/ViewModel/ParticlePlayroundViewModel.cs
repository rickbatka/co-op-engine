extern alias monoFrameworkAlias;
extern alias xnaFrameworkAlias;
extern alias xnaGameAlias;
extern alias xnaGraphicsAlias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevTools.Model;
using DevTools.ViewModel.Shared;
using System.Collections.ObjectModel;
using System.IO;
using DevTools.GraphicsControls;
using co_op_engine.Components.Particles;
using xnaFrameworkAlias.Microsoft.Xna.Framework;
using xnaGraphicsAlias.Microsoft.Xna.Framework.Graphics;
using DevTools.GraphicsControls.Boiler;
using co_op_engine.Utility;
using co_op_engine.Utility.Camera;


namespace DevTools.ViewModel
{
    internal class ParticlePlaygroundViewModel : ViewModelBase
    {
        
        private TimeSpan totalGameTime;
        private DateTime lastHit;
        ParticleEngine Engine;
        public monoFrameworkAlias.Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch;
        public monoFrameworkAlias.Microsoft.Xna.Framework.Graphics.Texture2D PlainTexture;
        public monoFrameworkAlias.Microsoft.Xna.Framework.Graphics.GraphicsDevice monoDevice;

        public ParticlePlaygroundViewModel()
        {
            totalGameTime = new TimeSpan(0);
            lastHit = DateTime.Now;

            
            
        }

        // textures and stuff will be loaded when it gets here
        public void ContentLoaded()
        {
            Camera.Instantiate(new monoFrameworkAlias.Microsoft.Xna.Framework.Rectangle(0,0, 1000, 1000));
            Engine = ParticleEngine.Instance;
            Engine.AddEmitter(new EmptyEmitter(PlainTexture));
        }

        public void UpdateAndDraw()
        {
            TimeSpan elapsedTime = HitAndGetInterval();
            totalGameTime += elapsedTime;
            var fakeGameTime = new monoFrameworkAlias.Microsoft.Xna.Framework.GameTime(totalGameTime, elapsedTime);
            Engine.Update(fakeGameTime);
            Draw();
        }

        

        private TimeSpan HitAndGetInterval()
        {
            DateTime swap = lastHit;
            lastHit = DateTime.Now;
            return lastHit - swap;
        }


        private void HandleModelChanged(object sender, EventArgs e)
        {
            UpdateParameters();
        }

        private void UpdateParameters()
        {
            //LogDebug("update");

            //OnPropertyChanged(() => this.Directions);
            //OnPropertyChanged(() => this.Actions);
            //OnPropertyChanged(() => this.maxSliderValue);
            //OnPropertyChanged(() => this.SliderText);
            //OnPropertyChanged(() => this.CurrentSliderValue);
            //OnPropertyChanged(() => this.TimescaleLabelText);
            //OnPropertyChanged(() => this.FileName);

            //OnPropertyChanged(() => this.CurrentFramePhysicsText);
            //OnPropertyChanged(() => this.CurrentFrameSourceText);
            //OnPropertyChanged(() => this.CurrentFrameTimeText);

            //OnPropertyChanged(() => this.DamageDotItems);
        }

        public void LoadContent( xnaFrameworkAlias.Microsoft.Xna.Framework.Content.ContentManager contentmgr, xnaGraphicsAlias.Microsoft.Xna.Framework.Graphics.GraphicsDevice device)
        {
            //model.LoadContent(contentmgr, device);
        }

        internal void Draw()
        {
            Camera.Instance.Position = monoFrameworkAlias.Microsoft.Xna.Framework.Vector2.Zero;
            monoDevice.Clear(monoFrameworkAlias.Microsoft.Xna.Framework.Color.Black);
            //TODO AAAH HAHH AHHAHA AHAHAHA HA!!!!!! So close. set up the textures above in loadcontent
            spriteBatch.Begin(
                sortMode: monoFrameworkAlias.Microsoft.Xna.Framework.Graphics.SpriteSortMode.Immediate,
                blendState: monoFrameworkAlias.Microsoft.Xna.Framework.Graphics.BlendState.NonPremultiplied,
                samplerState: monoFrameworkAlias.Microsoft.Xna.Framework.Graphics.SamplerState.PointWrap,
                depthStencilState: monoFrameworkAlias.Microsoft.Xna.Framework.Graphics.DepthStencilState.Default,
                rasterizerState: monoFrameworkAlias.Microsoft.Xna.Framework.Graphics.RasterizerState.CullNone);
            Engine.Draw(spriteBatch);
            //spriteBatch.Draw(PlainTexture, new monoFrameworkAlias.Microsoft.Xna.Framework.Rectangle(0, 0, 64, 64), monoFrameworkAlias.Microsoft.Xna.Framework.Color.Green);
            spriteBatch.End();
            //
        }

    }
}
