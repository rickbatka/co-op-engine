using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevTools.GraphicsControls.Boiler;
using DevTools.ViewModel.Shared;
using ContentCompiler.ContentCompilation;

namespace DevTools.ViewModel
{
    public class BuilderViewModel : ViewModelBase
    {
        private ContentBuilder contentBuilder;

        

        private string _id;
        public string InputDirectory
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(InputDirectory);
            }
        }

        private string _od;
        public string OutputDirectory
        {
            get { return _od; }
            set
            {
                _od = value;
                OnPropertyChanged("OutputDirectory");
            }
        }

        private string _ot;
        public string OutputText
        {
            get { return _ot; }
            set
            {
                _ot = value;
                OnPropertyChanged("OutputText");
            }
        }

        public BuilderViewModel()
        {
            contentBuilder = new ContentBuilder();
            LoadConfiguredValues();
        }

        private void LoadConfiguredValues()
        {
            OutputDirectory = ConfigurationManager.AppSettings["DefaultOutputDirectory"];
            InputDirectory = ConfigurationManager.AppSettings["DefaultInputDirectory"];
        }

        public void BuildAssets()
        {
            contentBuilder.OnOutput += HandleOutput;

            contentBuilder.BuildAssets(InputDirectory, OutputDirectory, false);

            contentBuilder.OnOutput -= HandleOutput;
        }

        private void HandleOutput(object sender, ContentBuildEventArgs e)
        {
            OutputText += e.Text + "\n";
        }
    }
}
