using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris
{
    class MenuItemDrawingContext :IEnumerable<DrawingContextItem>
    {
        private List<DrawingContextItem> Items;

        public MenuItemDrawingContext()
        {
            Items = new List<DrawingContextItem>();
        }

        public void AddDrawingContextItem(DrawingContextItem item)
        {
            Items.Add(item);
        }

        public MenuItemDrawingContextEnum GetEnumerator()
        {
            return new MenuItemDrawingContextEnum(Items.ToArray());
        }

        IEnumerator<DrawingContextItem> IEnumerable<DrawingContextItem>.GetEnumerator()
        {
            return (IEnumerator<DrawingContextItem>)GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }
    }

    class MenuItemDrawingContextEnum : IEnumerator<DrawingContextItem>
    {
        private int index = -1;

        public DrawingContextItem[] items;

        public MenuItemDrawingContextEnum(DrawingContextItem[] list)
        {
            items = list;
        }

        public DrawingContextItem Current
        {
            get
            {
                try
                {
                    return items[index];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public void Dispose()
        {
            //not implemented, do i have to?
        }

        public bool MoveNext()
        {
            index++;
            return (index < items.Length);
        }

        public void Reset()
        {
            index = -1;
        }
    }
}
