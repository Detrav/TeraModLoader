using Detrav.TeraApi;
using Detrav.TeraApi.Interfaces;
using Detrav.TeraModLoader.Core;
using Detrav.TeraModLoader.Core.Data;
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
        MyConfig myConfig = new MyConfig();
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
            InitWindow window = new InitWindow();
            if(window.ShowDialog() !=true)
            {
                Close();
                return;
            }
            //Запист пакетов
            capture = new Capture(window.device,window.server);
            capture.onNewConnectionSync += capture_onNewConnectionSync;
            capture.onEndConnectionSync += capture_onEndConnectionSync;
            capture.onPacketArrivalSync += capture_onPacketArrivalSync;
            //Работа с аддонами
            teraModManager = new TeraModManager();
            myConfig = teraModManager.loadConfig(myConfig);
            foreach(var mod in teraModManager.getModsCheckBox())
            {
                modsStackPanel.Children.Add(mod);
            }

            //Обработка данных
            timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Start();
            Logger.debug("{0}", "Window started");
            /*/TestVars
            capture_onNewConnectionSync(this, new ConnectionEventArgs(new Connection("0.0.0.0",0,"0.0.0.0",0)));
            Random r = new Random();
            for(int i =0;i<1000;i++)
            {
                byte[] bb = new byte[100];
                r.NextBytes(bb);
                capture_onPacketArrivalSync(this, new PacketArrivalEventArgs(new Connection("0.0.0.0", 0, "0.0.0.0", 0), new TeraPacketWithData(bb, PacketType.Any)));
            }*/
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
            teraModManager.saveConfig(myConfig);
            MessageBox.Show("Saved!", "SaveWindow");
        }

        private void menuItemTest_Click(object sender, RoutedEventArgs e)
        {
            capture_onNewConnectionSync(this, new ConnectionEventArgs(new Connection("0.0.0.0", 0, "0.0.0.0", 0)));
            Random r = new Random();
            for (int i = 0; i < 1000; i++)
            {
                byte[] bb = new byte[100];
                r.NextBytes(bb);
                capture_onPacketArrivalSync(this, new PacketArrivalEventArgs(new Connection("0.0.0.0", 0, "0.0.0.0", 0), new TeraPacketWithData(bb, PacketType.Any)));
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            capture.Dispose();
            foreach(var tc in teraClients)
            {
                tc.Value.unLoad();
            }
        }
    }
}
