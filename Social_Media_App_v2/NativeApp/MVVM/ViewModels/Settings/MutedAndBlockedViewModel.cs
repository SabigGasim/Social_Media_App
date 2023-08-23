﻿using Domain.Common;
using NativeApp.Interfaces;
using NativeApp.MVVM.Models;
using System.Windows.Input;

namespace NativeApp.MVVM.ViewModels.Settings;
public class MutedAndBlockedViewModel : SettingsViewModelBase
{
    private ICommand? _saveSettingsCommand;
    private MutedAndBlockedModel? _mutedAndBlocked;

    public MutedAndBlockedViewModel(INavigationService navigationService) : base(navigationService)
    {
    }

    public MutedAndBlockedModel? MutedAndBlocked
    {
        get => _mutedAndBlocked;
        set => TrySetValue(ref _mutedAndBlocked, value);
    }

    public override ICommand? SaveSettingsCommand => _saveSettingsCommand ??= new Command(async () =>
    {
        var result = await UpdateSettings();
        if (result.Success)
        {
            return;
        }

        var errorMessage = string.Join('\n', result.Errors!.Select(e => e.Message));

        Shell.Current.CurrentPage.Dispatcher
        .Dispatch(() => Shell.Current
        .DisplayAlert("Error", errorMessage, "Cancel"));
    });

    protected override Task<Result> UpdateSettings()
    {
        //1 - Update settings on the server


        //2 - if succeeds, update on the UI thread
        Shell.Current.CurrentPage.Dispatcher.Dispatch(() => OnPropertyChanged(nameof(MutedAndBlocked)));

        return base.UpdateSettings();
    }
}