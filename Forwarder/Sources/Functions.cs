using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
                        AUTHORIZATION.bGo.IsEnabled = true;
                        AUTHORIZATION.isWaiting = false;
                    }));
                    break;
            }
        }

        public static void UpdateUsersData(String users, String usersCB)
        {
            List<ClassResource.User> tempList = JsonConvert.DeserializeObject<List<ClassResource.User>>(users);
            List<String> tempStringList = JsonConvert.DeserializeObject<List<String>>(usersCB);
            MAINWINDOW.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                MAINWINDOW.UpdateUsers(tempStringList);
                MAINWINDOW.UpdateUsersData(tempList);
            }));
        }

        public static void UpdateEngineersData(String engineers)
        {
            List<ClassResource.Engineer> tempList = JsonConvert.DeserializeObject<List<ClassResource.Engineer>>(engineers);
            MAINWINDOW.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                MAINWINDOW.UpdateEngineersData(tempList);
            }));
        }

        public static void UpdateForwardersData(String engineers)
        {
            List<ClassResource.Forwarder> tempList = JsonConvert.DeserializeObject<List<ClassResource.Forwarder>>(engineers);
            MAINWINDOW.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                MAINWINDOW.UpdateForwardersData(tempList);
            }));
        }

        public static void UpdateCompaniesData(String companies)
        {
            List<ClassResource.Company> tempList = JsonConvert.DeserializeObject<List<ClassResource.Company>>(companies);
            MAINWINDOW.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                MAINWINDOW.UpdateCompaniesData(tempList);
            }));
        }

        public static void UpdateRequestsData(String requests)
        {
            List<ClassResource.Request> tempList = JsonConvert.DeserializeObject<List<ClassResource.Request>>(requests);
            MAINWINDOW.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                MAINWINDOW.UpdateRequestsData(tempList);
            }));
        }

        public static void UpdateDestinationsData(String destinations)
        {
            List<ClassResource.Destination> tempList = JsonConvert.DeserializeObject<List<ClassResource.Destination>>(destinations);
            MAINWINDOW.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                MAINWINDOW.UpdateDestinationsData(tempList);
            }));
        }

        public static void UpdateRoutesData(String routes)
        {
            List<ClassResource.Route> tempList = JsonConvert.DeserializeObject<List<ClassResource.Route>>(routes);
            MAINWINDOW.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                MAINWINDOW.UpdateRoutesData(tempList);
            }));
        }

        public static void AccountData(String login, String name, String role, String snapping)
        {
            MAINWINDOW.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                MAINWINDOW.USERLOGIN = login;
                MAINWINDOW.USERNAME = name;
                MAINWINDOW.USERROLE = role;
                MAINWINDOW.USERSNAPPING = snapping;
                MAINWINDOW.InitWindow();
            }));
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
