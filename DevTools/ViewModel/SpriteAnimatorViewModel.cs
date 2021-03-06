﻿extern alias xnaFrameworkAlias;
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
using xnaFrameworkAlias.Microsoft.Xna.Framework.Content;
using xnaGraphicsAlias.Microsoft.Xna.Framework.Graphics;
using xnaFrameworkAlias.Microsoft.Xna.Framework;

namespace DevTools.ViewModel
{
    internal class SpriteAnimatorViewModel : ViewModelBase
    {
        AnimationToolSystem model;

        #region PropertyBinds

        public class DamageDotListItem
        {
            public string HACKDot;
            public string DotLocation
            {
                get { return HACKDot;}
                set
                {
                    HACKDot = value;
                    Update(this, value);
                } 
            }
            public Rectangle original;
            public Action<DamageDotListItem, string> Update;
            public Action<DamageDotListItem> Remove;
            public VMCommand RemoveDotFromFrame
            {
                get
                {
                    return new VMCommand((o) =>
                        {
                            Remove(this);
                        });
                }
            }
        }
        private ObservableCollection<DamageDotListItem> _ddi = new ObservableCollection<DamageDotListItem>();
        public ObservableCollection<DamageDotListItem> DamageDotItems
        {
            get
            {
                var frame = model.GetCurrentFrame();
                if (frame == null)
                    return new ObservableCollection<DamageDotListItem>();

                var list = new ObservableCollection<DamageDotListItem>();

                foreach (var dot in frame.DamageDots)
                {
                    list.Add(new DamageDotListItem()
                    {
                        HACKDot = MetaFileAnimationManager.GetRectangleCSV(dot),
                        Remove = RemoveDotFromFrame,
                        original = dot,
                        Update = UpdateDot,
                    });
                }
                return list;
            }
            set
            {
                _ddi = value;
                OnPropertyChanged(() => this.DamageDotItems);
            }
        }

        public bool FileIsOutOfDate
        {
            get { return model.FileIsOutOfDate; }
            set { model.FileIsOutOfDate = value; }
        }

        public int maxSliderValue
        {
            get { return model.GetAnimationLength() - 1; }
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
            get { return model.CurrentDirectionIndex; }
            set
            {
                model.CurrentDirectionIndex = value;
            }
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
            get { return model.CurrentAnimationIndex; }
            set
            {
                model.CurrentAnimationIndex = value;
            }
        }
        public ObservableCollection<string> Actions
        {
            get
            {
                return new ObservableCollection<string>(model.animations.Keys.Select((i) => GetNameForAction(i)));
            }
        }

        private string GetNameForAction(int actionId)
        {
            switch (actionId)
            {
                case 0:
                    return "Idle";
                case 1:
                    return "Walking";
                case 2:
                    return "Dying";
                case 3:
                    return "Dead";
                case 4:
                    return "Placing";
                case 5:
                    return "Being Hurt";
                case 6:
                    return "Attacking";
                case 7:
                    return "Boosting";
                case 8:
                    return "Raging";
                case 9:
                    return "Casting";
                default:
                    return "Unknown " + actionId;
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
                LogDebug("Set Selection");
                _cs = value;
                OnPropertyChanged(() => this.CurrentSelection);
                OnPropertyChanged(() => this.CurrentSelectionText);
            }
        }

        public int GridSize
        {
            get { return model.GridSize; }
        }
        public string GridSizeText
        {
            get { return model.GridSize.ToString(); }
            set
            {
                model.GridSize = int.Parse(value);
                OnPropertyChanged(() => this.GridSizeText);
            }
        }

        public string CurrentSelectionText
        {
            get { return MetaFileAnimationManager.GetRectangleCSV(CurrentSelection); }
            set
            {
                string[] dimensions = value.Split(',');
                try
                {
                    Rectangle possibleNewSelection = new Rectangle(
                            int.Parse(dimensions[0]),
                            int.Parse(dimensions[1]),
                            int.Parse(dimensions[2]),
                            int.Parse(dimensions[3]));
                    CurrentSelection = possibleNewSelection;
                }
                catch { }
            }
        }

        public string CurrentFrameSourceText
        {
            get
            {
                try
                {
                    return MetaFileAnimationManager.GetRectangleCSV(model.GetCurrentFrame().SourceRectangle);
                }
                catch
                {
                    return "NO FILE";
                }
            }
            set
            {
                string[] dimensions = value.Split(',');
                try
                {
                    Rectangle possibleNewRectangle = new Rectangle(
                            int.Parse(dimensions[0]),
                            int.Parse(dimensions[1]),
                            int.Parse(dimensions[2]),
                            int.Parse(dimensions[3]));

                    //the same for now I think in the game as well
                    model.GetCurrentFrame().SourceRectangle = possibleNewRectangle;
                    model.GetCurrentFrame().DrawRectangle = new Rectangle(0, 0, possibleNewRectangle.Width, possibleNewRectangle.Height);
                }
                catch { }
            }
        }

        public string CurrentFramePhysicsText
        {
            get
            {
                try
                {
                    return MetaFileAnimationManager.GetRectangleCSV(model.GetCurrentFrame().PhysicsRectangle);
                }
                catch
                {
                    return "NO FILE";
                }
            }
            set
            {
                string[] dimensions = value.Split(',');
                try
                {
                    Rectangle possibleNewRectangle = new Rectangle(
                            int.Parse(dimensions[0]),
                            int.Parse(dimensions[1]),
                            int.Parse(dimensions[2]),
                            int.Parse(dimensions[3]));

                    model.GetCurrentFrame().PhysicsRectangle = possibleNewRectangle;
                }
                catch { }
            }
        }

