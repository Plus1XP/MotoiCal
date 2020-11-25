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

        private readonly string versionName = "Hammertime";

        private readonly string easterEggDate = "15 Nov"; //DD MMM, YYYY
        private readonly string easterEggTitle = "\nDid you know?\n";
        private readonly string easterEggMessage = "On this day, Lewis Hamilton became the joint most successful driver of all time with seven titles.\n" +
                                                    "His famous catchphrase came from his engineer's need to circumvent the heavy radio restrictions of the time.\n" +
                                                     "\"You can’t just say, push, because, you don’t know how hard, do you want to put a number on it?\n" +
                                                      "Let’s just use a different language. So, for an all-out lap it turned out to be Hammertime.\n" +
                                                       "Lewis suggested ‘put the hammer down’ but I thought that doesn’t sound right..\"";

        public AboutContentViewModel()
        {

        }

        public string AppTitle => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductName;

        public string AppVersion => $" Version {FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion}";

        public string ReleaseDate => File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).ToString("dd MMMM yyyy"); //12 November 2020

        public string WindowTitle => $"{this.AppTitle} v{this.AppVersion}";

        public string VersionName => $"\"{this.versionName}\"";

        public string AppURL => $"https://www.github.com/aleuts";

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
