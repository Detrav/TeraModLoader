using Detrav.TeraApi.Interfaces;
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
        Dictionary<Connection, ITeraClient> teraClients = new Dictionary<Connection,ITeraClient>();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Запист пакетов
            capture = new Capture(SharpPcap.CaptureDeviceList.Instance[1],"91.225.237.8");
            capture.onNewConnectionSync += capture_onNewConnectionSync;
            capture.onEndConnectionSync += capture_onEndConnectionSync;
            capture.onPacketArrivalSync += capture_onPacketArrivalSync;
            //Работа с аддонами
            teraModManager = new TeraModManager();
            teraModManager.loadConfig();
            foreach(var mod in teraModManager.getModsCheckBox())
            {
                modsStackPanel.Children.Add(mod);
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
            ITeraClient teraClient;
            if(teraClients.TryGetValue(e.connection,out teraClient))
            {
                teraClient.unLoad();
                teraClients.Remove(e.connection);
            }
        }

        void capture_onNewConnectionSync(object sender, ConnectionEventArgs e)
        {
            TeraClient teraClient= new TeraClient();
            ITeraMod[] mods; Button[] buttons;
            teraModManager.initializeMods(out mods,out buttons);
            teraClient.load(mods);
            teraClients.Add(e.connection, teraClient);
            TabItem item = new TabItem();
            item.Header = e.connection.ToString();
            var sp = new StackPanel();
            foreach (Button plugin in buttons)
            {
                sp.Children.Add(plugin);
            }
            item.Content = sp;
            tabControl.Items.Add(item);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            capture.doEventSync();
            timer.Start();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            teraModManager.saveConfig();
            MessageBox.Show("Saved!", "SaveWindow");
        }
    }
}
