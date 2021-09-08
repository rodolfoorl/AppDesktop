using Desktop.GrupoBW.DAL.Entidades;

namespace Desktop.GrupoBW.DAL
{
    public class Tabelas : Database
    {
        public Tabelas()
        {
            _database.CreateTableAsync<Titulos>().Wait();
            _database.CreateTableAsync<Parcelas>().Wait();
        }
    }
}
