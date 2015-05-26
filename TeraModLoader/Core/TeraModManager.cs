using Detrav.TeraApi;
using Detrav.TeraApi.Interfaces;
using Detrav.TeraModLoader.Core.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Detrav.TeraModLoader.Core
{
    class TeraModManager
    {
        public Data.Mod[] mods { get; private set; }
        ConfigManager config = new ConfigManager();
        static string directory = "mods";
        public TeraModManager()
        {
            Logger.debug("Start init for TeraModManager", "");
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            List<Data.Mod> ts = new List<Data.Mod>();
            foreach (var file in getFiles(directory, "*.exe|*.dll", SearchOption.TopDirectoryOnly))
            {
                Assembly a;
                try
                {
                    a = Assembly.LoadFrom(file);
                    Data.Mod m = new Data.Mod(a);
                    if (m.ready)
                    {
                        bool containts = false;
                        foreach(var mod in ts)
                        {
                            if(mod.name == m.name)
                            {
                                containts = true;
                                break;
                            }
                        }
                        if (!containts)
                        {
                            Logger.log("Loaded {0}", m);
                            ts.Add(m);
                        }
                    }
                }
                catch (Exception e) { Logger.debug("{0}", e); }
            }
            mods = ts.ToArray();
        }
        public MyConfig loadConfig(MyConfig cfg)
        {
            cfg = config.loadGlobal(cfg.GetType()) as MyConfig;
            foreach (var mod in mods)
            {
                bool enable;
                if (cfg.modEnable != null)
                    if (cfg.modEnable.TryGetValue(mod.name, out enable))
                    {
                        mod.enable = enable;
                        continue;
                    }
                mod.enable = true;
            }
            return cfg;
        }
        public void saveConfig(MyConfig cfg)
        {
            cfg.modEnable.Clear();
            foreach (var mod in mods)
            {
                cfg.modEnable.Add(mod.name.ToString(), mod.enable);
            }
            config.saveGlobal(cfg);
        }

        internal void initializeMods(out ITeraMod[] resultMods, out Button[] resultButtons)
        {
            Logger.log("Start initializeMods");
            List<ITeraMod> teraMods = new List<ITeraMod>();
            List<Button> buttons = new List<Button>();
            foreach(var mod in mods)
            {
                if (mod.enable)
                {
                    ITeraMod m = mod.create();
                    teraMods.Add(m);
                    Button b = new Button();
                    StackPanel sp = new StackPanel();
                    sp.Orientation = Orientation.Horizontal;
                    Image image = new Image();
                    image.Width = 32;
                    image.Height = 32;
                    image.Source = mod.icon;
                    sp.Children.Add(image);
                    Label label = new Label();
                    label.Content = new ButtonHidenValue() { iTeraMod = m, value = mod.ToString() };
                    sp.Children.Add(label);
                    b.Content = sp;
                    b.Click += b_Click;
                    buttons.Add(b);
                }
            }
            resultMods = teraMods.ToArray();
            resultButtons = buttons.ToArray();
        }

        void b_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ((((sender as Button).Content as StackPanel).Children[1] as Label).Content as ButtonHidenValue).iTeraMod.changeVisible();
        }

        class ButtonHidenValue
        {
            internal ITeraMod iTeraMod;
            internal string value;
            public override string ToString()
            {
                return value;
            }
        }

        internal StackPanel[] getModsCheckBox()
        {
            Logger.log("Start getModsCheckBox");
            List<StackPanel> modsStackPanel = new List<StackPanel>();
            foreach(var mod in mods)
            {
                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                Image image = new Image();
                image.Width = 32;
                image.Height = 32;
                image.Source = mod.icon;
                sp.Children.Add(image);
                CheckBox checkBox = new CheckBox();
                checkBox.IsChecked = mod.enable;
                checkBox.Checked += checkBox_Checked;
                checkBox.Unchecked += checkBox_Unchecked;
                checkBox.Content = mod;
                sp.Children.Add(checkBox);
                modsStackPanel.Add(sp);
            }
            return modsStackPanel.ToArray();
        }

        void checkBox_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckBox cb = (sender as CheckBox);
            var mod = (cb.Content as Core.Data.Mod);
            mod.enable = cb.IsChecked == true;
        }

        void checkBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            CheckBox cb = (sender as CheckBox);
            var mod = (cb.Content as Core.Data.Mod);
            mod.enable = cb.IsChecked == true;
        }

        public static string[] getFiles(string path, string searchPattern, SearchOption searchOption)
        {
            string[] searchPatterns = searchPattern.Split('|');
            List<string> files = new List<string>();
            foreach (string sp in searchPatterns)
                files.AddRange(System.IO.Directory.GetFiles(path, sp, searchOption));
            files.Sort();
            return files.ToArray();
        }
    }
}
