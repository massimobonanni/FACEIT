using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using FACEIT.Client.ViewModels;
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

namespace FACEIT.Client.Views
{
    /// <summary>
    /// Interaction logic for PersonsManagementWindow.xaml
    /// </summary>
    public partial class PersonsManagementWindow : Window
    {
        private IMessenger _messenger;
        public PersonsManagementWindow()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<PersonsManagementViewModel>();

            _messenger = Ioc.Default.GetRequiredService<IMessenger>();
        }

        internal PersonsManagementViewModel ViewModel => (PersonsManagementViewModel)DataContext;
    }
}
