using Azure;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenCvSharp.Internal.Vectors;
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

        protected void SetErrorMessage(Core.Entities.Response response)
        {
            if (response.Success)
            {
                this.ErrorMessage = null;
                this.IsErrorMessageVisible = false;
            }
            else
            {
                this.ErrorMessage = response.Message;
                this.IsErrorMessageVisible = true;
            }
        }
    }
}
