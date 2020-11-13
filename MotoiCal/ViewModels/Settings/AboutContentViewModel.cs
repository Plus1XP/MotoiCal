using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MotoiCal.ViewModels.Settings
{
    class AboutContentViewModel
    {
        private StringBuilder aboutText;
        private StringBuilder easterEggText;

        private readonly string versionName = "Bazooka";

        private readonly string easterEggDate = "04 May"; //DD MMM, YYYY
        private readonly string easterEggTitle = "Did you know?";
        private readonly string easterEggMessage = "On this day, Lorenzo made his championship debut.\n" +
                                                    "It was the second qualifying day for the 2002 125cc Spanish Grand Prix.\n" +
                                                     "He missed Friday practice as he was not old enough to race!";

        public AboutContentViewModel()
        {

        }

        public string AppTitle => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductName;

        public string AppVersion => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;

        public string ReleaseDate => File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).ToString("dd MMMM yyyy"); //12 November 2020

        public string WindowTitle => $"{this.AppTitle} v{this.AppVersion}";

        public string VersionName => this.versionName;

        public string AppURL => $"https://www.github.com/aleuts";

        public string AboutText => this.GetAboutText();

        public string EasterEggText => this.GetEasterEggText();

        private bool IsEasterEggActive()
        {
            return DateTime.Parse(this.easterEggDate) == DateTime.Now.Date ? true : false;
        }

        private string GetAboutText()
        {
            this.aboutText = new StringBuilder();

            this.aboutText.AppendLine(this.AppTitle);
            this.aboutText.AppendLine($"v{this.AppVersion}");
            this.aboutText.AppendLine($"\"{this.VersionName}\"");
            this.aboutText.AppendLine(this.ReleaseDate);
            this.aboutText.AppendLine(this.AppURL);

            return this.aboutText.ToString();
        }

        private string GetEasterEggText()
        {
            this.easterEggText = new StringBuilder();

            if (this.IsEasterEggActive())
            {
                this.easterEggText.AppendLine(this.easterEggTitle);
                //easterEggText.AppendLine("+---------------------+");
                this.easterEggText.AppendLine(this.easterEggMessage);
            }
            else
            {
                this.easterEggText.Clear();
            } 

            return this.easterEggText.ToString();
        }
    }
}
