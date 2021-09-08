using MahApps.Metro.Controls;

namespace Desktop.GrupoBW.AppModulos
{
    /// <summary>
    /// Interaction logic for Popup.xaml
    /// </summary>
    public partial class Popup : MetroWindow
    {
        IJanelaInicial JanelaInicial;
        public Popup(IJanelaInicial janelaInicial)
        {
            InitializeComponent();
            JanelaInicial = janelaInicial;
        }

        private void MetroWindow_Closed(object sender, System.EventArgs e)
        {
            JanelaInicial.Carregando(false);
        }
    }
}
