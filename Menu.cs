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
    public class MenuActionEventArgs : EventArgs
    {
        public Keys PressedKey { get; set; }
    }

    class Menu
    {
        //menu location relative to main window
        //default = 0,0
        public Point Loc { get; }
        private Point size;
        private List<MenuItem> menuItems;
        private SPACING spacing;
        public int SelectedIndex { get; set; }
        public int MemberCount { get { return menuItems.Count; } }
        public event EventHandler<MenuActionEventArgs> ESCPressed;

        public enum SPACING
        {
            SPACING_WIDE,
            SPACING_REGULAR,
            SPACING_CLOSE,
            SPACING_COLLIDING
        };

        /// <summary>
        /// l(ocation) is 0,0 by default, and s(ize) can be set to screen size
        /// </summary>
        /// <param name="l"></param>
        /// <param name="s"></param>
        public Menu(Point l, Point s)
        {
            menuItems = new List<MenuItem>();
            spacing = SPACING.SPACING_REGULAR;
            SelectedIndex = -1;
            Loc = l;
            size = s;
        }

        public Menu(Point s)
            : this(new Point(0, 0), s) { }

        public Menu(int x, int y, int width, int height)
            : this(new Point(x, y), new Point(width, height)) { }

        public void SetOrientationOnAll(MenuItem.MENUITEM_ORIENTATION o)
        {
            foreach(MenuItem mi in menuItems)
            {
                mi.SetOrientation(o);
            }
            AlignAll();
        }

        public void SetSpacing(SPACING s)
        {
            spacing = s;
            AlignAll();
        }

        private void AlignAll()
        {
            int verticalSpace = size.Y;
            int usedVerticalSpace = 0;
            int newTotalHeight = 0;

            int firstItemX = 0;
            foreach (MenuItem mi in menuItems)
            {
                usedVerticalSpace += mi.GetSize().Y;
                switch (mi.GetOrientation())
                {
                    case MenuItem.MENUITEM_ORIENTATION.MENUITEM_ORIENTATION_CENTERED:
                        {
                            //menu.loc.x = size.x/2 - menu.size.width/2
                            mi.SetLocation(size.X/2 - mi.GetSize().X/2, mi.GetLocation().Y);
                            break;
                        }
                    case MenuItem.MENUITEM_ORIENTATION.MENUITEM_ORIENTATION_CENTERED_RIGHT:
                        {
                            if (firstItemX == 0)
                                firstItemX = size.X / 2 + mi.GetSize().X / 2;
                            mi.SetLocation(firstItemX-mi.GetSize().X, mi.GetLocation().Y);
                            break;
                        }
                    case MenuItem.MENUITEM_ORIENTATION.MENUITEM_ORIENTATION_CENTERED_LEFT:
                        {
                            if (firstItemX == 0)
                                firstItemX = size.X / 2 - mi.GetSize().X / 2;
                            mi.SetLocation(firstItemX, mi.GetLocation().Y);
                            break;
                        }
                    case MenuItem.MENUITEM_ORIENTATION.MENUITEM_ORIENTATION_LEFTBOUND:
                        {
                            break;
                        }
                    case MenuItem.MENUITEM_ORIENTATION.MENUITEM_ORIENTATION_RIGHTBOUND:
                        {
                            break;
                        }
                }
            }
            switch (spacing)
            {
                case SPACING.SPACING_REGULAR:
                    {
                        verticalSpace -= usedVerticalSpace;
                        verticalSpace /= 3;
                        newTotalHeight = verticalSpace + usedVerticalSpace;
                        break;
                    }
                case SPACING.SPACING_CLOSE:
                    {
                        verticalSpace -= usedVerticalSpace;
                        verticalSpace /= 5;
                        newTotalHeight = verticalSpace + usedVerticalSpace;
                        break;
                    }
                case SPACING.SPACING_WIDE:
                    {
                        verticalSpace -= usedVerticalSpace;
                        verticalSpace = (int)(verticalSpace * 0.8d);
                        newTotalHeight = verticalSpace + usedVerticalSpace;
                        break;
                    }
                case SPACING.SPACING_COLLIDING:
                    {
                        verticalSpace -= usedVerticalSpace;
                        verticalSpace = 0;
                        newTotalHeight = verticalSpace + usedVerticalSpace;
                        break;
                    }
            }

            int lastItemsHeight = 0;
            int startingPointY = (size.Y - newTotalHeight)/2;
            for (int iii = 0; iii < menuItems.Count; iii++)
            {
                menuItems[iii].SetLocation(menuItems[iii].GetLocation().X, startingPointY + iii * (newTotalHeight/(menuItems.Count-1)));
                lastItemsHeight = menuItems[iii].GetSize().Y;
            }


        }

        public MenuItem[] GetItems()
        {
            return menuItems.ToArray();
        }

        public void AddItem(MenuItem mi)
        {
            if(mi != null)
                menuItems.Add(mi);
        }

        public MenuItem GetItem(int index)
        {
            return index >= 0 && index < menuItems.Count ? menuItems[index] : null;
        }

        public void SelectItem(int index)
        {
            if (index >= 0 && index < menuItems.Count)
            {
                if (SelectedIndex >= 0 && SelectedIndex < menuItems.Count)
                    menuItems[SelectedIndex].SetState(MenuItem.MENUITEM_STATE.MENUITEM_STATE_PASSIVE);
                SelectedIndex = index;
                menuItems[SelectedIndex].SetState(MenuItem.MENUITEM_STATE.MENUITEM_STATE_ACTIVE);
            }
                
        }

        public void HandleInput(Keys k)
        {
            if (SelectedIndex >= 0 && SelectedIndex < menuItems.Count)
                menuItems[SelectedIndex].SendKey(this, k);
            SendKey(k);
        }


        public void ResetMenuItemStatus()
        {
            SelectedIndex = -1;
            foreach(MenuItem mi in menuItems)
            {
                mi.SetState(MenuItem.MENUITEM_STATE.MENUITEM_STATE_PASSIVE);
            }
        }

        /// <summary>
        /// id is case sensitive
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MenuItem GetItem(string id)
        {
            foreach(MenuItem mi in menuItems)
            {
                if (mi.id.Equals(id))
                    return mi;
            }
            return null;
        }



        private void SendKey(Keys key)
        {
            MenuActionEventArgs args = new MenuActionEventArgs();
            args.PressedKey = key;

            OnESCPressed(args, this);
        }

        protected virtual void OnESCPressed(MenuActionEventArgs e, Menu sender)
        {
            if (e.PressedKey == Keys.Escape)
            {
                ESCPressed?.Invoke(sender, e);
            }
        }

    }
}
