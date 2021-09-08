using Desktop.GrupoBW.BLL.BO;
using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace Desktop.GrupoBW.Views
{
    /// <summary>
    /// Interaction logic for TituloDetalhes.xaml
    /// </summary>
    public partial class TituloDetalhes : UserControl
    {
        IJanelaInicial JanelaInicial;

        TitulosBO titulosBO;

        public TituloDetalhes(IJanelaInicial janelaInicial)
        {
            InitializeComponent();
            JanelaInicial = janelaInicial;
            titulosBO = new TitulosBO();
        }


        #region Click Button
        private void BtnAddParcela_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MostrarControle();
            }
            catch (Exception ex)
            {
                JanelaInicial.GravarErro(ex);
            }
        }

        private void BtnSalvarParcela_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double valor = Util.Util.PegarNumeros(TextBoxValor.Text);
                DateTime vencimento = DataPickerVencimento.SelectedDate.HasValue ? DataPickerVencimento.SelectedDate.Value : DateTime.Now;
                List<DAL.Entidades.Parcelas> parcelas = (List<DAL.Entidades.Parcelas>)DataGridParcelas.ItemsSource;

                DAL.Entidades.Parcelas parcela = new DAL.Entidades.Parcelas
                {
                    NumeroParcela = parcelas.Count + 1
                    ,
                    ValorParcela = valor
                    ,
                    DataVencimento = vencimento
                };

                parcelas.Add(parcela);
                RefreshParcelas();

                MostrarControle();
            }
            catch (Exception ex)
            {
                JanelaInicial.GravarErro(ex);
            }
            finally
            {
                TextBoxValor.Text = string.Empty;
                DataPickerVencimento.SelectedDate = null;
            }
        }

        private void BtnDelParcela_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DAL.Entidades.Parcelas parcela = (DAL.Entidades.Parcelas)DataGridParcelas.SelectedItem;

                if (parcela != null)
                    DeletarParcela(parcela);

                RefreshParcelas();
            }
            catch (Exception ex)
            {
                JanelaInicial.GravarErro(ex);
            }
        }
        private void BtnCancelarTitulo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var titulos = (DAL.Entidades.Titulos)this.DataContext;
                titulos.Cancelado = true;
                SalvarTitulo(titulos);
            }
            catch (Exception ex)
            {
                JanelaInicial.GravarErro(ex);
            }
        }

        private void BtnSalvarTitulo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BtnSalvarTitulo.IsEnabled = false;
                var titulos = (DAL.Entidades.Titulos)this.DataContext;
                SalvarTitulo(titulos);
            }
            catch (Exception ex)
            {
                JanelaInicial.GravarErro(ex);
            }

        }
        #endregion


        #region Edição
        private void DeletarParcela(DAL.Entidades.Parcelas parcela)
        {
            List<DAL.Entidades.Parcelas> parcelas = (List<DAL.Entidades.Parcelas>)DataGridParcelas.ItemsSource;

            if (parcelas != null)
                parcelas.Remove(parcela);
        }

        private void MostrarControle()
        {
            GridControle.Visibility = GridControle.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            GridAddParcela.Visibility = GridAddParcela.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private void RefreshParcelas()
        {
            int parcela = 1;
            List<DAL.Entidades.Parcelas> parcelas = (List<DAL.Entidades.Parcelas>)DataGridParcelas.ItemsSource;

            parcelas.OrderBy(p => p.DataVencimento).ToList().ForEach(delegate (DAL.Entidades.Parcelas p)
            {
                p.NumeroParcela = parcela++;
            });

            DataGridParcelas.ItemsSource = parcelas.OrderBy(p => p.DataVencimento).ToList();

            CollectionViewSource.GetDefaultView(DataGridParcelas.ItemsSource).Refresh();
            RefreshGrid();
        }

        private void RefreshGrid()
        {
            var titulos = (DAL.Entidades.Titulos)this.DataContext;

            this.DataContext = null;
            this.DataContext = titulos;
        }

        public async void SalvarTitulo(DAL.Entidades.Titulos titulos)
        {
            if (ValidarTitulo(titulos))
            {
                await titulosBO.SalvarTitulo(titulos);
                JanelaInicial.FecharPopup(this);
            }
            BtnSalvarTitulo.IsEnabled = true;
        }

        private bool ValidarTitulo(DAL.Entidades.Titulos titulos)
        {
            bool retorno = true;

            if (string.IsNullOrEmpty(titulos.NomeDevedor))
            {
                BindingExpression bindingExpression = BindingOperations.GetBindingExpression(TextBoxNome, TextBox.TextProperty);
                BindingExpressionBase bindingExpressionBase = BindingOperations.GetBindingExpressionBase(TextBoxNome, TextBox.TextProperty);
                ValidationError validationError = new ValidationError(new ExceptionValidationRule(), bindingExpression, "Preencha o nome do devedor.", null);
                Validation.MarkInvalid(bindingExpressionBase, validationError);
                retorno = false;
            }

            return retorno;
        }
        #endregion


        #region Eventos
        private void LostFocus_Taxa(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBox textBox = ((TextBox)sender);
                var titulos = (DAL.Entidades.Titulos)this.DataContext;
                double taxa = Util.Util.PegarNumeros(textBox.Text);

                if (textBox.Tag.Equals("Juros"))
                    titulos.TaxaJuros = taxa;
                else if (textBox.Tag.Equals("Multa"))
                    titulos.TaxaMulta = taxa;

                RefreshGrid();
            }
            catch (Exception ex)
            {
                JanelaInicial.GravarErro(ex);
            }
        }
        #endregion

    }
}
