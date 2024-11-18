using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FACEIT.Client.Messages;
using FACEIT.Core.Entities;
using FACEIT.Core.Interfaces;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FACEIT.Client.ViewModels;

public partial class MainWindowViewModel : ObservableRecipient
{

    private readonly IServiceProvider _serviceProvider;
    private readonly IMessenger _messenger;
    private readonly IGroupsManager _groupManager;

    public MainWindowViewModel(IServiceProvider serviceProvider, IMessenger messenger, IGroupsManager groupManager)
    {
        _serviceProvider = serviceProvider;
        _messenger = messenger;
        _groupManager = groupManager;

        _messenger.Register<FrameCapturedMessage>(this, (r, m) => this.CameraFrame = m.Value);
        _messenger.Register<CameraReadyMessage>(this, async (r, m) =>
        {
            await LoadGroupsAsync();
        });

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

    [ObservableProperty]
    private ObservableCollection<Group> groups = new ObservableCollection<Group>();

    [ObservableProperty]
    private Group selectedGroup;

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
}
