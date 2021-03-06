﻿using Detrav.TeraApi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Detrav.TeraModLoader.Windows
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.debug("{0}", Core.Data.ExceptionExtended.GetExceptionDetails(e.Exception));
        }



        private void Application_Startup(object sender, StartupEventArgs e)
        {
#if !DEBUG
            this.DispatcherUnhandledException += Application_DispatcherUnhandledException;
#endif
        }
    }
}
