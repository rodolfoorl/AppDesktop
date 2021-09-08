namespace Desktop.GrupoBW.BLL
{
    public class TiposRelatorios
    {
        public TiposRelatorio Tipo { get; set; }
        public string Descricao { get; set; }
    }
    public enum TiposRelatorio
    {
        TotalTitulosPorDia = 1
    }

    public class RelatorioTotalTitulosPorDia
    {
        public string Numero { get; set; }
        public string Nome { get; set; }
        public string TaxaJuros { get; set; }
        public string TaxaMulta { get; set; }
        public string TotalParcelas { get; set; }
        public string Total { get; set; }
        public string DiasAtrasados { get; set; }
        public string ValorTotal { get; set; }
        public string DataCriacao { get; set; }
        public string DataAlteracao { get; set; }
        public string Status { get; set; }
    }
}
