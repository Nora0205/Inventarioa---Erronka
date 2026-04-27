using System;
using System.Windows.Forms;
using StockBaseApp.Kontrolagailuak;

namespace StockBaseApp
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Initialize();

            bool restart = true;
            while (restart)
            {
                LoginForm login = new LoginForm();
                if (login.ShowDialog() == DialogResult.OK)
                {
                    MainForm main = new MainForm(login.LoggedUser!);
                    DialogResult result = main.ShowDialog();
                    
                    if (result != DialogResult.Retry)
                    {
                        restart = false;
                    }
                }
                else
                {
                    restart = false;
                }
            }
        }
    }
}