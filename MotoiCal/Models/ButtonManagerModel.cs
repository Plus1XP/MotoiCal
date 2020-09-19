using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoiCal.ViewModels
{
    class ButtonManagerModel
    {
        private List<ButtonStatusModel> allButtons;

        public ButtonManagerModel()
        {

            this.allButtons = new List<ButtonStatusModel>();
        }

        public void AddButton(ButtonStatusModel button)
        {
            this.allButtons.Add(button);
        }

        public void RemoveButton(ButtonStatusModel button)
        {
            this.allButtons.Remove(button);
        }

        public void SetActiveButton(ButtonStatusModel activeButton)
        {
            for (int i = 0; i < this.allButtons.Count; i++)
            {
                if (this.allButtons[i].Name != activeButton.Name)
                {
                    this.allButtons[i].ButtonActive = false;
                }
                else
                {
                    this.allButtons[i].ButtonActive = true;
                }
            }
        }
    }
}
