using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AddressBook_MVVM.ViewModel;

namespace AddressBook_MVVM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            MainWindow view = new MainWindow();
            ContactManagerViewModel viewModel = new ContactManagerViewModel();
            view.DataContext = viewModel;
            view.Show();
        }
    }
}
