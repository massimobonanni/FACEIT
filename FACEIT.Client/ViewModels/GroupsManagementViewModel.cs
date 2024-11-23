using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FACEIT.Client.Entities;
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

internal partial class GroupsManagementViewModel : BaseViewModel
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMessenger _messenger;
    private readonly IGroupsManager _groupManager;

    public GroupsManagementViewModel(IServiceProvider serviceProvider, IMessenger messenger, IGroupsManager groupManager)
    {
        _serviceProvider = serviceProvider;
        _messenger = messenger;
        _groupManager = groupManager;

        LoadGroupsCommand = new AsyncRelayCommand(LoadGroupsAsync);
        SaveGroupCommand = new AsyncRelayCommand(SaveGroupAsync);
        DeleteGroupCommand = new AsyncRelayCommand(DeleteGroupAsync);
        TrainGroupCommand = new AsyncRelayCommand(TrainGroupAsync);

        ErrorMessage = string.Empty;
        IsErrorMessageVisible = false;
        IsBusy = false;

        IsActive = true;
    }

    protected override async void OnActivated()
    {
        base.OnActivated();

        IsBusy = true;
        await LoadGroupsAsync();
        IsBusy = false;
    }

    public IAsyncRelayCommand LoadGroupsCommand { get; }
    public IAsyncRelayCommand SaveGroupCommand { get; }
    public IAsyncRelayCommand DeleteGroupCommand { get; }
    public IAsyncRelayCommand TrainGroupCommand { get; }

    private async Task LoadGroupsAsync()
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
                var trainingDataResponse = await this._groupManager.GetTrainingStatusAsync(group.Id);
                if (trainingDataResponse.Success)
                {
                    clientGroup.TrainingData = trainingDataResponse.Data;
                }
                Groups.Add(clientGroup);
            }
        }
    }


    [RelayCommand()]
    public void AddGroup()
    {
        var group = new Client.Entities.Group();
        group.Id = Guid.NewGuid().ToString();
        group.Name = "New Group";
        group.IsNew = true;
        Groups.Add(group);
        SelectedGroup = group;
    }

    private async Task TrainGroupAsync()
    {
        IsBusy = true;
        if (IsGroupSelected)
        {
            var response = await this._groupManager.TrainGroupAsync(this.SelectedGroup.Id);
            if (response.Success)
            {
                var trainResponse = await _groupManager.GetTrainingStatusAsync(SelectedGroup.Id);
                if (trainResponse.Success)
                {
                    var currentGroup = this.SelectedGroup;
                    currentGroup.TrainingData= trainResponse.Data;
                    this.SelectedGroup = null;
                    this.SelectedGroup = currentGroup;
                }
            }
            else
            {
                ErrorMessage = response.Message;
                IsErrorMessageVisible = true;
            }
        }
        IsBusy = false;
    }

    public async Task DeleteGroupAsync()
    {
        IsBusy = true;
        var response = await this._groupManager.RemoveGroupAsync(this.SelectedGroup.Id);
        _messenger.Send(new GroupChangedMessage(this.SelectedGroup, GroupChangeType.Deleted));
        await LoadGroupsAsync();
        IsBusy = false;
    }
    public async Task SaveGroupAsync()
    {
        IsBusy = true;
        if (this.SelectedGroup.IsNew)
        {
            var createresponse = await this._groupManager.CreateGroupAsync(this.SelectedGroup.Id, this.SelectedGroup.Name,this.SelectedGroup.Properties);
            _messenger.Send(new GroupChangedMessage(this.SelectedGroup, GroupChangeType.Created));
        }
        else
        {
            var udateResponse = await this._groupManager.UpdateGroupAsync(this.SelectedGroup.Id, this.SelectedGroup.Name, this.SelectedGroup.Properties);
            _messenger.Send(new GroupChangedMessage(this.SelectedGroup, GroupChangeType.Updated));
        }
        await LoadGroupsAsync();
        IsBusy = false;
    }

    [ObservableProperty]
    private ObservableCollection<Client.Entities.Group> groups = new ObservableCollection<Client.Entities.Group>();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsGroupSelected))]
    private Client.Entities.Group selectedGroup;

    public bool IsGroupSelected { get => SelectedGroup != null; }



}
