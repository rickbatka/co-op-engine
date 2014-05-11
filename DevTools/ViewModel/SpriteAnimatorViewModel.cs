using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevTools.Model;
using DevTools.ViewModel.Shared;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DevTools.ViewModel
{
    class SpriteAnimatorViewModel : ViewModelBase
    {
        AnimationToolSystem model;
        ContentManager Content;

        public SpriteAnimatorViewModel()
        {}

        public void LoadContent(ContentManager contentmgr)
        {
            Content = contentmgr;
        }

        public int maxSliderValue = 100;
        public int TimescaleSliderValue
        {
            get { return (int)(model.Timescale * maxSliderValue); }
            set
            {
                model.Timescale = (float)value / (float)maxSliderValue;
                OnPropertyChanged("TimescaleSliderValue");
            }
        }

        public string FileName
        {
            get { return model.FileName; }
            set
            {
                model.SetFile(value, Content);
                OnPropertyChanged("FileName");
            }
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            model.Draw(spriteBatch);
        }
    }
}
