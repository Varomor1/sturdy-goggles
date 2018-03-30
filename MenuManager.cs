using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    class MenuManager
    {
        private Dictionary<string, Menu> menus;
        private Menu activeMenu;

        public MenuManager()
        {
            menus = new Dictionary<string, Menu>();
        }

        public Menu GetMenu(string key)
        {
            return menus.ContainsKey(key) ? menus[key] : null;
        }

        public Menu GetActiveMenu()
        {
            return activeMenu;
        }

        public void SetActiveMenu(string key)
        {
            if (menus.Keys.Contains(key))
            {
                activeMenu = menus[key];
                activeMenu.ResetMenuItemStatus();
            }
                
        }

        public void AddMenu(string key, Menu m)
        {
            if(m != null && key != null)
                menus.Add(key, m);
        }

        public int GetXOffset() { return activeMenu.Loc.X; }
        public int GetYOffset() { return activeMenu.Loc.Y; }
        public Point GetOffset() { return activeMenu.Loc; }


        /// <summary>
        /// this method needs to be called in a already managed way
        /// </summary>
        /// <param name="k"></param>
        public void HandleInput(Keys k)
        {
            if (k == Keys.S || k == Keys.Down)
            {
                if (activeMenu.SelectedIndex < activeMenu.MemberCount - 1)
                    activeMenu.SelectItem(activeMenu.SelectedIndex + 1);
                else
                    activeMenu.SelectItem(0);
            }
            if (k == Keys.W || k == Keys.Up)
            {
                if (activeMenu.SelectedIndex > 0)
                    activeMenu.SelectItem(activeMenu.SelectedIndex - 1);
                else
                    activeMenu.SelectItem(activeMenu.MemberCount-1);
            }
            activeMenu.HandleInput(k);
        }

    }
}
