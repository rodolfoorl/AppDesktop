using Desktop.GrupoBW.BLL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace Desktop.GrupoBW.Views
{
    /// <summary>
    /// Interaction logic for Relatorios.xaml
    /// </summary>
    public partial class Relatorios : UserControl
    {
        List<SeriesDados> Series { get; set; }

        readonly BLL.BO.TitulosBO TitulosBO;

        List<DAL.Entidades.Titulos> ListaTitulos;
        readonly IJanelaInicial JanelaInicial;

        public Relatorios(IJanelaInicial janelaInicial)
        {
            try
            {
                InitializeComponent();
                JanelaInicial = janelaInicial;

                TitulosBO = new BLL.BO.TitulosBO();
                BuscarDados();

                ComboBoxRelatorio.ItemsSource = new List<TiposRelatorios>(
                    new TiposRelatorios[]
                    {
                    new TiposRelatorios { Tipo = TiposRelatorio.TotalTitulosPorDia, Descricao = "Totalização de Títulos por Data" }
                    });
            }
            catch(Exception ex)
            {
                JanelaInicial.GravarErro(ex);
            }
        }

        #region Construtor
        private async void BuscarDados()
        {
            try
            {
                ListaTitulos = await TitulosBO.BuscarTitulo(string.Empty);
                ContruirGraficos(ListaTitulos);
            }
            catch (Exception ex)
            {
                JanelaInicial.GravarErro(ex);
            }
        }
        #endregion


        #region Graficos
        public void ContruirGraficos(List<DAL.Entidades.Titulos> titulos)
        {
            GraficoParcelaTitulos(titulos);
            GraficoTituloValor(titulos);
            GraficoTitulos(titulos);
        }

        public void GraficoParcelaTitulos(List<DAL.Entidades.Titulos> titulos)
        {
            ObservableCollection<Dados> TopProduto = new ObservableCollection<Dados>(
                titulos.GroupBy(p => new { p.NomeDevedor }).Select(p => new Dados
                {
                    Descricao = p.Key.NomeDevedor
                    ,
                    Valor = p.Sum(g => g.TotalParcelas)
                }).ToList().OrderByDescending(p => p.Valor).Take(5));

            ChartTopProduto.ItemsSource = TopProduto;
        }

        public void GraficoTituloValor(List<DAL.Entidades.Titulos> titulos)
        {
            ObservableCollection<Dados> TituloValor = new ObservableCollection<Dados>(
                titulos.GroupBy(p => new { p.NomeDevedor }).Select(p => new Dados
                {
                    Descricao = p.Key.NomeDevedor
                    ,
                    Valor = (float)p.Sum(g => g.ValorTotal)
                }).ToList().OrderByDescending(p => p.Valor).Take(5));

            Series = new List<SeriesDados>
            {
                new SeriesDados() { DisplayName = "Título/Valor", Items = TituloValor }
            };
            ChartTopVendas.SeriesSource = Series;
        }

        public void GraficoTitulos(List<DAL.Entidades.Titulos> titulos)
        {
            ObservableCollection<Dados> Titulos = new ObservableCollection<Dados>(
                titulos.GroupBy(p => new { p.Status }).Select(p => new Dados
                {
                    Descricao = p.Key.Status
                    ,
                    Valor = (float)p.Count()
                }).ToList().OrderByDescending(p => p.Valor).Take(5));

            Series = new List<SeriesDados>
            {
                new SeriesDados() { DisplayName = "Titulos", Items = Titulos }
            };
            ChartTitulos.SeriesSource = Series;
        }
        #endregion


        #region Relatorios
        private List<RelatorioTotalTitulosPorDia> GerarTotalTitulosPorDia()
        {
            return ListaTitulos.Select(p => new RelatorioTotalTitulosPorDia
            {
                Numero = p.NumeroTitulo
                ,
                DataAlteracao = p.DataAlteracao.ToString("dd/MM/yyyy HH:mm:ss")
                ,
                DataCriacao = p.DataCriacao.ToString("dd/MM/yyyy HH:mm:ss")
                ,
                DiasAtrasados = p.DiasAtraso.ToString()
                ,
                Nome = p.NomeDevedor
                ,
                Status = p.Status
                ,
                TaxaJuros = (p.TaxaJuros / 100).ToString("0%")
                ,
                TaxaMulta = (p.TaxaMulta / 100).ToString("0%")
                ,
                Total = p.Valor.ToString("c")
                ,
                ValorTotal = p.ValorTotal.ToString("c")
                ,
                TotalParcelas = p.TotalParcelas.ToString()
            }).OrderBy(p =>p.DataCriacao).ToList();
        }
        #endregion


        #region Click e Eventos
        private void ComboBoxRelatorio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBox comboBox = (ComboBox)sender;

                if (sender != null)
                {
                    var tipo = (TiposRelatorios)comboBox.SelectedItem;

                    switch (tipo.Tipo)
                    {
                        case TiposRelatorio.TotalTitulosPorDia:
                            DataGridRelatorio.ItemsSource = GerarTotalTitulosPorDia();
                            DataGridRelatorio.Columns[1].Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                JanelaInicial.GravarErro(ex);
            }
        }

        private void BtnExportar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string excel = string.Empty;
                string c = ";";
                string arquivo = string.Format("{0}Relatorio-{1}.csv", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyyMMddHHmmss"));

                PropertyInfo[] properties = ((IEnumerable<object>)DataGridRelatorio.ItemsSource).First().GetType().GetProperties();
                List<string> headerNames = properties.Select(prop => prop.Name).ToList();

                using (StreamWriter sw = File.CreateText(arquivo))
                {
                    for (int i = 0; i < headerNames.Count; i++)
                    {
                        excel += headerNames[i] + c;
                    }

                    foreach (var item in ((IEnumerable<object>)DataGridRelatorio.ItemsSource))
                    {
                        excel += Environment.NewLine;

                        foreach (var prop in properties)
                        {
                            excel += prop.GetValue(item) + c;
                        }
                    }
                    sw.Write(excel);
                }

                JanelaInicial.ShowAviso(string.Format("Relatório Exportado em {0}", arquivo));
            }
            catch (Exception ex)
            {
                JanelaInicial.GravarErro(ex);
            }
        }
        #endregion

    }


}
