using Desktop.GrupoBW.DAL.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Desktop.GrupoBW.DAL.DAO
{
    public class TitulosDAO : Database
    {
        public TitulosDAO()
        {
            Tabelas tabelas = new Tabelas();
        }

        public async Task<bool> SavarTituloAsync(Titulos titulo)
        {

            try
            {
                bool retorno = false;

                if (titulo.Id == 0)
                    retorno = await base._database.InsertAsync(titulo) > 0;
                else
                {
                    titulo.DataAlteracao = DateTime.Now;
                    retorno = await base._database.UpdateAsync(titulo) > 0;
                    (await ListarParcelasAsync(titulo.Id)).ForEach(async delegate (Parcelas p)
                    {
                        retorno = await base._database.DeleteAsync(p) > 0;
                    });
                }

                foreach (var item in titulo.Parcelas)
                {
                    item.IdTitulo = titulo.Id;
                    retorno = await base._database.InsertAsync(item) > 0;
                }

                return retorno;
            }
            catch(Exception ex)
            { 
                return false;
            }
        }

        public async Task<List<Titulos>> ListarTituloAsync(string Nome)
        {
            try
            {
                List<Titulos> titulos = new List<Titulos>();

                titulos = await base._database.Table<Titulos>().ToListAsync();

                foreach (var item in titulos)
                {
                    item.Parcelas = await base._database.Table<Parcelas>().Where(c => c.IdTitulo.Equals(item.Id)).ToListAsync();
                }

                titulos = titulos.Where(p => p.NomeDevedor.ToLower().Contains(Nome.ToLower())).ToList();

                return titulos;
            }
            catch
            {
                return new List<Titulos>();
            }
        }

        public async Task<List<Parcelas>> ListarParcelasAsync(int IdTitulo)
        {
            try
            {
                List<Parcelas> parcelas = new List<Parcelas>();

                parcelas = await base._database.Table<Parcelas>().Where(c => c.IdTitulo.Equals(IdTitulo)).ToListAsync();

                return parcelas;
            }
            catch
            {
                return new List<Parcelas>();
            }
        }
    }
}
