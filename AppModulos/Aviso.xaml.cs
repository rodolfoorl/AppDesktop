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

namespace Desktop.GrupoBW.AppModulos
{
    /// <summary>
    /// Interaction logic for Aviso.xaml
    /// </summary>
    public partial class Aviso : UserControl
    {
        public IJanelaInicial JanelaInicial;

        public Aviso()
        {
            InitializeComponent();
        }

        private void ClickFechar(object sender, RoutedEventArgs e)
        {
            FlyoutAviso.IsOpen = false;
        }
    }
}
