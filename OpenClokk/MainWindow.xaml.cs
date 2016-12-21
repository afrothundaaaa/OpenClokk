﻿using System;
using System.Windows;
using System.Windows.Threading;
using System.Windows.Input;

namespace OpenClokk
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Load User Settings
            this.Loaded += MainWindow_Loaded;

            // Start Timer
            DispatcherTimer clock = new DispatcherTimer();

            clock.Tick += clock_Tick;
            clock.Interval = new TimeSpan(0, 0, 0, 0, 10);
            clock.Start();

            // Save User settings
            this.Closing += MainWindow_Closing;
        }

        // Tick Event Handler
        private void clock_Tick(object sender, EventArgs e)
        {
            Time_Textblock.Text = DateTime.Now.ToString("hh:mm:ss");
            AMPM_Textblock.Text = DateTime.Now.ToString("tt");
            Date_Textblock.Text = DateTime.Now.ToString("dddd dd MMMM yyyy");
        }

        // Enable Drag on Left Click
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        // Snap to Corners on Left Double Click
        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            var workingArea = System.Windows.SystemParameters.WorkArea;

            if (this.Left == 0)
            {
                this.Left = workingArea.Right - this.Width;
                this.Top = workingArea.Bottom - this.Height;
            }
            else
            {
                this.Left = 0;
                this.Top = workingArea.Bottom - this.Height;
            }

        }

        // Retrieve window settings from Window Load Event
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var workingArea = System.Windows.SystemParameters.WorkArea;
            
            // Set Window Location
            if (Properties.Settings.Default.WindowLocation_Top != 0
                && Properties.Settings.Default.WindowLocation_Left != 0
                )
            {
                this.Left = Properties.Settings.Default.WindowLocation_Left;
                this.Top = Properties.Settings.Default.WindowLocation_Top;
            }
           else if (Properties.Settings.Default.WindowLocation_Top == 0
                && Properties.Settings.Default.WindowLocation_Left == 0)
            {
                this.Left = workingArea.Right - this.Width;
                this.Top = workingArea.Bottom - this.Height;
            }

            if (this.Left >= workingArea.Right
                && this.Top >= workingArea.Bottom)
            {
                // Define Working Area
                this.Left = workingArea.Right - this.Width;
                this.Top = workingArea.Bottom - this.Height;
            }
        }

        // Window Closing Event
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Copy Window location to app settings
            Properties.Settings.Default.WindowLocation_Left = this.Left;
            Properties.Settings.Default.WindowLocation_Top = this.Top;

            // Save Settings
            Properties.Settings.Default.Save();
        }
    }
}