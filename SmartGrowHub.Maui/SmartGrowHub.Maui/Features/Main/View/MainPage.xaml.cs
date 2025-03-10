using System.ComponentModel;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Layouts;
using SmartGrowHub.Maui.Base;
using SmartGrowHub.Maui.Features.Main.ViewModel;
using SmartGrowHub.Shared.GrowHubs.Components;

namespace SmartGrowHub.Maui.Features.Main.View;

public sealed partial class MainPage
{
	public MainPage()
	{
		InitializeComponent();
		
		StateContainer.SetCurrentState(DevicesLayout, PageStates.Loading);
		StateContainer.SetCurrentState(EnvironmentStackLayout, PageStates.Loading);
		StateContainer.SetCurrentState(QuickSettingsStackLayout, PageStates.Loading);
	}
	
	private MainPageModel? ViewModel { get; set; }

	protected override void OnBindingContextChanged()
	{
		base.OnBindingContextChanged();
		
		if (BindingContext is not MainPageModel viewModel) return;
		if (ViewModel is not null) ViewModel.PropertyChanged -= OnPropertyChanged;

		ViewModel = viewModel;
		ViewModel.PropertyChanged += OnPropertyChanged;
	}

	private async void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(MainPageModel.CurrentState)) return;
		if (ViewModel is null) return;

		string? newState = ViewModel.CurrentState;

		ViewModel.CanStateChange = false;
		
		Task task1 = ChangeStateAsync(DevicesLayout, newState);
		Task task2 = ChangeStateAsync(EnvironmentStackLayout, newState);
		Task task3 = ChangeStateAsync(QuickSettingsStackLayout, newState);
		
		await Task.WhenAll(task1, task2, task3);
		
		ViewModel.CanStateChange = true;
	}

	private static Task ChangeStateAsync(BindableObject bindableObject, string? state) =>
		StateContainer.ChangeStateWithAnimation(bindableObject, state,
			(element, token) => element.FadeTo(0, easing: Easing.Linear).WaitAsync(token),
			(element, token) => element.FadeTo(1, easing: Easing.Linear).WaitAsync(token));
}