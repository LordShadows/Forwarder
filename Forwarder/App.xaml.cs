using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Forwarder
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            InitializeComponent();
        }

        [STAThread]
        static void Main()
        {
            App app = new App();

            AdditionalWindows.Authorization authorization = new AdditionalWindows.Authorization();
            Dialogs.Dialog.WINDOW = authorization;

            if (!Sources.Client.InitClient()) return;

            authorization.ShowDialog();
            //if (!authorization.isAuthorization) return;

            //MainWindow window = new MainWindow();
            //Dialogs.Dialog.WINDOW = window;
            //app.Run(window);
        }
    }
}
