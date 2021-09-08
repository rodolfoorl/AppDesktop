using SQLite;
using System;
using System.Collections.Generic;

namespace Desktop.GrupoBW.DAL.Entidades
{
    public class Parcelas
    {
        public Parcelas()
        {

        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int IdTitulo { get; set; }
        public int NumeroParcela { get; set; }
        public DateTime DataVencimento { get; set; } = DateTime.Now;
        public double ValorParcela { get; set; } 
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime DataAlteracao { get; set; } = DateTime.Now;
        [Ignore]
        public bool Vencido
        {
            get
            {
                return DataVencimento.Date < DateTime.Now ? true : false;
            }
            set { }
        }
    }
}
