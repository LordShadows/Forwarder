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

        public static void AddJournalEntry(String entry)
        {
            MAINWINDOW.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                MAINWINDOW.lbMessages.Items.Add($"> {DateTime.Now.ToString()} {entry}");
            }));
        }

        public static void Shutdown()
        {
            MAINWINDOW.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() =>
            {
                App.Current.Shutdown();
            }));
        }
    }
}
