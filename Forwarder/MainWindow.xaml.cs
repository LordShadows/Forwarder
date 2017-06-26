using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Forwarder
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Объявление переменных для изменения размеров окна
        bool isRightResize = false;
        bool isLeftResize = false;
        bool isBottomResize = false;
        bool isTopResize = false;
        bool isRightBottomResize = false;
        bool isLeftTopResize = false;
        bool isRightTopResize = false;
        bool isLeftBottomResize = false;
        double positionRightResize = 0;
        double positionLeftResize = 0;
        double positionBottomResize = 0;
        double positionTopResize = 0;
        double positionXRightBottomResize = 0;
        double positionYRightBottomResize = 0;
        double positionXLeftTopResize = 0;
        double positionYLeftTopResize = 0;
        double positionXRightTopResize = 0;
        double positionYRightTopResize = 0;
        double positionXLeftBottomResize = 0;
        double positionYLeftBottomResize = 0;
        #endregion

        private List<ClassResource.User> USERS;
        private List<ClassResource.Engineer> ENGINEERS;
        private List<ClassResource.Forwarder> FORWARDERS;
        private List<ClassResource.Company> COMPANIES;
        private List<ClassResource.Request> REQUESTS;

        private ClassResource.User SELECTUSER;
        private ClassResource.Engineer SELECTENGINEER;
        private ClassResource.Forwarder SELECTFORWARDER;
        private ClassResource.Company SELECTCOMPANY;
        private ClassResource.Request SELECTREQUEST;

        public String USERLOGIN = "";
        public String USERNAME = "";
        public String USERROLE = "";

        private const double minWidth = 900;
        private const double minHight = 600;

        private const String APPNAME = "Forwarder Tools 1.0";

        public MainWindow()
        {
            InitializeComponent();
            Properties.Settings.Default.Maximized = false;
            Properties.Settings.Default.Minimized = false;
            Properties.Settings.Default.Save();
            Sources.Functions.MAINWINDOW = this;

            this.Height = SystemParameters.WorkArea.Height * 0.8;
            this.Width = SystemParameters.WorkArea.Width * 0.8;
            this.Top = SystemParameters.WorkArea.Top + SystemParameters.WorkArea.Height * 0.1;
            this.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width * 0.1;

            HideAll();
            InitComponent();
        }

        public void InitWindow()
        {
            switch (USERROLE)
            {
                case "Администратор":
                    usersPage.Visibility = Visibility.Visible;
                    UsersPage.Visibility = Visibility.Visible;

                    engineersPage.Visibility = Visibility.Visible;
                    EngineersPage.Visibility = Visibility.Visible;

                    forwardersPage.Visibility = Visibility.Visible;
                    ForwardersPage.Visibility = Visibility.Visible;

                    companiesPage.Visibility = Visibility.Visible;
                    CompaniesPage.Visibility = Visibility.Visible;

                    requestsPage.Visibility = Visibility.Visible;
                    RequestsPage.Visibility = Visibility.Visible;

                    Sources.Client.SendMessage("UpdateAllData", new String[] { });
                    
                    tcPages.SelectedItem = ForwardersPage;
                    break;
                case "Инженер":
                    break;
                case "Экспедитор":
                    break;
                case "Руководитель экспедиторов":
                    break;
            }
        }

        private void InitComponent()
        {
            cbUserRole.Items.Clear();
            cbUserRole.Items.Add("Инженер");
            cbUserRole.Items.Add("Экспедитор");
            cbUserRole.Items.Add("Руководитель экспедиторов");
            cbUserRole.Items.Add("Администратор");
        }

        #region Реализация скрытия всех элементов
        private void HideAll()
        {
            requestsPage.Visibility = Visibility.Hidden;
            routesPage.Visibility = Visibility.Hidden;
            companiesPage.Visibility = Visibility.Hidden;
            forwardersPage.Visibility = Visibility.Hidden;
            engineersPage.Visibility = Visibility.Hidden;
            usersPage.Visibility = Visibility.Hidden;
            serverPage.Visibility = Visibility.Hidden;
            RequestsPage.Visibility = Visibility.Hidden;
            RoutesPage.Visibility = Visibility.Hidden;
            CompaniesPage.Visibility = Visibility.Hidden;
            ForwardersPage.Visibility = Visibility.Hidden;
            EngineersPage.Visibility = Visibility.Hidden;
            UsersPage.Visibility = Visibility.Hidden;
            ServerPage.Visibility = Visibility.Hidden;
        }
        #endregion

        #region Реализация кнопок управления
        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void HeaderButton_MouseEnter(object sender, MouseEventArgs e)
        {
            close.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\close-hover.png", UriKind.Relative)));
            min.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\min-hover.png", UriKind.Relative)));
            max.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\max-hover.png", UriKind.Relative)));
        }

        private void HeaderButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.IsActive)
            {
                close.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\close-normal.png", UriKind.Relative)));
                min.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\min-normal.png", UriKind.Relative)));
                max.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\max-normal.png", UriKind.Relative)));
            }
            else
            {
                close.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\nofocus.png", UriKind.Relative)));
                min.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\nofocus.png", UriKind.Relative)));
                max.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\nofocus.png", UriKind.Relative)));
            }
        }

        private void Max_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(Properties.Settings.Default.Maximized)
            {
                SetWindowStage("Normal");
            }
            else
            {
                SetWindowStage("Maximized");
            }
            
        }

        private void Min_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void SetWindowStage(String _stage)
        {
            switch (_stage)
            {
                case "Maximized":
                    {
                        Properties.Settings.Default.Height = this.Height;
                        Properties.Settings.Default.Width = this.Width;
                        Properties.Settings.Default.Top = this.Top;
                        Properties.Settings.Default.Left = this.Left;

                        this.Height = SystemParameters.WorkArea.Height;
                        this.Width = SystemParameters.WorkArea.Width;
                        this.Top = SystemParameters.WorkArea.Top;
                        this.Left = SystemParameters.WorkArea.Left;
                        this.body.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);

                        Properties.Settings.Default.Maximized = true;
                        Properties.Settings.Default.Save();
                        break;
                    }

                case "Normal":
                    {
                        this.body.Margin = new Thickness(20.0, 20.0, 20.0, 20.0);
                        this.Height = Properties.Settings.Default.Height;
                        this.Width = Properties.Settings.Default.Width;
                        this.Top = Properties.Settings.Default.Top;
                        this.Left = Properties.Settings.Default.Left;

                        Properties.Settings.Default.Maximized = false;
                        Properties.Settings.Default.Save();

                        break;
                    }
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if(this.WindowState == WindowState.Maximized)
            {
                SetWindowStage("Maximized");
                this.WindowState = WindowState.Normal;
            }
        }
        #endregion

        #region Реализация перемещения окна
        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ClickCount == 2)
            {
                if (!Properties.Settings.Default.Maximized)
                {
                    SetWindowStage("Maximized");
                }
                else
                {
                    SetWindowStage("Normal");
                }
            }
            else if (!Properties.Settings.Default.Maximized) this.DragMove();
        }
        #endregion

        #region Реализация изменения размеров окна
        private void RightResize_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRightResize && !Properties.Settings.Default.Maximized)
            {
                double newWidth = this.Width + (e.GetPosition(this).X - positionRightResize);
                if (newWidth > minWidth + 40) {
                    this.Width = newWidth;
                    positionRightResize = e.GetPosition(this).X;
                }
            }
        }

        private void RightBottomResize_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRightBottomResize && !Properties.Settings.Default.Maximized)
            {
                double newWidth = this.Width + (e.GetPosition(this).X - positionXRightBottomResize);
                double newHeight = this.Height + (e.GetPosition(this).Y - positionYRightBottomResize);
                if (newWidth > minWidth + 40)
                {
                    this.Width = newWidth;
                    positionXRightBottomResize = e.GetPosition(this).X;
                }
                if (newHeight > minHight + 40)
                {
                    this.Height = newHeight;
                    positionYRightBottomResize = e.GetPosition(this).Y;
                }
            }
        }

        private void LeftTopResize_MouseMove(object sender, MouseEventArgs e)
        {
            if (isLeftTopResize && !Properties.Settings.Default.Maximized)
            {
                double newWidth = this.Width - (e.GetPosition(this).X - positionXLeftTopResize);
                double newHeight = this.Height - (e.GetPosition(this).Y - positionYLeftTopResize);
                double newLeft = this.Left + (e.GetPosition(this).X - positionXLeftTopResize);
                double newTop = this.Top + (e.GetPosition(this).Y - positionYLeftTopResize);
                if (newWidth > minWidth + 40)
                {
                    this.Width = newWidth;
                    this.Left = newLeft;
                }
                if (newHeight > minHight + 40)
                {
                    this.Height = newHeight;
                    this.Top = newTop;
                }
            }
        }

        private void LeftBottomResize_MouseMove(object sender, MouseEventArgs e)
        {
            if (isLeftBottomResize && !Properties.Settings.Default.Maximized)
            {
                double newWidth = this.Width - (e.GetPosition(this).X - positionXLeftBottomResize);
                double newHeight = this.Height + (e.GetPosition(this).Y - positionYLeftBottomResize);
                double newLeft = this.Left + (e.GetPosition(this).X - positionXLeftBottomResize);
                if (newWidth > minWidth + 40)
                {
                    this.Width = newWidth;
                    this.Left = newLeft;
                }
                if (newHeight > minHight + 40)
                {
                    this.Height = newHeight;
                    positionYLeftBottomResize = e.GetPosition(this).Y;
                }
            }
        }

        private void RightTopResize_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRightTopResize && !Properties.Settings.Default.Maximized)
            {
                double newWidth = this.Width + (e.GetPosition(this).X - positionXRightTopResize);
                double newHeight = this.Height - (e.GetPosition(this).Y - positionYRightTopResize);
                double newTop = this.Top + (e.GetPosition(this).Y - positionYRightTopResize);
                if (newWidth > minWidth + 40)
                {
                    this.Width = newWidth;
                    positionXRightTopResize = e.GetPosition(this).X;
                }
                if (newHeight > minHight + 40)
                {
                    this.Height = newHeight;
                    this.Top = newTop;
                }
            }
        }

        private void LeftResize_MouseMove(object sender, MouseEventArgs e)
        {
            if (isLeftResize && !Properties.Settings.Default.Maximized)
            {
                double newWidth = this.Width - (e.GetPosition(this).X - positionLeftResize);
                double newLeft = this.Left + (e.GetPosition(this).X - positionLeftResize);
                if (newWidth > minWidth + 40)
                {
                    this.Width = newWidth;
                    this.Left = newLeft;
                }
            }
        }

        private void BottomResize_MouseMove(object sender, MouseEventArgs e)
        {
            if (isBottomResize && !Properties.Settings.Default.Maximized)
            {
                double newHeight = this.Height + (e.GetPosition(this).Y - positionBottomResize);
                if (newHeight > minHight + 40)
                {
                    this.Height = newHeight;
                    positionBottomResize = e.GetPosition(this).Y;
                }
            }
        }

        private void TopResize_MouseMove(object sender, MouseEventArgs e)
        {
            if (isTopResize && !Properties.Settings.Default.Maximized)
            {
                double newHeight = this.Height - (e.GetPosition(this).Y - positionTopResize);
                double newTop = this.Top + (e.GetPosition(this).Y - positionTopResize);
                if (newHeight > minHight + 40)
                {
                    this.Height = newHeight;
                    this.Top = newTop;
                }
            }
        }

        private void Resize_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rect = (Rectangle)sender;
            rect.CaptureMouse();
            switch (rect.Name)
            {
                case "rightResize":
                    isRightResize = true;
                    positionRightResize = e.GetPosition(this).X;
                    break;
                case "leftResize":
                    isLeftResize = true;
                    positionLeftResize = e.GetPosition(this).X;
                    break;
                case "bottomResize":
                    isBottomResize = true;
                    positionBottomResize = e.GetPosition(this).Y;
                    break;
                case "topResize":
                    isTopResize = true;
                    positionTopResize = e.GetPosition(this).Y;
                    break;
                case "rightBottomResize":
                    isRightBottomResize = true;
                    positionXRightBottomResize = e.GetPosition(this).X;
                    positionYRightBottomResize = e.GetPosition(this).Y;
                    break;
                case "leftTopResize":
                    isLeftTopResize = true;
                    positionXLeftTopResize = e.GetPosition(this).X;
                    positionYLeftTopResize = e.GetPosition(this).Y;
                    break;
                case "rightTopResize":
                    isRightTopResize = true;
                    positionXRightTopResize = e.GetPosition(this).X;
                    positionYRightTopResize = e.GetPosition(this).Y;
                    break;
                case "leftBottomResize":
                    isLeftBottomResize = true;
                    positionXLeftBottomResize = e.GetPosition(this).X;
                    positionYLeftBottomResize = e.GetPosition(this).Y;
                    break;
            } 
        }

        private void Resize_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Rectangle rect = (Rectangle)sender;
            rect.ReleaseMouseCapture();
            switch (rect.Name)
            {
                case "rightResize":
                    isRightResize = false;
                    break;
                case "leftResize":
                    isLeftResize = false;
                    break;
                case "bottomResize":
                    isBottomResize = false;
                    break;
                case "topResize":
                    isTopResize = false;
                    break;
                case "rightBottomResize":
                    isRightBottomResize = false;
                    break;
                case "leftTopResize":
                    isLeftTopResize = false;
                    break;
                case "rightTopResize":
                    isRightTopResize = false;
                    break;
                case "leftBottomResize":
                    isLeftBottomResize = false;
                    break;
            }
        }
        #endregion

        #region Реализация активации и деактивации окна
        private void Window_Activated(object sender, EventArgs e)
        {
            close.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\close-normal.png", UriKind.Relative)));
            min.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\min-normal.png", UriKind.Relative)));
            max.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\max-normal.png", UriKind.Relative)));
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush()
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1)
            };
            linearGradientBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF454545"), 0.0));
            linearGradientBrush.GradientStops.Add(new GradientStop((Color)ColorConverter.ConvertFromString("#FF404040"), 1.0));
            header.Background = linearGradientBrush;
            header.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF3C3C3C"));
            background.Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FF3C3C3C"));
            mainTitle.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#CCFFFFFF"));
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            close.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\nofocus.png", UriKind.Relative)));
            min.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\nofocus.png", UriKind.Relative)));
            max.Fill = new ImageBrush(new BitmapImage(new Uri(@"Resources\nofocus.png", UriKind.Relative)));
            header.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF6F6F6"));
            header.BorderBrush = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFB0B0B0"));
            background.Stroke = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFB0B0B0"));
            mainTitle.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFB0B0B0"));
        }
        #endregion

        #region Реализация перехода между страницами
        private void TCPages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((TabItem)tcPages.SelectedItem).Name)
            {
                case "RequstsPage":
                    mainTitle.Content = "Заявки";
                    this.Title = APPNAME + " - Заявки";
                    requestsPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFC8C8C8"));
                    routesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    companiesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    forwardersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    engineersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    usersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    serverPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    break;
                case "RoutesPage":
                    mainTitle.Content = "Маршруты";
                    this.Title = APPNAME + " - Маршруты";
                    requestsPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    routesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFC8C8C8"));
                    companiesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    forwardersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    engineersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    usersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    serverPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    break;
                case "CompaniesPage":
                    mainTitle.Content = "Фирмы";
                    this.Title = APPNAME + " - Фирмы";
                    requestsPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    routesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    companiesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFC8C8C8"));
                    forwardersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    engineersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    usersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    serverPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    break;
                case "ForwardersPage":
                    mainTitle.Content = "Экспедиторы";
                    this.Title = APPNAME + " - Экспедиторы";
                    requestsPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    routesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    companiesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    forwardersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFC8C8C8"));
                    engineersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    usersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    serverPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    break;
                case "EngineersPage":
                    mainTitle.Content = "Инженера";
                    this.Title = APPNAME + " - Инженера";
                    requestsPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    routesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    companiesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    forwardersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    engineersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFC8C8C8"));
                    usersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    serverPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    break;
                case "UsersPage":
                    mainTitle.Content = "Пользователи";
                    this.Title = APPNAME + " - Пользователи";
                    requestsPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    routesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    companiesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    forwardersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    engineersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    usersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFC8C8C8"));
                    serverPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    break;
                case "ServerPage":
                    mainTitle.Content = "Сервер";
                    this.Title = APPNAME + " - Сервер";
                    requestsPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    routesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    companiesPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    forwardersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    engineersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    usersPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
                    serverPage.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFC8C8C8"));
                    break;
            }
        }

        private void SettingMessageBotton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            settingMessageBotton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFC8C8C8"));
            AdditionalWindows.Settings setting = new AdditionalWindows.Settings();
            setting.Show();
        }

        private void SettingMessageBotton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            settingMessageBotton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
        }

        private void ExitMessageBotton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            exitMessageBotton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFC8C8C8"));
            App.Current.Shutdown();
        }

        private void ExitMessageBotton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            exitMessageBotton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
        }

        private void AboutMessageBotton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            aboutMessageBotton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFC8C8C8"));
            AdditionalWindows.About about = new AdditionalWindows.About();
            about.Show();
        }

        private void AboutMessageBotton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            aboutMessageBotton.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#FFF0F0F0"));
        }

        private void RequstsPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.tcPages.SelectedItem = RequestsPage;
        }

        private void RoutesPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.tcPages.SelectedItem = RoutesPage;
        }

        private void CompaniesPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.tcPages.SelectedItem = CompaniesPage;
        }

        private void ForwardersPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.tcPages.SelectedItem = ForwardersPage;
        }

        private void EngineersPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.tcPages.SelectedItem = EngineersPage;
        }

        private void UsersPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.tcPages.SelectedItem = UsersPage;
        }

        private void ServerPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.tcPages.SelectedItem = ServerPage;
        }
        #endregion

        #region ПОЛЬЗОВАТЕЛИ Обновление данных на странице Пользователи
        public void UpdateUsersData(List<ClassResource.User> users)
        {
            USERS = users;
            lvUsers.Items.Clear();
            foreach (ClassResource.User user in USERS)
            {
                lvUsers.Items.Add(new UserListItem(user.Login, user.Name, user.Role, ((user.Snapping == "") ? "Не определена" : (user.Engineer != "" ? user.Engineer + " (Инженер)" : user.Forwarder + " (Экспедитор)"))));
            }
            if(lvUsers.Items.Count > 0)
            {
                lvUsers.SelectedIndex = 0;
            }
        }

        public void UpdateUsers(List<String> users)
        {
            cbUserSnappingInfo.Items.Clear();
            foreach (String user in users)
            {
                cbUserSnappingInfo.Items.Add(user);
            }
        }
        #endregion

        #region ПОЛЬЗОВАТЕЛИ Обработка выбора пользователя
        private void LVUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lvUsers.SelectedIndex >= 0)
            {
                SELECTUSER = USERS.Find(x => x.Login == ((UserListItem)lvUsers.SelectedItem).Login);
                bUserSaveChange.IsEnabled = false;
                bUserDelete.IsEnabled = true;
                tbUserLogin.Text = SELECTUSER.Login;
                tbUserName.Text = SELECTUSER.Name;
                tbUserSnapping.Text = SELECTUSER.Snapping;
                cbUserRole.SelectedItem = SELECTUSER.Role;
                cbUserSnappingInfo.SelectedItem = (SELECTUSER.Snapping == "") ? "Не определена" : (SELECTUSER.Engineer != "" ? SELECTUSER.Engineer + " (Инженер)" : SELECTUSER.Forwarder + " (Экспедитор)");
            }
            else
            {
                tbUserLogin.Text = "";
                tbUserName.Text = "";
                tbUserSnapping.Text = "";
                cbUserRole.SelectedIndex = -1;
                cbUserSnappingInfo.SelectedIndex = -1;
                bUserSaveChange.IsEnabled = false;
                bUserDelete.IsEnabled = false;
            }
        }
        #endregion


        
        class UserListItem
        {
            public String Login { get; set; }
            public String Name { get; set; }
            public String Role { get; set; }
            public String Snapping { get; set; }

            public UserListItem(String login, String name, String role, String snapping)
            {
                this.Login = login;
                this.Name = name;
                this.Role = role;
                this.Snapping = snapping;
            }
        }

        class EngineerListItem
        {
            public String ID { get; set; }
            public String Name { get; set; }
            public String СontactNumber { get; set; }

            public EngineerListItem(String id, String name, String contactNumber)
            {
                this.ID = id;
                this.Name = name;
                this.СontactNumber = contactNumber;
            }
        }


        class ForwarderListItem
        {
            public String ID { get; set; }
            public String Name { get; set; }
            public String СontactNumber { get; set; }

            public ForwarderListItem(String id, String name, String contactNumber)
            {
                this.ID = id;
                this.Name = name;
                this.СontactNumber = contactNumber;
            }
        }

        class CompanyListItem
        {
            public String ID { get; set; }
            public String Name { get; set; }
            public String Country { get; set; }
            public String City { get; set; }
            public String NameСontactPerson { get; set; }
            public String PhoneContactPerson { get; set; }

            public CompanyListItem(String id, String name, String country, String city, String nameСontactPerson, String phoneContactPerson)
            {
                this.ID = id;
                this.Name = name;
                this.Country = country;
                this.City = city;
                this.NameСontactPerson = nameСontactPerson;
                this.PhoneContactPerson = phoneContactPerson;
            }
        }


        class RequestListItem
        {
            public String ID { get; set; }
            public String Number { get; set; }
            public String ProductName { get; set; }
            public String ProductWeight { get; set; }
            public String ProductDimensions { get; set; }
            public String Quantity { get; set; }
            public String Company { get; set; }
            public String Note { get; set; }

            public RequestListItem(String id, String number, String productName, String productWeight, String productDimensions, String quantity, String company, String note)
            {
                this.ID = id;
                this.Number = number;
                this.ProductName = productName;
                this.ProductWeight = productWeight;
                this.ProductDimensions = productDimensions;
                this.Quantity = quantity;
                this.Company = company;
                this.Note = note;
            }
        
        }
    
        
        private bool IsInName(String name, String str)
        {
            foreach (String item in name.Split(' '))
            {
                if (item.IndexOf(str, StringComparison.OrdinalIgnoreCase) == 0) return true;
            }
            return false;
        }

        #region ПОЛЬЗОВАТЕЛИ Реализация поиска среди пользователей
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(tbUserSearch.Text != "")
            {
                lvUsers.Items.Clear();
                foreach (ClassResource.User user in USERS)
                {
                    if(user.Login.IndexOf(tbUserSearch.Text, StringComparison.OrdinalIgnoreCase) == 0 || IsInName(user.Name, tbUserSearch.Text) || user.Role.IndexOf(tbUserSearch.Text, StringComparison.OrdinalIgnoreCase) == 0)
                    lvUsers.Items.Add(new UserListItem(user.Login, user.Name, user.Role, ((user.Snapping == "") ? "Не определена" : (user.Engineer != "" ? user.Engineer + " (Инженер)" : user.Forwarder + " (Экспедитор)"))));
                }
                if (lvUsers.Items.Count > 0)
                {
                    lvUsers.SelectedIndex = 0;
                }
            }
            else
            {
                lvUsers.Items.Clear();
                foreach (ClassResource.User user in USERS)
                {
                    lvUsers.Items.Add(new UserListItem(user.Login, user.Name, user.Role, ((user.Snapping == "") ? "Не определена" : (user.Engineer != "" ? user.Engineer + " (Инженер)" : user.Forwarder + " (Экспедитор)"))));
                }
                if (lvUsers.Items.Count > 0)
                {
                    lvUsers.SelectedIndex = 0;
                }
            }
        }
        #endregion

        #region ИНЖЕНЕРА Обновление данных на странице Инженера
        public void UpdateEngineersData(List<ClassResource.Engineer> engineers)
        {
            ENGINEERS = engineers;
            lvEngineers.Items.Clear();
            foreach (ClassResource.Engineer engineer in ENGINEERS)
            {
                lvEngineers.Items.Add(new EngineerListItem(engineer.ID, engineer.Name, engineer.ContactNumber));
            }
            if (lvEngineers.Items.Count > 0)
            {
                lvEngineers.SelectedIndex = 0;
            }
        }
        #endregion

        #region ИНЖЕНЕРА Обработка выбора инженера
        private void LVEngineers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvEngineers.SelectedIndex >= 0)
            {
                SELECTENGINEER = ENGINEERS.Find(x => x.ID == ((EngineerListItem)lvEngineers.SelectedItem).ID);
                bEngineerDelete.IsEnabled = true;
                bEngineerCopyID.IsEnabled = true;
                bEngineerSaveChange.IsEnabled = false;
                tbEngineerName.Text = SELECTENGINEER.Name;
                tbEngineerPhone.Text = SELECTENGINEER.ContactNumber;
                tbEngineerNote.Text = SELECTENGINEER.Note;
            }
            else
            {
                bEngineerDelete.IsEnabled = false;
                bEngineerCopyID.IsEnabled = false;
                bEngineerSaveChange.IsEnabled = false;
                tbEngineerName.Text = "";
                tbEngineerPhone.Text = "";
                tbEngineerNote.Text = "";
            }
        }
        #endregion

        #region ИНЖЕНЕРА Реализация поиска среди инженеров
        private void TBEngineerSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbEngineerSearch.Text != "")
            {
                lvEngineers.Items.Clear();
                foreach (ClassResource.Engineer engineer in ENGINEERS)
                {
                    if (engineer.ID.IndexOf(tbEngineerSearch.Text, StringComparison.OrdinalIgnoreCase) == 0 || IsInName(engineer.Name, tbEngineerSearch.Text) || engineer.ContactNumber.Contains(tbEngineerSearch.Text))
                        lvEngineers.Items.Add(new EngineerListItem(engineer.ID, engineer.Name, engineer.ContactNumber));
                }
                if (lvEngineers.Items.Count > 0)
                {
                    lvEngineers.SelectedIndex = 0;
                }
            }
            else
            {
                lvEngineers.Items.Clear();
                foreach (ClassResource.Engineer engineer in ENGINEERS)
                {
                    lvEngineers.Items.Add(new EngineerListItem(engineer.ID, engineer.Name, engineer.ContactNumber));
                }
                if (lvEngineers.Items.Count > 0)
                {
                    lvEngineers.SelectedIndex = 0;
                }
            }
        }
        #endregion

        #region ЭКСПЕДИТОРЫ Обновление данных на странице Экспедиторы
        public void UpdateForwardersData(List<ClassResource.Forwarder> forwarders)
        {
            FORWARDERS = forwarders;
            lvForwarders.Items.Clear();
            foreach (ClassResource.Forwarder forwarder in FORWARDERS)
            {
                lvForwarders.Items.Add(new ForwarderListItem(forwarder.ID, forwarder.Name, forwarder.ContactNumber));
            }
            if (lvForwarders.Items.Count > 0)
            {
                lvForwarders.SelectedIndex = 0;
            }
        }
        #endregion

        #region ЭКСПЕДИТОРЫ Обработка выбора экспедитора
        private void LVForwarders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(lvForwarders.SelectedIndex >= 0)
            {
                SELECTFORWARDER = FORWARDERS.Find(x => x.ID == ((ForwarderListItem)lvForwarders.SelectedItem).ID);
                bForwarderDelete.IsEnabled = true;
                bForwarderCopyID.IsEnabled = true;
                bForwarderSaveChange.IsEnabled = false;
                tbForwarderName.Text = SELECTFORWARDER.Name;
                tbForwarderPhone.Text = SELECTFORWARDER.ContactNumber;
                tbForwarderNote.Text = SELECTFORWARDER.Note;
            }
            else
            {
                bForwarderDelete.IsEnabled = false;
                bForwarderCopyID.IsEnabled = false;
                bForwarderSaveChange.IsEnabled = false;
                tbForwarderName.Text = "";
                tbForwarderPhone.Text = "";
                tbForwarderNote.Text = "";
            }
        }
        #endregion

        #region ЭКСПЕДИТОРЫ Реализация поиска среди экспедиторов
        private void TBForwarderSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(tbForwarderSearch.Text != "")
            {
                lvForwarders.Items.Clear();
                foreach (ClassResource.Forwarder forwarder in FORWARDERS)
                {
                    if(forwarder.ID.IndexOf(tbForwarderSearch.Text, StringComparison.OrdinalIgnoreCase) == 0 || IsInName(forwarder.Name, tbForwarderSearch.Text) || forwarder.ContactNumber.Contains(tbForwarderSearch.Text))
                    lvForwarders.Items.Add(new ForwarderListItem(forwarder.ID, forwarder.Name, forwarder.ContactNumber));
                }
                if (lvForwarders.Items.Count > 0)
                {
                    lvForwarders.SelectedIndex = 0;
                }
            }
            else
            {
                lvForwarders.Items.Clear();
                foreach (ClassResource.Forwarder forwarder in FORWARDERS)
                {
                    lvForwarders.Items.Add(new ForwarderListItem(forwarder.ID, forwarder.Name, forwarder.ContactNumber));
                }
                if (lvForwarders.Items.Count > 0)
                {
                    lvForwarders.SelectedIndex = 0;
                }
            }
        }
        #endregion

        #region ФИРМЫ Обновление данных на странице Фирмы
        public void UpdateCompaniesData(List<ClassResource.Company> companies)
        {
            COMPANIES = companies;
            lvCompanies.Items.Clear();
            foreach (ClassResource.Company company in COMPANIES)
            {
                lvCompanies.Items.Add(new CompanyListItem(company.ID, company.Name, company.Country, company.City, company.NameСontactPerson, company.PhoneContactPerson));
            }
            if (lvCompanies.Items.Count > 0)
            {
                lvCompanies.SelectedIndex = 0;
            }
        }
        #endregion

        #region ФИРМЫ Обработка выбора фирмы
        private void LVCompanies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvCompanies.SelectedIndex >= 0)
            {
                SELECTCOMPANY = COMPANIES.Find(x => x.ID == ((CompanyListItem)lvCompanies.SelectedItem).ID);
                bCompanyDelete.IsEnabled = true;
                bCompanyMap.IsEnabled = true;
                bCompanySaveChange.IsEnabled = false;
                tbCompanyName.Text = SELECTCOMPANY.Name;
                tbCompanyCountry.Text = SELECTCOMPANY.Country;
                tbCompanyCity.Text = SELECTCOMPANY.City;
                tbCompanyAddress.Text = SELECTCOMPANY.Address;
                tbCompanyContactName.Text = SELECTCOMPANY.NameСontactPerson;
                tbCompanyContactPhone.Text = SELECTCOMPANY.PhoneContactPerson;
            }
            else
            {
                bCompanyDelete.IsEnabled = false;
                bCompanyMap.IsEnabled = false;
                bCompanySaveChange.IsEnabled = false;
                tbCompanyName.Text = "";
                tbCompanyCountry.Text = "";
                tbCompanyCity.Text = "";
                tbCompanyAddress.Text = "";
                tbCompanyContactName.Text = "";
                tbCompanyContactPhone.Text = "";
            }
        }
        #endregion

        #region ФИРМЫ Реализация поиска среди фирм
        private void TBCompanySearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbCompanySearch.Text != "")
            {
                lvCompanies.Items.Clear();
                foreach (ClassResource.Company company in COMPANIES)
                {
                    if (company.Name.IndexOf(tbCompanySearch.Text, StringComparison.OrdinalIgnoreCase) == 0 || company.Country.IndexOf(tbCompanySearch.Text, StringComparison.OrdinalIgnoreCase) == 0 || company.City.IndexOf(tbCompanySearch.Text, StringComparison.OrdinalIgnoreCase) == 0 || company.PhoneContactPerson.Contains(tbCompanySearch.Text) || IsInName(company.NameСontactPerson, tbCompanySearch.Text))
                        lvCompanies.Items.Add(new CompanyListItem(company.ID, company.Name, company.Country, company.City, company.NameСontactPerson, company.PhoneContactPerson));
                }
                if (lvCompanies.Items.Count > 0)
                {
                    lvCompanies.SelectedIndex = 0;
                }
            }
            else
            {
                lvCompanies.Items.Clear();
                foreach (ClassResource.Company company in COMPANIES)
                {
                    lvCompanies.Items.Add(new CompanyListItem(company.ID, company.Name, company.Country, company.City, company.NameСontactPerson, company.PhoneContactPerson));
                }
                if (lvCompanies.Items.Count > 0)
                {
                    lvCompanies.SelectedIndex = 0;
                }
            }
        }
        #endregion

        #region ЗАЯВКИ Обновление данных на странице Заявки
        public void UpdateRequestsData(List<ClassResource.Request> requests)
        {
            REQUESTS = requests;
            lvRequests.Items.Clear();
            foreach (ClassResource.Request request in REQUESTS)
            {
                lvRequests.Items.Add(new RequestListItem(request.ID, request.Number, request.ProductName, request.ProductWeight, request.ProductDimensions, request.Quantity, COMPANIES.Find(x => x.ID == request.IDCompany).Name, request.Note));
            }
            if (lvRequests.Items.Count > 0)
            {
                lvRequests.SelectedIndex = 0;
            }
        }
        #endregion

        #region ЗАЯВКИ Обработка выбора заявки
        private void LVRequests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvRequests.SelectedIndex >= 0)
            {
                SELECTREQUEST = REQUESTS.Find(x => x.ID == ((RequestListItem)lvRequests.SelectedItem).ID);
                bRequestDelete.IsEnabled = true;
                bRequestDistribute.IsEnabled = true;
                bRequestOpen.IsEnabled = true;
                bRequestSaveChange.IsEnabled = false;
                tbRequestNumber.Text = SELECTREQUEST.Number;
                tbRequestProductName.Text = SELECTREQUEST.ProductName;
                tbRequestWeight.Text = SELECTREQUEST.ProductWeight;
                tbRequestDimensions.Text = SELECTREQUEST.ProductDimensions;
                tbRequestQuantity.Text = SELECTREQUEST.Quantity;
                tbRequestCompany.Text = COMPANIES.Find(x => x.ID == SELECTREQUEST.IDCompany).Name;
            }
            else
            {
                bRequestDelete.IsEnabled = false;
                bRequestDistribute.IsEnabled = false;
                bRequestOpen.IsEnabled = false;
                bRequestSaveChange.IsEnabled = false;
                tbRequestNumber.Text = "";
                tbRequestProductName.Text = "";
                tbRequestWeight.Text = "";
                tbRequestDimensions.Text = "";
                tbRequestQuantity.Text = "";
                tbRequestCompany.Text = "";
            }
        }
        #endregion

        #region ЗАЯВКИ Реализация поиска среди заявок
        private void TBRequestSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbRequestSearch.Text != "")
            {
                lvRequests.Items.Clear();
                foreach (ClassResource.Request request in REQUESTS)
                {
                    if (request.Number.IndexOf(tbRequestSearch.Text, StringComparison.OrdinalIgnoreCase) == 0 || request.ProductName.Contains(tbRequestSearch.Text) || COMPANIES.Find(x => x.ID == SELECTREQUEST.IDCompany).Name.Contains(tbRequestSearch.Text))
                        lvRequests.Items.Add(new RequestListItem(request.ID, request.Number, request.ProductName, request.ProductWeight, request.ProductDimensions, request.Quantity, COMPANIES.Find(x => x.ID == request.IDCompany).Name, request.Note));
                }
                if (lvRequests.Items.Count > 0)
                {
                    lvRequests.SelectedIndex = 0;
                }
            }
            else
            {
                lvRequests.Items.Clear();
                foreach (ClassResource.Request request in REQUESTS)
                {
                    lvRequests.Items.Add(new RequestListItem(request.ID, request.Number, request.ProductName, request.ProductWeight, request.ProductDimensions, request.Quantity, COMPANIES.Find(x => x.ID == request.IDCompany).Name, request.Note));
                }
                if (lvRequests.Items.Count > 0)
                {
                    lvRequests.SelectedIndex = 0;
                }
            }
        }
        #endregion

        private void BRequestAdd_Click(object sender, RoutedEventArgs e)
        {
            AdditionalWindows.Request request = new AdditionalWindows.Request(COMPANIES, USERNAME, "+375 (23) 544-56-56");
            request.Show();
        }

        private void BRequestOpen_Click(object sender, RoutedEventArgs e)
        {
            AdditionalWindows.Request request = new AdditionalWindows.Request(COMPANIES, SELECTREQUEST, ENGINEERS.Find(x => x.ID == SELECTREQUEST.IDEngineer).Name, ENGINEERS.Find(x => x.ID == SELECTREQUEST.IDEngineer).ContactNumber);
            request.Show();
        }

        private void BCompanyMap_Click(object sender, RoutedEventArgs e)
        {
            AdditionalWindows.Map map = new AdditionalWindows.Map(SELECTCOMPANY.Address);
            map.Show();
        }
    }
}
