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
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FACEIT.Client.ViewModels;

internal partial class PersonsManagementViewModel : BaseViewModel, IRecipient<FrameCapturedMessage>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMessenger _messenger;
    private readonly IPersonsManager _personsManager;
    private readonly IGroupsManager _groupsManager;

    public PersonsManagementViewModel(IServiceProvider serviceProvider, IMessenger messenger,
        IPersonsManager personsManager, IGroupsManager groupsManager)
    {
        _serviceProvider = serviceProvider;
        _messenger = messenger;
        _personsManager = personsManager;
        _groupsManager = groupsManager;

        CaptureImageCommand = new AsyncRelayCommand(CaptureImageAsync);
        LoadPersonsCommand = new AsyncRelayCommand(LoadPersonsAsync);
        SavePersonCommand = new AsyncRelayCommand(SavePersonAsync);
        DeletePersonCommand = new AsyncRelayCommand(DeletePersonAsync);

        ErrorMessage = string.Empty;
        IsErrorMessageVisible = false;
        IsBusy = false;

        IsActive = true;
    }

    private async Task DeletePersonAsync()
    {
        IsBusy = true;
        if (IsPersonSelected)
        {
            var deleteResponse = await this._personsManager.RemovePersonAsync(this.SelectedGroup.Id, this.SelectedPerson.Id);
            if (deleteResponse.Success)
            {
                await LoadPersonsAsync();
                SelectedPerson = null;
            }
            else
            {
                ErrorMessage = deleteResponse.Message;
                IsErrorMessageVisible = true;
            }
        }
        IsBusy = false;
    }

    private async Task SavePersonAsync()
    {
        IsBusy = true;
        if (IsPersonSelected)
        {
            Response response = null;
            string personId = null;
            if (SelectedPerson.IsNew)
            {
                response = await this._personsManager.CreatePersonAsync(this.SelectedGroup.Id, this.SelectedPerson.Name,this.SelectedPerson.Properties);
                if (response.Success)
                {
                    personId = ((Response<Person>)response).Data.Id;
                }
            }
            else
            {
                response = await this._personsManager.UpdatePersonAsync(this.SelectedGroup.Id, this.SelectedPerson.Id, this.SelectedPerson.Name, this.SelectedPerson.Properties);
                personId = this.SelectedPerson.Id;
            }

            if (response.Success)
            {
                await LoadPersonsAsync();
                SelectedPerson = Persons.FirstOrDefault(p => p.Id == personId);
            }
            else
            {
                ErrorMessage = response.Message;
                IsErrorMessageVisible = true;
            }
        }
        IsBusy = false;
    }

    public IAsyncRelayCommand LoadPersonsCommand { get; }
    public IAsyncRelayCommand SavePersonCommand { get; }
    public IAsyncRelayCommand DeletePersonCommand { get; }

    public IAsyncRelayCommand CaptureImageCommand { get; }

    protected override async void OnActivated()
    {
        base.OnActivated();

        IsBusy = true;
        await LoadGroupsAsync();
        IsBusy = false;
    }

    private async Task LoadGroupsAsync()
    {
        var response = await _groupsManager.GetGroupsAsync();
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
        }
    }

    private async Task LoadPersonsAsync()
    {
        var response = await _personsManager.GetPersonsByGroupAsync(SelectedGroup.Id);
        if (response.Success)
        {
            Persons.Clear();
            foreach (var person in response.Data.OrderBy(g => g.Name))
            {
                var clientPerson = new Client.Entities.Person
                {
                    Id = person.Id,
                    Name = person.Name,
                    Properties = person.Properties,
                    PersistedFaceIds = person.PersistedFaceIds,
                    IsNew = false
                };
                Persons.Add(clientPerson);
            }
            SelectedPerson = null;
        }
    }

    [ObservableProperty]
    private ObservableCollection<Client.Entities.Group> groups = new ObservableCollection<Client.Entities.Group>();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsGroupSelected))]
    private Client.Entities.Group selectedGroup;

    async partial void OnSelectedGroupChanged(Entities.Group value)
    {
        //IsBusy = true;
        if (IsGroupSelected)
            await LoadPersonsAsync();
        else
            Persons.Clear();
        //IsBusy = false;
    }

    public bool IsGroupSelected { get => SelectedGroup != null; }

    [ObservableProperty]
    private ObservableCollection<Client.Entities.Person> persons = new ObservableCollection<Client.Entities.Person>();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsPersonSelected))]
    [NotifyPropertyChangedFor(nameof(DisplayCameraCapture))]
    private Client.Entities.Person selectedPerson;

    public bool IsPersonSelected { get => SelectedPerson != null; }
    public bool DisplayCameraCapture { get => SelectedPerson != null && !SelectedPerson.IsNew; }

    public void Receive(FrameCapturedMessage message)
    {
        this.CameraFrame = message.Value;
    }

    [ObservableProperty]
    private WriteableBitmap cameraFrame;

    [ObservableProperty]
    private BitmapFrame capturedImage;

    private async Task CaptureImageAsync()
    {
        IsBusy = true;
        var currentFrame = BitmapFrame.Create(CameraFrame);
        using var memStream = new MemoryStream();
        BitmapEncoder encoder = new PngBitmapEncoder();
        encoder.Frames.Add(currentFrame);
        encoder.Save(memStream);

        memStream.Position = 0;

        if (SelectedPerson != null && !SelectedPerson.IsNew)
        {
            var response = await _personsManager.AddImageToPersonAsync(SelectedGroup.Id, SelectedPerson.Id, memStream);
            if (response.Success)
            {
                var currentPerson = SelectedPerson;
                await LoadPersonsAsync();
                SelectedPerson = Persons.FirstOrDefault(p => p.Id == currentPerson.Id);
            }
        }
        IsBusy = false;
    }

    [RelayCommand()]
    public void AddPerson()
    {
        var person = new Client.Entities.Person();
        person.Name = "New Person";
        person.IsNew = true;
        Persons.Add(person);
        SelectedPerson = person;
    }
}