        public string CurrentFrameTimeText
        {
            get
            {
                try
                {
                    return model.GetCurrentFrame().FrameTime.ToString();
                }
                catch
                {
                    return "NO FILE";
                }
            }
            set
            {
                try
                {
                    int time = int.Parse(value);

                    model.GetCurrentFrame().FrameTime = time;
                }
                catch { }
            }
        }

        #endregion PropertyBinds

        #region Command Binds

        private VMCommand _aftc;
        public VMCommand AddFrameToCurrent
        {
            get
            {
                return _aftc ?? (_aftc = new VMCommand((o) =>
                    {
                        model.CreateNewFrame();
                        UpdateParameters();
                    }));
            }
        }

        private VMCommand _rffc;
        public VMCommand RemoveFrameFromCurrent
        {
            get
            {
                return _rffc ?? (_rffc = new VMCommand((o) =>
                    {
                        model.RemoveFrame();
                        UpdateParameters();
                    }));
            }
        }

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

        //not used anymore, no need
        private VMCommand _rcc;
        public VMCommand RefreshCurrentContent
        {
            get
            {
                return _rcc ?? (_rcc = new VMCommand((o) =>
                    {
                        LogDebug("Refreshing Content");
                        model.RecompileReload();
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

        private VMCommand _onf;
        public VMCommand OpenNewFile
        {
            get
            {
                return _onf ?? (_onf = new VMCommand((o) =>
                    {
                        string file = BrowseForImage();
                        if (file != null)
                        {
                            LogDebug("Opening Image, Creating Metadata");

                            model.LoadTexture(file);
                            model.CreateNewMetaData(file);

                            ResetValues();
                            UpdateParameters();
                        }
                    }));
            }
        }

        private VMCommand _ofp;
        public VMCommand OpenFilePair
        {
            get
            {
                return _ofp ?? (_ofp = new VMCommand((o) =>
                    {
                        string filename = BrowseForImage();
                        if (filename != null)
                        {
                            LogDebug("Opening File");

                            model.LoadTexture(filename);
                            model.LoadMetaData(filename);

                            ResetValues();
                            UpdateParameters();
                        }
                    }));
            }
        }

        private VMCommand _adtf;
        public VMCommand AddDotToFrame
        {
            get
            {
                return _adtf ?? (_adtf = new VMCommand((o) =>
                    {
                        var dots = model.GetCurrentFrame().DamageDots.ToList<Rectangle>();
                        dots.Add(new Rectangle(0, 0, 0, 0));
                        model.GetCurrentFrame().DamageDots = dots.ToArray();

                        OnPropertyChanged(() => this.DamageDotItems);

                        LogDebug("AddingDot");
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
            LogDebug("update");

            OnPropertyChanged(() => this.Directions);
            OnPropertyChanged(() => this.Actions);
            OnPropertyChanged(() => this.maxSliderValue);
            OnPropertyChanged(() => this.SliderText);
            OnPropertyChanged(() => this.CurrentSliderValue);
            OnPropertyChanged(() => this.TimescaleLabelText);
            OnPropertyChanged(() => this.FileName);

            OnPropertyChanged(() => this.CurrentFramePhysicsText);
            OnPropertyChanged(() => this.CurrentFrameSourceText);
            OnPropertyChanged(() => this.CurrentFrameTimeText);

            OnPropertyChanged(() => this.DamageDotItems);
        }

        private void ResetValues()
        {
            SelectedDirection = 0;
            SelectedAction = 0;

            OnPropertyChanged(() => this.SelectedAction);
            OnPropertyChanged(() => this.SelectedDirection);
        }

        #region Actions

        public void LoadContent(ContentManager contentmgr, GraphicsDevice device)
        {
            model.LoadContent(contentmgr, device);
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

        private string BrowseForImage()
        {
            System.Windows.Forms.OpenFileDialog opener = new System.Windows.Forms.OpenFileDialog();

            opener.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            opener.Filter = "Image files (*.jpg, *.bmp, *.png) | *.jpg; *.bmp; *.png";//"Content files| *.jpg; *.bmp; *.png | All Files (*.*) | *.*";

            if (opener.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return opener.FileName;
            }
            else
            {
                return null;
            }
        }

        public void RemoveDotFromFrame(DamageDotListItem sender)
        {
            model.GetCurrentFrame().DamageDots = model.GetCurrentFrame().DamageDots.ToList().Where(r => MetaFileAnimationManager.GetRectangleCSV(r) != sender.DotLocation).ToArray();

            OnPropertyChanged(() => this.DamageDotItems);

            LogDebug("AddingDot");
        }

        public void UpdateDot(DamageDotListItem sender, string update)
        {
            for(int i=0; i< model.GetCurrentFrame().DamageDots.Count(); ++i)
            {
                if (model.GetCurrentFrame().DamageDots[i] == sender.original)
                {
                    string[] dimensions = update.Split(',');
                    try
                    {
                        Rectangle rect = new Rectangle(
                                int.Parse(dimensions[0]),
                                int.Parse(dimensions[1]),
                                int.Parse(dimensions[2]),
                                int.Parse(dimensions[3]));
                        model.GetCurrentFrame().DamageDots[i] = rect;
                        sender.original = rect;
                    }
                    catch { }

                    return;
                }
            }

            OnPropertyChanged(() => this.DamageDotItems);
        }

        #endregion Actions
    }
}
