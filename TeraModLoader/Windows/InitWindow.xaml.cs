using Detrav.TeraApi;
using Detrav.TeraApi.OpCodes;
using Detrav.TeraModLoader.Core;
using Detrav.TeraModLoader.Core.Data;
using Detrav.TeraModLoader.Sniffer;
using SharpPcap;
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

namespace Detrav.TeraModLoader.Windows
{
    /// <summary>
    /// Логика взаимодействия для InitWindow.xaml
    /// </summary>
    public partial class InitWindow : Window
    {
        internal ICapture device { get; private set; }
        MyConfig config;
        public InitWindow()
        {
            InitializeComponent();
            Logger.debug("new InitWindow");
            ConfigManager cm = new ConfigManager();
            config = cm.loadGlobal(typeof(MyConfig)) as MyConfig;
            if (config == null) config = new MyConfig();
            comboBoxDriver.SelectedIndex = config.driverType;
            foreach (var el in config.servers)
                listBoxServers.Items.Add(el.name);
            listBoxServers.SelectedIndex = config.serverIndex;
            foreach (var el in PacketCreator.getVerions())
                listBoxVersion.Items.Add(new ComboBoxEnumWithDescription(el));

            listBoxVersion.SelectedItem = config.version;
            Logger.debug("end InitWindow");
        }

        private void buttonCansel_Click(object sender, RoutedEventArgs e)
        {
            Logger.debug("canseled InitWindow");
            DialogResult = false;
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            if(comboBoxDriver.SelectedIndex<1)
            {
                Logger.debug("Нужно выбрать драйвер!");
                System.Windows.MessageBox.Show("Нужно выбрать драйвер!");
                return;
            }
            if (listBoxDevices.SelectedIndex < 0)
            {
                Logger.debug("Нужно выбрать одно из устройств!");
                System.Windows.MessageBox.Show("Нужно выбрать одно из устройств!");
                return;
            }
            if (listBoxServers.SelectedIndex < 0)
            {
                Logger.debug("Нужно выбрать один из серверов!");
                System.Windows.MessageBox.Show("Нужно выбрать один из серверов!");
                return;
            }
            if(!(listBoxVersion.SelectedItem is ComboBoxEnumWithDescription))
            {
                Logger.debug("Нужно выбрать версию!");
                System.Windows.MessageBox.Show("Нужно выбрать версию!");
                return;
            }
            string server = config.servers[listBoxServers.SelectedIndex].ip;
            config.driverType = comboBoxDriver.SelectedIndex;
            switch(comboBoxDriver.SelectedIndex)
            {
                case 1:
                    device = new CapturePcap(CaptureDeviceList.Instance[listBoxDevices.SelectedIndex], server);
                    if (tcpFilter != null) tcpFilter.Dispose();
                    break;
                case 2:
                    device = new CaptureWinpkFilter(tcpFilter, listBoxDevices.SelectedIndex, server);
                    break;
            }
            config.deviceIndex = listBoxDevices.SelectedIndex;
            config.serverIndex = listBoxServers.SelectedIndex;
            config.version = (OpCodeVersion)(listBoxVersion.SelectedItem as ComboBoxEnumWithDescription).e;
            PacketCreator.setVersion(config.version);
            ConfigManager cm = new ConfigManager();
            cm.saveGlobal(config);
            Logger.debug("Ok InitWindow");
            DialogResult = true;
        }
        Detrav.WinpkFilterWrapper.TcpFilter tcpFilter;

        private void comboBoxDriver_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listBoxDevices.Items.Clear();
            switch (comboBoxDriver.SelectedIndex)
            {
                case 1:
                    try
                    {
                        foreach (var el in CaptureDeviceList.Instance)
                            listBoxDevices.Items.Add(el.Description);
                        listBoxDevices.SelectedIndex = config.deviceIndex;
                        if (listBoxDevices.Items.Count == 0)
                        {
                            MessageBox.Show("У вас не установлен драйвер pcap, или нету соединений!");
                            comboBoxDriver.SelectedIndex = 0;
                        }
                    }
                    catch { MessageBox.Show("У вас не установлен драйвер pcap, или нету соединений!"); comboBoxDriver.SelectedIndex = 0; }
                    break;
                case 2:
                    try
                    {
                        if (tcpFilter == null) tcpFilter = Detrav.WinpkFilterWrapper.TcpFilter.create();
                        foreach (var el in tcpFilter.deviceList)
                            listBoxDevices.Items.Add(el);
                        listBoxDevices.SelectedIndex = config.deviceIndex;
                        if (listBoxDevices.Items.Count == 0)
                        {
                            MessageBox.Show("У вас не установлен драйвер WinpkFilter, или нету соединений!"); comboBoxDriver.SelectedIndex = 0;
                        }
                    }
                    catch { MessageBox.Show("У вас не установлен драйвер WinpkFilter, или нету соединений!"); comboBoxDriver.SelectedIndex = 0; }
                    break;
            }
        }
    }
}