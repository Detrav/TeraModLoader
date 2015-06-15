using Detrav.TeraApi.Interfaces;
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
    /// Логика взаимодействия для EntityWindow.xaml
    /// </summary>
    public partial class EntityWindow : Window
    {
        ITeraClient client;
        public EntityWindow(ITeraClient client)
        {
            this.client = client;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.Columns.Clear();
            switch (comboBoxType.SelectedIndex)
            {
                case 0:
                    dataGrid.ItemsSource = null;
                    dataGrid.ItemsSource = client.getEntities();
                    break;
                case 1:
                    dataGrid.ItemsSource = null;
                    dataGrid.ItemsSource = client.getNpcs();
                    break;
                case 2:
                    dataGrid.ItemsSource = null;
                    dataGrid.ItemsSource = client.getPlayers();
                    break;
                case 3:
                    dataGrid.ItemsSource = null;
                    dataGrid.ItemsSource = client.getParty();
                    break;
            }
            
        }
    }
}
