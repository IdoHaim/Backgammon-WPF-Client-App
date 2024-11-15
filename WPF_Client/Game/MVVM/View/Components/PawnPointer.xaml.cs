﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Client.Game.MVVM.View.Components
{
    /// <summary>
    /// Interaction logic for PawnPointer.xaml
    /// </summary>
    public partial class PawnPointer : UserControl
    {
        public string OriginalLocation { get; set; }
        public PawnPointer()
        {
            InitializeComponent();
            OriginalLocation = string.Empty;
        }
    }
}
