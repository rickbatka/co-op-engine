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
    internal class SpriteAnimatorViewModel : ViewModelBase
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

        private bool ShowFrameEditBool;
        public System.Windows.Visibility ShowFrameEdit
        {
            get
            {
                if (ShowFrameEditBool)
                {
                    return System.Windows.Visibility.Visible;
                }
                else
                {
                    return System.Windows.Visibility.Collapsed;
                }
            }
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

        #region Command Binds

        private VMCommand _tfep;
        public VMCommand ToggleFrameEditPanel
        {
            get
            {
                return _tfep ?? (_tfep = new VMCommand((o) =>
                    {
                        ShowFrameEditBool = !ShowFrameEditBool;
                        OnPropertyChanged(() => this.ShowFrameEdit);
                    }));
            }
        }

        private VMCommand _smd;
        public VMCommand SaveMetaData
        {
            get
            {
                return _smd ?? (_smd = new VMCommand((o) =>
                    {
                        model.SaveMetaData();
                    }));
            }
        }

        private VMCommand _rcc;
        public VMCommand RefreshCurrentContent
        {
            get
            {
                return _rcc ?? (_rcc = new VMCommand((o) =>
                    {
                        LogDebug("Refreshing Content");
                        model.RecompileReload(Content, device);
                    }));
            }
        }

        private VMCommand _pause;
        public VMCommand Pause
        {
            get
            {
                return _pause ?? (_pause = new VMCommand((o) =>
                    {
                        model.Timescale = 0;
                        UpdateParameters();
                    }));
            }
        }

        private VMCommand _play;
        public VMCommand Play
        {
            get
            {
                return _play ?? (_play = new VMCommand((o) =>
                    {
                        model.Timescale = 1;
                        UpdateParameters();
                    }));
            }
        }

        #endregion Command Binds

        public SpriteAnimatorViewModel()
        {
            DebugEntry = new ObservableCollection<string>();
            model = new AnimationToolSystem();

            model.OnModelChanged += HandleModelChanged;
        }

        private void HandleModelChanged(object sender, EventArgs e)
        {
            UpdateParameters();
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

        public void OpenNewFile(string file)
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

        public void OpenFilePair(string filename)
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



        #endregion Actions
    }
}
