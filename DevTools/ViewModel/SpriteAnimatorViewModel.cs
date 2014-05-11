using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevTools.Model;
using DevTools.ViewModel.Shared;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;
using System.IO;

namespace DevTools.ViewModel
{
    class SpriteAnimatorViewModel : ViewModelBase
    {
        AnimationToolSystem model;
        ContentManager Content;

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
        }

        public int SelectedDirection
        {
            get { return model.CurrentDirection; }
            set { model.CurrentDirection = value; }
        }
        public ObservableCollection<string> Directions
        {
            get
            {
                return new ObservableCollection<string>(){"1","2","3","4"};
            }
        }

        public int SelectedAction
        {
            get { return model.CurrentAnimation; }
            set { model.CurrentAnimation = value; }
        }
        public ObservableCollection<string> Actions
        {
            get
            {
                return new ObservableCollection<string>(model.animations.Keys.Select((i) => i.ToString()));
            }
        }

        private void UpdateParameters()
        {
            OnPropertyChanged(() => this.Directions);
            OnPropertyChanged(() => this.Actions);
        }

        public SpriteAnimatorViewModel()
        {
            model = new AnimationToolSystem();
        }

        public void LoadContent(ContentManager contentmgr)
        {
            Content = contentmgr;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            model.Draw(spriteBatch);
        }

        bool hasLoadedContentBefore = false;
        internal void OpenFilePair(string filename)
        {
            FileInfo info = new FileInfo(filename);
            if (!hasLoadedContentBefore)
            {
                hasLoadedContentBefore = true;
                Content.RootDirectory = info.Directory.FullName;
            }
            model.LoadTexture( filename , Content);
            model.FileName = filename;
            UpdateParameters();
            OnPropertyChanged(() => this.FileName);
        }

        internal void RefreshCurrentContent()
        {
            OpenFilePair(FileName);
        }
    }
}
