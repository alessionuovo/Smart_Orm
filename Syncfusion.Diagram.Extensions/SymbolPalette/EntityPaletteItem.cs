﻿using Syncfusion.Windows.Diagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Diagram.Extensions.SymbolPalette
{
    public class EntityPaletteItem : SymbolPaletteItem
    {
        public override object CloneContent()
        {
            return new Node();
        }
    }
}
