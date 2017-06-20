using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forwarder.Sources
{
    class Functions
    {
        public static MainWindow MAINWINDOW;
        public static AdditionalWindows.Authorization AUTHORIZATION;

        public static void AuthenticationAttempt(String result)
        {
            switch (result)
            {
                case "Yes":
                    AUTHORIZATION.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                    {
                        AUTHORIZATION.isAuthorization = true;
                        AUTHORIZATION.Close();
                    }));
                    break;
                case "No":
                    AUTHORIZATION.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
                    {
                        AUTHORIZATION.lError.Content = "Неправильно введен логин или пароль.";
                    }));
                    break;
            }
        }

        public static void Shutdown()
        {
            Dialogs.Dialog.WINDOW.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                App.Current.Shutdown();
            }));
        }
    }
}
