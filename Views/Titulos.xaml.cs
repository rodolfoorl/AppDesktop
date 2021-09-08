using Desktop.GrupoBW.BLL.BO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Desktop.GrupoBW.Views
{
    /// <summary>
    /// Interaction logic for Titulos.xaml
    /// </summary>
    public partial class Titulos : UserControl
    {
        readonly IJanelaInicial JanelaInicial;
        readonly TitulosBO TitulosBO;
        List<DAL.Entidades.Titulos> ListaTitulos;

        public Titulos(IJanelaInicial janela)
        {
            try
            {
                InitializeComponent();
                JanelaInicial = janela;
                TitulosBO = new TitulosBO();
            }
            catch(Exception ex)
            {
                JanelaInicial.GravarErro(ex);
            }
        }

        #region Click Button
        private async void BtnPesquisar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                JanelaInicial.Carregando(true);

                string Nome = string.IsNullOrEmpty(TextBoxPesquisa.Text) ? string.Empty : TextBoxPesquisa.Text;
                ListaTitulos = await Task.Run(() => TitulosBO.BuscarTitulo(Nome));
                DataGridTitulos.ItemsSource = ListaTitulos;

                JanelaInicial.ShowAviso(string.Format("Fora encontrados {0} títulos.", ListaTitulos.Count));
            }
            catch (Exception ex)
            {
                JanelaInicial.GravarErro(ex);
            }
            finally
            {
                JanelaInicial.Carregando(false);
            }
        }

        private void BtnCriar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DAL.Entidades.Titulos titulo = new DAL.Entidades.Titulos();
                EditarTitulo(titulo);
            }
            catch (Exception ex)
            {
                JanelaInicial.GravarErro(ex);
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataGridTitulos_MouseDoubleClick(DataGridTitulos, null);
            }
            catch (Exception ex)
            {
                JanelaInicial.GravarErro(ex);
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DAL.Entidades.Titulos titulos = (DAL.Entidades.Titulos)DataGridTitulos.SelectedItem;

                if (titulos != null)
                {
                    titulos.Cancelado = true;
                    new TituloDetalhes(JanelaInicial).SalvarTitulo(titulos);
                    CollectionViewSource.GetDefaultView(DataGridTitulos.ItemsSource).Refresh();
                }

            }
            catch (Exception ex)
            {
                JanelaInicial.GravarErro(ex);
            }
        }
        #endregion


        #region Eventos Grid
        private void DataGridTitulos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DataGrid dataGrid = (DataGrid)sender;

                if (dataGrid != null)
                {
                    DAL.Entidades.Titulos titulo = (DAL.Entidades.Titulos)dataGrid.SelectedItem;

                    if (titulo != null)
                        GridDetalhes.DataContext = titulo;
                }
            }
            catch (Exception ex)
            {
                JanelaInicial.GravarErro(ex);
            }
        }

        private void GridDetalhes_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if (e.NewValue != null)
                {
                    if (e.NewValue.GetType() == new DAL.Entidades.Titulos().GetType())
                    {
                        StackDetalhes.Visibility = Visibility.Visible;
                        TextBlockNenhumTitulo.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        TextBlockNenhumTitulo.Visibility = Visibility.Visible;
                        StackDetalhes.Visibility = Visibility.Collapsed;
                    }
                }
            }
            catch (Exception ex)
            {
                JanelaInicial.GravarErro(ex);
            }
        }

        private void DataGridTitulos_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                DataGrid dataGrid = (DataGrid)sender;

                if (dataGrid != null)
                {
                    DAL.Entidades.Titulos titulo = (DAL.Entidades.Titulos)dataGrid.SelectedItem;

                    if (titulo != null)
                    {
                        EditarTitulo(titulo);
                    }
                }
            }
            catch (Exception ex)
            {
                JanelaInicial.GravarErro(ex);
            }
        }
        #endregion


        #region Edição
        public void EditarTitulo(DAL.Entidades.Titulos titulos)
        {
            TituloDetalhes tituloDetalhes = new TituloDetalhes(JanelaInicial);
            tituloDetalhes.DataContext = titulos;

            string title = titulos.Id > 0 ? string.Format("Título Nº {0}", titulos.NumeroTitulo) : "Novo Título";
            tituloDetalhes.BtnCancelarTitulo.IsEnabled = titulos.Id > 0 ? true : false;
            tituloDetalhes.IsEnabled = !titulos.Cancelado;
            JanelaInicial.AbrirPopup(title, tituloDetalhes);
        }
        #endregion
    }
}
