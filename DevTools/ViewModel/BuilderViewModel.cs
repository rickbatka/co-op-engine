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
            OutputText = "Gathering Files...\n";

            DirectoryInfo dinfo = new DirectoryInfo(InputDirectory);
            FileInfo[] infos = dinfo.GetFiles("*.png");
            infos.ToList().AddRange(dinfo.GetFiles("*.jpg"));
            infos.ToList().AddRange(dinfo.GetFiles("*.tif"));
            infos.ToList().AddRange(dinfo.GetFiles("*.bmp"));

            contentBuilder.Clear();

            foreach (FileInfo finfo in infos)
            {
                OutputText += finfo.FullName + "\n";
                contentBuilder.Add(finfo.FullName, finfo.Name, "TextureImporter", "TextureProcessor");
            }

            string builtDir = contentBuilder.Build();
            OutputText += " - Done Building - \nCopying Files to:\n" + OutputDirectory + "\n";

            DirectoryInfo builtDirectoryInfo = new DirectoryInfo(builtDir);
            FileInfo[] builtFiles = builtDirectoryInfo.GetFiles();

            foreach (FileInfo builtFile in builtFiles)
            {
                OutputText += builtFile.Name + "\n";
                builtFile.CopyTo(OutputDirectory + "\\" + builtFile.Name, true);
            }
        }
    }
}
