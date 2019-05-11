﻿using DiagramContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Orm.DiagramResources
{
    public class DiagramProperties
    {
        #region Private Properties

        private bool p_SymbolPaletteEnable;
        private bool p_HorizantalRulers;
        private bool p_VerticalRulers;
        private bool p_HorizantalLines;
        private bool p_VerticalLines;
        private bool p_PanEnabled;
        private bool p_ZoomEnabled;
        private bool p_PageEdit;
        public Model model;
        #endregion

        #region Public Properties

        public bool SymbolPaletteEnable
        {
            get
            {
                return p_SymbolPaletteEnable;
            }
            set
            {
                p_SymbolPaletteEnable = value;
                OnPropertyChanged("SymbolPaletteEnable");
            }
        }

        public bool HorizantalRulers
        {
            get
            {
                return p_HorizantalRulers;
            }
            set
            {
                p_HorizantalRulers = value;
                OnPropertyChanged("HorizantalRulers");
            }
        }

        public bool VerticalRulers
        {
            get
            {
                return p_VerticalRulers;
            }
            set
            {
                p_VerticalRulers = value;
                OnPropertyChanged("VerticalRulers");
            }
        }

        public bool HorizantalLines
        {
            get
            {
                return p_HorizantalLines;
            }
            set
            {
                p_HorizantalLines = value;
                OnPropertyChanged("HorizantalLines");
            }
        }

        public bool VerticalLines
        {
            get
            {
                return p_VerticalLines;
            }
            set
            {
                p_VerticalLines = value;
                OnPropertyChanged("VerticalLines");
            }
        }

        public bool PanEnabled
        {
            get
            {
                return p_PanEnabled;
            }
            set
            {
                p_PanEnabled = value;
                OnPropertyChanged("PanEnabled");
            }
        }

        public bool ZoomEnabled
        {
            get
            {
                return p_ZoomEnabled;
            }
            set
            {
                p_ZoomEnabled = value;
                OnPropertyChanged("ZoomEnabled");
            }
        }

        public bool PageEdit
        {
            get
            {
                return p_PageEdit;
            }
            set
            {
                p_PageEdit = value;
                OnPropertyChanged("PageEdit");
            }
        }

        #endregion

        #region Event

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this,
                    new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
