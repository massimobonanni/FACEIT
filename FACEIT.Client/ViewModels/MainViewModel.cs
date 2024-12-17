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

internal partial class MainViewModel : BaseViewModel, IRecipient<FrameCapturedMessage>, IRecipient<GroupChangedMessage>
{

    private readonly IServiceProvider _serviceProvider;
    private readonly IMessenger _messenger;
    private readonly IGroupsManager _groupManager;
    private readonly IFaceRecognizer _faceRecognizer;
    private readonly IPersonsManager _personsManager;

    public MainViewModel(IServiceProvider serviceProvider, IMessenger messenger,
        IGroupsManager groupManager, IFaceRecognizer faceRecognizer, IPersonsManager personsManager)
    {
        _serviceProvider = serviceProvider;
        _messenger = messenger;
        _groupManager = groupManager;
        _faceRecognizer = faceRecognizer;
        _personsManager = personsManager;

        CaptureImageCommand = new AsyncRelayCommand(CaptureImageAsync, CanCaptureImage);

        IsActive = true;
    }

    public IAsyncRelayCommand CaptureImageCommand { get; }

    internal async Task LoadGroupsAsync()
    {
        var response = await _groupManager.GetGroupsAsync();
        if (response.Success)
        {
            Groups.Clear();
            foreach (var group in response.Data.OrderBy(g => g.Name))
            {
                var clientGroup = new Client.Entities.Group
                {
                    Id = group.Id,
                    Name = group.Name,
                    Properties = group.Properties,
                    IsNew = false
                };
                Groups.Add(clientGroup);
            }
            SelectedGroup = null;
        }
        else
        {
            SetErrorMessage(response);
        }
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsGroupSelected))]
    [NotifyCanExecuteChangedFor(nameof(CaptureImageCommand))]
    private Client.Entities.Group selectedGroup;

    public bool IsGroupSelected { get => SelectedGroup != null; }

    override protected async void OnActivated()
    {
        base.OnActivated();
        IsBusy = true;
        await LoadGroupsAsync();
        IsBusy = false;
    }

    [ObservableProperty]
    private ObservableCollection<Client.Entities.Group> groups = new ObservableCollection<Client.Entities.Group>();

    [ObservableProperty]
    private WriteableBitmap cameraFrame;

    [ObservableProperty]
    private BitmapFrame capturedImage;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsPersonRecognized))]
    [NotifyPropertyChangedFor(nameof(IsNotPersonRecognized))]
    private Client.Entities.RecognizedPerson recognizedPerson;

    public bool IsPersonRecognized { get => RecognizedPerson != null; }
    public bool IsNotPersonRecognized { get => RecognizedPerson == null; }

    [ObservableProperty]
    private bool showRecognizedPersonPanel;

    private async Task CaptureImageAsync()
    {
        IsBusy = true;
        this.ShowRecognizedPersonPanel = false;
        var currentFrame = BitmapFrame.Create(CameraFrame);
        using var memStream = new MemoryStream();
        BitmapEncoder encoder = new PngBitmapEncoder();
        encoder.Frames.Add(currentFrame);
        encoder.Save(memStream);

        memStream.Position = 0;
        CapturedImage = currentFrame;

        var tempFaceResponse = await this._faceRecognizer.DetectAsync(memStream, 60);
        if (tempFaceResponse.Success)
        {
            if (tempFaceResponse.Data != null)
            {
                var faceResponse = await this._faceRecognizer.RecognizeAsync(this.SelectedGroup.Id, tempFaceResponse.Data, 0.80f);
                if (faceResponse.Success)
                {
                    if (faceResponse.Data == null || !faceResponse.Data.Any())
                    {
                        this.ShowRecognizedPersonPanel = true;
                        this.RecognizedPerson = null;
                        _messenger.Send(new PersonRecognizedMessage(this.RecognizedPerson));
                    }
                    else
                    {
                        var personRecognized = faceResponse.Data.OrderByDescending(p => p.Confidence).First();
                        var personResponse = await this._personsManager.GetPersonAsync(this.SelectedGroup.Id, personRecognized.Id);
                        if (personResponse.Success)
                        {
                            this.RecognizedPerson = new Client.Entities.RecognizedPerson
                            {
                                Id = personRecognized.Id,
                                Person = new Entities.Person(personResponse.Data),
                                Confidence = personRecognized.Confidence
                            };
                            this.ShowRecognizedPersonPanel = true;
                            _messenger.Send(new PersonRecognizedMessage(this.RecognizedPerson));
                        }
                        else
                        {
                            SetErrorMessage(personResponse);
                        }
                    }
                }
                else
                {
                    SetErrorMessage(faceResponse);
                }
            }
            else
            {
                SetErrorMessage("No face detected");
            }
        }
        else
        {
            SetErrorMessage(tempFaceResponse);
        }

        IsBusy = false;
    }

    private bool CanCaptureImage()
    {
        return IsGroupSelected;
    }

    [RelayCommand()]
    private void OpenGroupsManagement()
    {
        _messenger.Send(new OpenNewWindowMessage(nameof(GroupsManagementWindow)));
    }

    [RelayCommand()]
    private void OpenPersonsManagement()
    {
        _messenger.Send(new OpenNewWindowMessage(nameof(PersonsManagementWindow)));
    }

    public void Receive(FrameCapturedMessage message)
    {
        this.CameraFrame = message.Value;
    }

    public async void Receive(GroupChangedMessage message)
    {
        var selectedGroup = this.SelectedGroup;
        await LoadGroupsAsync();
        if (selectedGroup != null)
            this.SelectedGroup = Groups.FirstOrDefault(g => g.Id == selectedGroup.Id);
    }
}
