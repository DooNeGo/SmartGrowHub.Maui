using System.ComponentModel;
using CommunityToolkit.Maui.Layouts;
using SmartGrowHub.Maui.Base;
using SmartGrowHub.Maui.Features.Main.ViewModel;

namespace SmartGrowHub.Maui.Features.Main.View;

public sealed partial class MainPage
{
	public MainPage(MainPageModel pageModel) : base(pageModel)
	{
		InitializeComponent();
		pageModel.PropertyChanged += OnPropertyChanged;
		
		StateContainer.SetCurrentState(DevicesLayout, PageStates.Loading);
		StateContainer.SetCurrentState(EnvironmentStackLayout, PageStates.Loading);
		StateContainer.SetCurrentState(QuickSettingsStackLayout, PageStates.Loading);
	}

	private async void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(MainPageModel.CurrentState)) return;

		string? newState = BindingContext.CurrentState;

		BindingContext.CanStateChange = false;
		
		Task task1 = ChangeStateAsync(DevicesLayout, newState);
		Task task2 = ChangeStateAsync(EnvironmentStackLayout, newState);
		Task task3 = ChangeStateAsync(QuickSettingsStackLayout, newState);
		
		await Task.WhenAll(task1, task2, task3);
		
		BindingContext.CanStateChange = true;
	}

	private static Task ChangeStateAsync(BindableObject bindableObject, string? state) =>
		StateContainer.ChangeStateWithAnimation(bindableObject, state,
			(element, token) => element.FadeTo(0, easing: Easing.Linear).WaitAsync(token),
			(element, token) => element.FadeTo(1, easing: Easing.Linear).WaitAsync(token));
}