using SQLite;
using System;
using System.Collections.Generic;

namespace Desktop.GrupoBW.DAL.Entidades
{
    public class Titulos
    {
        public Titulos()
        {
            Parcelas = new List<Parcelas>();
        }

        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string NumeroTitulo { get; set; } = Guid.NewGuid().ToString();
        public string NomeDevedor { get; set; }
        public double TaxaJuros { get; set; }
        public double TaxaMulta { get; set; }
        public bool Cancelado { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime DataAlteracao { get; set; } = DateTime.Now;
        [Ignore]
        public List<Parcelas> Parcelas { get; set; }
        [Ignore]
        public string Status
        {
            get
            {
                return Cancelado ? "Cancelado" : "Ativo";
            }
            set { }
        }
        [Ignore]
        public int TotalParcelas
        {
            get
            {
                return Parcelas != null ? Parcelas.Count : 0;
            }
            set { }
        }
        [Ignore]
        public double Valor
        {
            get
            {
                double valor = 0;

                if (Parcelas != null)
                {
                    Parcelas.ForEach(delegate (Parcelas p)
                    {
                        valor += p.ValorParcela;

                    });
                }

                return valor;
            }
            set { }
        }
        [Ignore]
        public int DiasAtraso
        {
            get
            {
                int dias = 0;
                DateTime hoje = DateTime.Now;

                if (Parcelas != null)
                {
                    Parcelas.ForEach(delegate (Parcelas p)
                    {
                        if (p.DataVencimento.Date < hoje.Date)
                        {
                            int totaldias = (hoje.Date - p.DataVencimento.Date).Days;
                            dias = totaldias > dias ? totaldias : dias;
                        }
                    });
                }

                return dias;
            }
            set { }
        }
        [Ignore]
        public double ValorTotal
        {
            get
            {
                double valortotal = 0;
                DateTime hoje = DateTime.Now;

                if (Parcelas != null)
                {
                    bool atrasado = false;
                    valortotal = Valor;

                    Parcelas.ForEach(delegate (Parcelas p)
                    {
                        if (p.DataVencimento.Date < hoje.Date)
                        {
                            int dias = (hoje.Date - p.DataVencimento.Date).Days;
                            valortotal += p.ValorParcela * (((TaxaJuros / 30) * dias) / 100);
                            atrasado = true;
                        }
                    });

                    if (atrasado)
                        valortotal += Valor * (TaxaMulta / 100);
                }

                return valortotal;
            }
            set { }
        }
    }
}
