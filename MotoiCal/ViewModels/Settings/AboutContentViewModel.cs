using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace MotoiCal.ViewModels.Settings
{
    class AboutContentViewModel
    {
        private StringBuilder aboutText;
        private StringBuilder easterEggText;

        private readonly string versionName = "TheRedBaron";

        private readonly string easterEggDate = "2 Jun"; //DD MMM, YYYY
        private readonly string easterEggTitle = "\nDid you know?\n";
        private readonly string easterEggMessage = "On this day, Michael Schumacher won his first Ferrari victory.\n" +
            "At the start of the race, he lost several positions due to a clutch problem.\n" +
            "Despite this, he produced a stunning drive in torrential rain and earned the nickname \"Rain Master\"." +
            "All while finising the 1996 Spanish GP 45 Seconds ahead of Jean Alesi.";

        public AboutContentViewModel()
        {

        }

        public string AppTitle => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductName;

        public string AppVersion => $" Version {FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion}";

        public string ReleaseDate => File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).ToString("dd MMMM yyyy"); //12 November 2020

        public string WindowTitle => $"{this.AppTitle} v{this.AppVersion}";

        public string VersionName => $"\"{this.versionName}\"";

        public string AppURL => $"https://www.github.com/Plus1XP";

        public string AboutText => this.GetAboutText();

        public string EasterEggText => this.GetEasterEggText();

        public bool IsEasterEggActive()
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
                //this.easterEggText.AppendLine("");
                //this.easterEggText.AppendLine("");
                //this.easterEggText.AppendLine("");
                //this.easterEggText.AppendLine("");
            }

            return this.easterEggText.ToString();
        }
    }
}
