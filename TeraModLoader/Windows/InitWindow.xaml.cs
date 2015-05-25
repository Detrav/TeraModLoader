using Detrav.TeraApi.OpCodes;
using Detrav.TeraModLoader.Core;
using Detrav.TeraModLoader.Core.Data;
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
        public ICaptureDevice device { get; private set; }
        public string server { get; private set; }
        MyConfig config;
        public InitWindow()
        {
            InitializeComponent();
            ConfigManager cm = new ConfigManager();
            config = cm.loadGlobal(typeof(MyConfig)) as MyConfig;
            foreach (var el in CaptureDeviceList.Instance)
                listBoxDevices.Items.Add(el.Description);
            listBoxDevices.SelectedIndex = config.deviceIndex;
            foreach (var el in config.servers)
                listBoxServers.Items.Add(el.name);
            listBoxServers.SelectedIndex = config.serverIndex;
            foreach (var el in PacketCreator.getVerions())
                listBoxVersion.Items.Add(el);
            listBoxVersion.SelectedItem = config.version;
            
        }

        private void buttonCansel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void buttonOk_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxDevices.SelectedIndex < 0)
            {
                System.Windows.MessageBox.Show("Нужно выбрать одно из устройств!");
                return;
            }
            if (listBoxServers.SelectedIndex < 0)
            {
                System.Windows.MessageBox.Show("Нужно выбрать один из серверов!");
                return;
            }
            if (listBoxVersion.SelectedIndex < 0)
            {
                System.Windows.MessageBox.Show("Нужно выбрать версию!");
                return;
            }
            server = config.servers[listBoxServers.SelectedIndex].ip;
            device = CaptureDeviceList.Instance[listBoxDevices.SelectedIndex];
            config.deviceIndex = listBoxDevices.SelectedIndex;
            config.serverIndex = listBoxServers.SelectedIndex;
            config.version = (OpCodeVersion)listBoxVersion.SelectedItem;
            ConfigManager cm = new ConfigManager();
            cm.saveGlobal(config);
            DialogResult = true;
        }


    }
}
