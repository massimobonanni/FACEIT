using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FACEIT.Client.Messages;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace FACEIT.Client.ViewModels;

public partial class MainWindowViewModel : ObservableRecipient
{

    private readonly IServiceProvider _serviceProvider;
    private readonly IMessenger _messenger;

    public MainWindowViewModel(IServiceProvider serviceProvider, IMessenger messenger)
    {
        _serviceProvider = serviceProvider;
        _messenger = messenger;
        
        _messenger.Register<FrameCapturedMessage>(this, (r, m) => this.CameraFrame=m.Value);
    }

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
