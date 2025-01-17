using HYPERMAGE.Models;
using HYPERMAGE.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYPERMAGE.Managers
{
    public static class UIManager
    {
        public static readonly List<UIElement> uiList = [];
        public static void Clear()
        {
            uiList.Clear();
        }
        public static void AddElement(UIElement ui)
        {
            uiList.Add(ui);
        }

        public static void RemoveElement(UIElement ui)
        {
            uiList.Remove(ui);
        }

        public static void UpdateUI()
        {
            foreach (var ui in uiList.ToList())
            {
                ui.Update();
            }
        }

        public static void Update()
        {
            UpdateUI();
        }

        public static void Draw()
        {
            foreach (var ui in uiList.ToList())
            {
                ui.Draw();
            }
        }
    }
}
