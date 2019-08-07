using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GoLV2.scr.Classes
{
    public class CanvasButton : Button
    {
        public readonly int _row;
        public readonly int _column;
        public CanvasButton(int row, int col) : base()
        {
            _row = row;
            _column = col;
        }
    }
}
