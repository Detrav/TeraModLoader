using Detrav.TeraModLoader.Core;
using Detrav.TeraModLoader.Sniffer;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Detrav.TeraModLoader.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Capture capture;
        DispatcherTimer timer;
        TeraModManager teraModManager;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Запист пакетов
            capture = new Capture(SharpPcap.CaptureDeviceList.Instance[1],"91.225.237.8");
            capture.onNewConnectionSync += capture_onNewConnectionSync;
            capture.onEndConnectionSync += capture_onEndConnectionSync;
            capture.onPacketArrivalSync += capture_onPacketArrivalSync;
            //Работа с аддонами
            teraModManager = new TeraModManager();
            
            foreach(var mod in teraModManager.mods)
            {
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                Image image = new Image();
                image.Source = mod.icon;
                sp.Children.Add(image);
                CheckBox checkBox = new CheckBox();
                checkBox.IsChecked = mod.enable;
                checkBox.Content = String.Format("{0} {1}", mod.name, mod.version);
                sp.Children.Add(checkBox);
                modsStackPanel.Children.Add(sp);
            }

            //Обработка данных
            timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Start();
        }

        void capture_onPacketArrivalSync(object sender, PacketArrivalEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void capture_onEndConnectionSync(object sender, ConnectionEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void capture_onNewConnectionSync(object sender, ConnectionEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            capture.doEventSync();
            timer.Start();
        }
    }
}
