using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevTools.Model;
using DevTools.ViewModel.Shared;

namespace DevTools.ViewModel
{
    class SpriteAnimator : ViewModelBase
    {
        AnimationToolSystem model;

        public SpriteAnimator()
        {
            model = new AnimationToolSystem();
        }

        public int maxSliderValue = 100;
        public int TimescaleSliderValue
        {
            get { return (int)(model.Timescale * maxSliderValue); }
            set
            {
                model.SetTimeScale(value / maxSliderValue);
                OnPropertyChanged("TimescaleSliderValue");
            }
        }

        public string FileName
        {
            get { return model.FileName; }
            set
            {
                model.SetFile(value);
                OnPropertyChanged("FileName");
            }
        }


    }
}
