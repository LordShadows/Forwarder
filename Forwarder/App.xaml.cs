using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;

namespace Forwarder
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AdditionalWindows.Splash splash = new AdditionalWindows.Splash();
            this.MainWindow = splash;
            Dialogs.Dialog.WINDOW = splash;
            splash.Show();

            MainWindow mainWindow = new MainWindow();
            AdditionalWindows.Authorization authorization = new AdditionalWindows.Authorization();

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(900);

                if(!Sources.Client.InitClient())return;
                
                Thread.Sleep(3000);

                this.Dispatcher.Invoke(() =>
                {
                    this.MainWindow = mainWindow;
                    Dialogs.Dialog.WINDOW = mainWindow;
                    splash.Close();
                    Task.Factory.StartNew(() => 
                    {
                        Thread.Sleep(300);
                        this.Dispatcher.Invoke(() =>
                        {
                            authorization.ShowDialog();
                            mainWindow.Show();
                        });
                    });
                });
            });
        }
    }
}
