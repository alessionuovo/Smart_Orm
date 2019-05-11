
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Smart_Orm
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
          /*  MainWindow mw = new Smart_Orm.MainWindow();
            mw.Show();*/
            OpenWindow rof = new OpenWindow();
            rof.Show();
        }
    }
}
