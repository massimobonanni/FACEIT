using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.Client.ViewModels
{
    internal partial class BaseViewModel : ObservableRecipient
    {

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private bool isErrorMessageVisible;

        [RelayCommand()]
        public void ClearErrorMessage()
        {
            this.ErrorMessage = string.Empty;
            this.IsErrorMessageVisible = false;
        }
    }
}
