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


    class MenuItem
    {

        public event EventHandler<MenuActionEventArgs> ButtonPressed;
        private MenuItemDrawingContext DrawingContext;

        public enum MENUITEM_STATE
        {
            MENUITEM_STATE_PASSIVE = 0,
            MENUITEM_STATE_ACTIVE = 1,
            MENUITEM_STATE_PRESSED = 2
        };

        public enum MENUITEM_ORIENTATION
        {
            MENUITEM_ORIENTATION_CENTERED,
            MENUITEM_ORIENTATION_CENTERED_LEFT,
            MENUITEM_ORIENTATION_CENTERED_RIGHT,
            MENUITEM_ORIENTATION_LEFTBOUND,
            MENUITEM_ORIENTATION_RIGHTBOUND
        };

        private MENUITEM_STATE state;
        private MENUITEM_ORIENTATION orientation;

        public string id;
        private string text;
        private Texture2D[] texture;
        private SpriteFont[] font;

        private Point loc;
        private Point size;


        public MenuItem(string name, SpriteFont f, Point l, Point s, Texture2D t)
        {
            text = name;
            id = name;

            loc = l;
            size = s;
            
            texture = new Texture2D[3];
            texture[0] = t; texture[1] = t; texture[2] = t;

            font = new SpriteFont[3];
            font[0] = f; font[1] = f; font[2] = f;
        }

        public MenuItem(string name, SpriteFont f, Point l, Point s)
            : this(name, f, l, s, null) { }

        public MenuItem(string name, SpriteFont f, Point s)
            : this(name, f, new Point(0,0), s, null) { }

        public void SetTexture(Texture2D t, MENUITEM_STATE s)
        {
            texture[(int)s] = t;
        }

        public void SetFont(SpriteFont t)
        {
            font[0] = t; font[1] = t; font[2] = t;
        }

        public void SetFont(SpriteFont t, MENUITEM_STATE s)
        {
            font[(int)s] = t;
        }

        public void SetText(string t)
        {
            text = t;
        }

        public void SetLocation(int x, int y)
        {
            loc.X = x;
            loc.Y = y;
        }

        public void SetLocation(Point v)
        {
            loc = v;
        }

        public void SetSize(int x, int y)
        {
            size.X = x;
            size.Y = y;
        }

        public void SetSize(Point v)
        {
            size = v;
        }

        public string GetText()
        {
            return text;
        }

        public Texture2D GetTexture()
        {
            return texture[(int)state];
        }

        public SpriteFont GetFont()
        {
            return font[(int)state];
        }

        public Texture2D GetTexture(MENUITEM_STATE s)
        {
            return texture[(int)s];
        }

        public SpriteFont GetFont(MENUITEM_STATE s)
        {
            return font[(int)s];
        }

        public MENUITEM_STATE GetState()
        {
            return state;
        }

        public void SetState(MENUITEM_STATE s)
        {
            state = s;
        }

        public MENUITEM_ORIENTATION GetOrientation()
        {
            return orientation;
        }

        public void SetOrientation(MENUITEM_ORIENTATION s)
        {
            orientation = s;
        }

        public Point GetLocation()
        {
            return loc;
        }

        public Point GetSize()
        {
            return size;
        }

        public void SendKey(Menu sender, Keys key)
        {
            MenuActionEventArgs args = new MenuActionEventArgs();
            args.PressedKey = key;

            OnButtonPressed(args, sender);
        }

        protected virtual void OnButtonPressed(MenuActionEventArgs e, Menu sender)
        {
            if (e.PressedKey == Keys.Space || e.PressedKey == Keys.Enter)
            {
                ButtonPressed?.Invoke(sender, e);
            }
        }
    }
}
