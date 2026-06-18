using System.ComponentModel;
using CommunityToolkit.Maui.Layouts;
using SmartGrowHub.Maui.Base;
using SmartGrowHub.Maui.Features.Main.ViewModel;

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
		
		await Task.WhenAll(
			ChangeStateAsync(DevicesLayout, newState),
			ChangeStateAsync(EnvironmentStackLayout, newState),
			ChangeStateAsync(QuickSettingsStackLayout, newState));
		
		ViewModel.CanStateChange = true;
	}
	
	private static Task ChangeStateAsync(BindableObject bindableObject, string? state) =>
		StateContainer.ChangeStateWithAnimation(bindableObject, state,
			(element, token) => element.FadeToAsync(0, easing: Easing.Linear).WaitAsync(token),
			(element, token) => element.FadeToAsync(1, easing: Easing.Linear).WaitAsync(token));
}