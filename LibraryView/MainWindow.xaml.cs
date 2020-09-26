using System;
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
using POILibrary;

namespace LibraryView
{
    public partial class MainWindow : Window
    {
        Library Library { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Library = new Library();
        }
    }
}
