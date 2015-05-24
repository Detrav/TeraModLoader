using Detrav.TeraApi;
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
            Logger.debug("{0}", "InitializeComponent");
        }

        Capture capture;
        DispatcherTimer timer;
        TeraModManager teraModManager;
        Dictionary<Connection, TeraClient> teraClients = new Dictionary<Connection,TeraClient>();

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
            Logger.debug("{0}", "Window started");
        }

        void capture_onPacketArrivalSync(object sender, PacketArrivalEventArgs e)
        {
            TeraClient client;
            if(teraClients.TryGetValue(e.connection,out client))
            {
                client.PacketArrival(e.packet);
            }

        }

        void capture_onEndConnectionSync(object sender, ConnectionEventArgs e)
        {
            
            TeraClient teraClient;
            if (teraClients.TryGetValue(e.connection, out teraClient))
            {
                teraClient.unLoad();
                teraClients.Remove(e.connection);
                TabItem ti = null;
                foreach (TabItem temp in tabControl.Items)
                {
                    if(e.connection.Equals(temp.Header))
                    {
                        ti = temp;
                        break;
                    }
                }
                if (ti != null)
                {
                    tabControl.Items.Remove(ti);
                    Logger.debug("Tab {0} in tab control removed", e.connection);
                }
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
            item.Header = e.connection;
            var sp = new StackPanel();
            foreach (Button plugin in buttons)
            {
                sp.Children.Add(plugin);
            }
            item.Content = sp;
            tabControl.Items.Add(item);
            Logger.debug("Tab {0} in tab control added", e.connection);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            capture.doEventSync();
            foreach (var pair in teraClients)
                pair.Value.doEvents();
            timer.Start();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            teraModManager.saveConfig();
            MessageBox.Show("Saved!", "SaveWindow");
        }
    }
}
