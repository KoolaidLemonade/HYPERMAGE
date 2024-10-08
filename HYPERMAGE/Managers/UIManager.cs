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
        public static readonly List<UIElement> _ui = [];

        public static void AddElement(UIElement ui)
        {
            _ui.Add(ui);
        }

        public static void UpdateUI()
        {
            foreach (var ui in _ui)
            {
                ui.Update();
            }
        }

        public static void Update()
        {
            UpdateUI();
        }

        public static void Draw(Player player, SpriteFont spriteFont)
        {
            foreach (var ui in _ui)
            {
                ui.Draw(player, spriteFont);
            }
        }
    }
}
