using MahApps.Metro.Controls;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Desktop.GrupoBW
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class JanelaInicial : MetroWindow, IJanelaInicial
    {

        Views.Titulos Titulos;
        Views.Relatorios Relatorios;

        public JanelaInicial()
        {
            try
            {
                InitializeComponent();
                DelegarJanelaPai(this);
                SetVersaoApp();
                SetLogoBW();
                DelegarClickMenus();
            }
            catch (Exception ex)
            {
                GravarErro(ex);
            }
        }

        #region Contrutores
        private void DelegarJanelaPai(IJanelaInicial janela)
        {
            ucAviso.JanelaInicial =
            ucMenuLateral.JanelaInicial =
            ucStatus.JanelaInicial = janela;
        }

        private void SetVersaoApp()
        {
            ucStatus.StatusVersao.Text = string.Format("VERSÃO {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        private void SetLogoBW()
        {
            ContentControlViews.Content = new AppModulos.Logo();
        }

        private void DelegarClickMenus()
        {
            ucMenuLateral.BtnTitulos.Click += BtnTitulos_Click;
            ucMenuLateral.BtnRelatorios.Click += BtnRelatorios_Click;
        }
        #endregion


        #region ButtonClick
        private void MenuSuperior_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ucMenuLateral.FlyoutMenu.IsOpen = !ucMenuLateral.FlyoutMenu.IsOpen;
            }
            catch (Exception ex)
            {
                GravarErro(ex);
            }
        }

        private void ClickOut(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (!e.Source.ToString().Contains("MenuLateral"))
                    ucMenuLateral.FlyoutMenu.IsOpen = false;

                if (!e.Source.ToString().Contains("Aviso"))
                    ucAviso.FlyoutAviso.IsOpen = false;
            }
            catch (Exception ex)
            {
                GravarErro(ex);
            }
        }


        private void BtnRelatorios_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Relatorios = new Views.Relatorios(this);
                ContentControlViews.Content = Relatorios;
                ucMenuLateral.FlyoutMenu.IsOpen = false;
            }
            catch (Exception ex)
            {
                GravarErro(ex);
            }
        }

        private void BtnTitulos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Titulos = new Views.Titulos(this);
                ContentControlViews.Content = Titulos;
                ucMenuLateral.FlyoutMenu.IsOpen = false;
            }
            catch (Exception ex)
            {
                GravarErro(ex);
            }
        }
        #endregion


        #region Metodos Interface
        public void GravarErro(Exception ex)
        {
            ShowAviso(ex.Message);
            Util.Log.GravarLog(ex);
        }

        public void ShowAviso(string mensagem)
        {
            try
            {
                ucAviso.AvisoMSG.Text = mensagem;
                ucAviso.FlyoutAviso.IsOpen = true;
            }
            catch (Exception ex)
            {
                GravarErro(ex);
            }
        }

        public async void Carregando(bool Carregando)
        {
            if (Carregando)
            {
                ucStatus.StatusProgressBar.Visibility = Visibility.Visible;
                await this.ShowOverlayAsync();
            }
            else
            {
                ucStatus.StatusProgressBar.Visibility = Visibility.Collapsed;
                await this.HideOverlayAsync();
            }
        }
        #endregion


        #region Popup
        public void AbrirPopup(string Titulo, UserControl UC)
        {
            AppModulos.Popup popup = new AppModulos.Popup(this);
            popup.DataContext = UC;
            popup.Title = Titulo;
            popup.Owner = this;
            popup.Show();
            Carregando(true);
        }

        public void FecharPopup(UserControl UC)
        {
            foreach (var item in UC.GetAncestors())
            {
                if (item.GetType().Name.Equals(typeof(AppModulos.Popup).Name))
                    ((MetroWindow)item).Close();
            }
        }
        #endregion
    }
}
