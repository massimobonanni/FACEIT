using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FACEIT.Client.Messages;
using FACEIT.Client.Views;
using FACEIT.Core.Entities;
using FACEIT.Core.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FACEIT.Client.ViewModels;

internal partial class MainViewModel : BaseViewModel, IRecipient<FrameCapturedMessage>
{

    private readonly IServiceProvider _serviceProvider;
    private readonly IMessenger _messenger;
    private readonly IGroupsManager _groupManager;

    public MainViewModel(IServiceProvider serviceProvider, IMessenger messenger, IGroupsManager groupManager)
    {
        _serviceProvider = serviceProvider;
        _messenger = messenger;
        _groupManager = groupManager;

        //_messenger.Register(this);

        IsActive = true;
    }

    internal async Task LoadGroupsAsync()
    {
        var response = await _groupManager.GetGroupsAsync();
        if (response.Success)
        {
            Groups.Clear();
            foreach (var group in response.Data.OrderBy(g => g.Name))
            {
                Groups.Add(group);
            }
        }
    }

    override protected async void OnActivated()
    {
        base.OnActivated();
        IsBusy = true;
        await LoadGroupsAsync();
        IsBusy = false;
    }

    [ObservableProperty]
    private ObservableCollection<Group> groups = new ObservableCollection<Group>();

    [ObservableProperty]
    private WriteableBitmap cameraFrame;

    [ObservableProperty]
    private BitmapFrame capturedImage;

    [RelayCommand()]
    private void CaptureImage()
    {
        var currentFrame = BitmapFrame.Create(CameraFrame);
        using var memStream = new MemoryStream();
        BitmapEncoder encoder = new PngBitmapEncoder();
        encoder.Frames.Add(currentFrame);
        encoder.Save(memStream);

        memStream.Position = 0;
        CapturedImage = currentFrame;
    }

    [RelayCommand()]
    private void OpenGroupsManagement()
    {
        _messenger.Send(new OpenNewWindowMessage(nameof(GroupsManagementWindow) ));
    }

    public void Receive(FrameCapturedMessage message)
    {
        this.CameraFrame = message.Value;
    }

}
