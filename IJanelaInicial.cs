using MahApps.Metro.Controls;
using System;
using System.Windows.Controls;

namespace Desktop.GrupoBW
{
    public interface IJanelaInicial
    {
        void ShowAviso(string mensagem);

        void Carregando(bool Carregando);

        void GravarErro(Exception ex);

        void AbrirPopup(string Titulo, UserControl UC);

        void FecharPopup(UserControl UC);
    }
}