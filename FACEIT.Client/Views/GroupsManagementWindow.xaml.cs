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
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using FACEIT.Client.Messages;
using FACEIT.Client.ViewModels;

namespace FACEIT.Client.Views
{
    /// <summary>
    /// Interaction logic for GroupsManagementWindow.xaml
    /// </summary>
    public partial class GroupsManagementWindow : Window
    {
        private IMessenger _messenger;

        public GroupsManagementWindow()
        {
            InitializeComponent();

            DataContext = Ioc.Default.GetRequiredService<GroupsManagementViewModel>();
            
            _messenger = Ioc.Default.GetRequiredService<IMessenger>();
        }

        internal GroupsManagementViewModel ViewModel => (GroupsManagementViewModel)DataContext;
    }
}
