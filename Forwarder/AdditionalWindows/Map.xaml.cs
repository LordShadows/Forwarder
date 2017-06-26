﻿using System;
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

namespace Forwarder.AdditionalWindows
{
    /// <summary>
    /// Логика взаимодействия для Map.xaml
    /// </summary>
    public partial class Map : Window
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

        private const double minWidth = 900;
        private const double minHight = 600;

        private double WIDTH = 0;
        private double HEIGHT = 0;
        private double TOP = 0;
        private double LEFT = 0;
        private bool Maximized = false;

        public Map()
        {
            InitializeComponent();
            this.Height = SystemParameters.WorkArea.Height * 0.75;
            this.Width = SystemParameters.WorkArea.Width * 0.75;
            this.Top = SystemParameters.WorkArea.Top + SystemParameters.WorkArea.Height * 0.125;
            this.Left = SystemParameters.WorkArea.Left + SystemParameters.WorkArea.Width * 0.125;
        }

        #region Реализация кнопок управления
        private void Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
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
            if (Maximized)
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
                        HEIGHT = this.Height;
                        WIDTH = this.Width;
                        TOP = this.Top;
                        LEFT = this.Left;

                        this.Height = SystemParameters.WorkArea.Height;
                        this.Width = SystemParameters.WorkArea.Width;
                        this.Top = SystemParameters.WorkArea.Top;
                        this.Left = SystemParameters.WorkArea.Left;
                        this.body.Margin = new Thickness(0.0, 0.0, 0.0, 0.0);

                        Maximized = true;
                        break;
                    }

                case "Normal":
                    {
                        this.body.Margin = new Thickness(20.0, 20.0, 20.0, 20.0);
                        this.Height = HEIGHT;
                        this.Width = WIDTH;
                        this.Top = TOP;
                        this.Left = LEFT;

                        Maximized = false;

                        break;
                    }
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                SetWindowStage("Maximized");
                this.WindowState = WindowState.Normal;
            }
        }
        #endregion

        #region Реализация перемещения окна
        private void Header_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (!Maximized)
                {
                    SetWindowStage("Maximized");
                }
                else
                {
                    SetWindowStage("Normal");
                }
            }
            else if (!Maximized) this.DragMove();
        }
        #endregion

        #region Реализация изменения размеров окна
        private void RightResize_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRightResize && !Maximized)
            {
                double newWidth = this.Width + (e.GetPosition(this).X - positionRightResize);
                if (newWidth > minWidth + 40)
                {
                    this.Width = newWidth;
                    positionRightResize = e.GetPosition(this).X;
                }
            }
        }

        private void RightBottomResize_MouseMove(object sender, MouseEventArgs e)
        {
            if (isRightBottomResize && !Maximized)
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
            if (isLeftTopResize && !Maximized)
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
            if (isLeftBottomResize && !Maximized)
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
            if (isRightTopResize && !Maximized)
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
            if (isLeftResize && !Maximized)
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
            if (isBottomResize && !Maximized)
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
            if (isTopResize && !Maximized)
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
    }
}