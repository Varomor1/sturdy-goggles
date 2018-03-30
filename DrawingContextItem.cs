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
    class DrawingContextItem
    {
        public enum DRAWING_TYPE
        {
            TEXT,
            IMAGE
        };


        public DRAWING_TYPE DrawingType {get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public string Text { get; set; }
        public Texture2D Image { get; set; }

    }
}
