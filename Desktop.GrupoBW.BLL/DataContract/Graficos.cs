using System.Collections.ObjectModel;

namespace Desktop.GrupoBW.BLL
{

    public class SeriesDados
    {
        public string DisplayName { get; set; }

        public string Description { get; set; }

        public ObservableCollection<Dados> Items { get; set; }
    }
    public class Dados
    {
        public string Descricao { get; set; }

        private float _valor = 0;
        public float Valor
        {
            get { return _valor; }
            set { _valor = value; }
        }
    }
}
