using System;

namespace MotoiCal.Models.ButtonManagement
{
    public class ButtonStatusModel
    {
        private bool buttonActive;

        public bool ButtonActive
        {
            get { return this.buttonActive; }
            set
            {
                this.buttonActive = value;
                this.OnButtonStatusChanged(EventArgs.Empty);
            }
        }

        public string Name { get; set; }

        public string ToolTip { get; set; }

        public EventHandler ButtonStatusChanged { get; set; }

        public ButtonStatusModel(string name, string toolTip)
        {
            this.Name = name;
            this.ToolTip = toolTip;
        }

        public void OnButtonStatusChanged(EventArgs e)
        {
            this.ButtonStatusChanged?.Invoke(this, e);
        }
    }
}
