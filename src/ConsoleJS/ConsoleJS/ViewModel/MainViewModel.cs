using GalaSoft.MvvmLight;
using System;
using System.IO;

namespace ConsoleJS.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public GalaSoft.MvvmLight.Command.RelayCommand RunCommand { get; set; }

        public GalaSoft.MvvmLight.Command.RelayCommand ClearCommand { get; set; }

        private string scriptText;

        public string ScriptText
        {
            get { return scriptText; }
            set
            {
                scriptText = value;
                RaisePropertyChanged("ScriptText");
            }
        }

        private string scriptOutput;

        public string ScriptOutput
        {
            get { return scriptOutput; }
            set
            {
                scriptOutput = value;
                RaisePropertyChanged("ScriptOutput");
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            this.RunCommand = new GalaSoft.MvvmLight.Command.RelayCommand(this.RunScript);
            this.ClearCommand = new GalaSoft.MvvmLight.Command.RelayCommand(this.Clear);
        }

        private void RunScript()
        {
            if (!string.IsNullOrWhiteSpace(this.ScriptText))
            {
                try
                {
                    File.WriteAllText("tmp.js", this.ScriptText);
                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    proc.StartInfo.FileName = "runscript.bat";
                    proc.StartInfo.RedirectStandardError = false;
                    proc.StartInfo.RedirectStandardOutput = false;
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.CreateNoWindow = true;

                    proc.Start();
                    proc.WaitForExit();

                    var res = "error";
                    if (proc.ExitCode == 0)
                    {
                        res = File.ReadAllText("result.txt");
                    }
                    this.ScriptOutput = res;
                }
                catch (Exception e)
                {
                    this.ScriptOutput = e.Message;
                }
            }
            else
            {
                this.ScriptOutput = string.Empty;
            }
        }

        private void Clear()
        {
            this.ScriptOutput = string.Empty;
            this.ScriptText = string.Empty;
        }
    }
}