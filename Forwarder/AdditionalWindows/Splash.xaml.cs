using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;

namespace Forwarder.AdditionalWindows
{
    /// <summary>
    /// Логика взаимодействия для Splash.xaml
    /// </summary>
    public partial class Splash : Window
    {
        DispatcherTimer dispatcherTimer1;
        DispatcherTimer dispatcherTimer2;

        private bool isRealClose = false;

        public Splash()
        {
            InitializeComponent();
            this.Opacity = 0;
        }

        #region Функция первого таймера (быстрый вылет слева)
        private void DispatcherTimer1_Tick(object sender, EventArgs e)
        {
            this.Opacity = this.Opacity + 0.01;

            if (this.Opacity + 0.01 > 1)
            {
                this.Opacity = 1;
                dispatcherTimer1.Stop();
            }
        }
        #endregion

        #region Функция второго таймера (плавное перемещение)
        private void DispatcherTimer2_Tick(object sender, EventArgs e)
        {
            this.Opacity = this.Opacity - 0.075;

            if (this.Opacity - 0.075 < 0)
            {
                dispatcherTimer2.Stop();
                this.Close();
            }
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dispatcherTimer1 = new DispatcherTimer();
            dispatcherTimer1.Tick += new EventHandler(DispatcherTimer1_Tick);
            dispatcherTimer1.Interval = new TimeSpan(0, 0, 0, 0, 1);
            dispatcherTimer1.Start();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!isRealClose)
            {
                e.Cancel = true;
                isRealClose = true;
                dispatcherTimer2 = new DispatcherTimer();
                dispatcherTimer2.Tick += new EventHandler(DispatcherTimer2_Tick);
                dispatcherTimer2.Interval = new TimeSpan(0, 0, 0, 0, 1);
                dispatcherTimer2.Start();
            }
            
        }
    }
}
