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
using Microsoft.Xna.Framework;
using DevTools.GraphicsControls;

namespace DevTools.ViewModel
{
    class SpriteAnimatorViewModel : ViewModelBase
    {
        AnimationToolSystem model;
        ContentManager Content;
        GraphicsDevice device;
        bool hasLoadedContentBefore = false;

        #region PropertyBinds

        public int maxSliderValue
        {
            get { return model.GetAnimationLength(); }
        }
        public int CurrentSliderValue
        {
            get { return model.GetCurrentFrameIndex(); }
            set
            {
                if (CurrentSliderValue != value)
                {
                    model.SetFrameIndex(value);
                    UpdateParameters();
                }
            }
        }
        public string SliderText
        {
            get { return CurrentSliderValue + "/" + maxSliderValue; }
        }

        public readonly int MaxTimscaleSliderValue = 100;
        public int TimescaleSliderValue
        {
            get { return (int)(model.Timescale * MaxTimscaleSliderValue); }
            set
            {
                model.Timescale = (float)value / (float)MaxTimscaleSliderValue;
                OnPropertyChanged(() => this.TimescaleSliderValue);
                OnPropertyChanged(() => this.TimescaleLabelText);
            }
        }
        public string TimescaleLabelText
        {
            get { return TimescaleSliderValue + "%"; }
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
                return new ObservableCollection<string>() { "1", "2", "3", "4" };
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

        public void LogDebug(string message)
        {
            DebugEntry.Insert(0, message);
            OnPropertyChanged(() => this.DebugEntry);
        }
        public ObservableCollection<string> DebugEntry
        {
            get;
            set;
        }

        private Rectangle _cs = new Rectangle(0, 0, 1, 1);
        public Rectangle CurrentSelection
        {
            get { return _cs; }
            set
            {
                _cs = value;
                OnPropertyChanged(() => this.CurrentSelection);
            }
        }

        #endregion PropertyBinds

        public SpriteAnimatorViewModel(GraphicsControlBase drawElement)
        {
            DebugEntry = new ObservableCollection<string>();
            model = new AnimationToolSystem();
        }

        private void UpdateParameters()
        {
            OnPropertyChanged(() => this.Directions);
            OnPropertyChanged(() => this.Actions);
            OnPropertyChanged(() => this.maxSliderValue);
            OnPropertyChanged(() => this.SliderText);
            OnPropertyChanged(() => this.CurrentSliderValue);
            OnPropertyChanged(() => this.TimescaleLabelText);
        }

        #region Actions

        internal void OpenFilePair(string filename)
        {
            LogDebug("Opening File");

            FileInfo info = new FileInfo(filename);
            if (!hasLoadedContentBefore)
            {
                hasLoadedContentBefore = true;
                Content.RootDirectory = info.Directory.FullName;
            }

            model.LoadTexture(filename, Content, device);
            model.LoadMetaData(filename);

            UpdateParameters();
            OnPropertyChanged(() => this.FileName);
        }

        internal void OpenNewFile(string file)
        {
            LogDebug("Opening Image, Creating Metadata");

            FileInfo info = new FileInfo(file);
            if (!hasLoadedContentBefore)
            {
                hasLoadedContentBefore = true;
                Content.RootDirectory = info.Directory.FullName;
            }

            model.LoadTexture(file, Content, device);
            model.CreateNewMetaData(info);

            UpdateParameters();
            OnPropertyChanged(() => this.FileName);
        }

        internal void SaveMetaFile()
        {
            model.SaveMetaData();
        }

        internal void RefreshCurrentContent()
        {
            LogDebug("Refreshing Content");

            model.RecompileReload(Content, device);
        }

        public void LoadContent(ContentManager contentmgr, GraphicsDevice Device)
        {
            Content = contentmgr;
            device = Device;
        }

        internal void Draw(SpriteBatch spriteBatch)
        {
            model.Draw(spriteBatch);
        }

        internal void DrawPhysics(SpriteBatch spriteBatch)
        {
            model.DrawPhysics(spriteBatch);
        }

        internal void DrawSelection(SpriteBatch spriteBatch)
        {
            model.DrawSelection(spriteBatch, CurrentSelection);
        }

        internal void DrawDamageDots(SpriteBatch spriteBatch)
        {
            model.DrawDamageDots(spriteBatch);
        }

        internal void Pause()
        {
            model.Timescale = 0;
            UpdateParameters();
        }

        internal void Play()
        {
            model.Timescale = 1;
            UpdateParameters();
        }

        #endregion Actions
    }
}
