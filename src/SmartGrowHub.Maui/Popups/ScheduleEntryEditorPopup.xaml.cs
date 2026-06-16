using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Maui.Services.Infrastructure;
using SmartGrowHub.Shared.GrowHubs.Model;

namespace SmartGrowHub.Maui.Popups;

public sealed partial class ScheduleEntryEditorPopup
{
    private readonly TaskCompletionSource<ScheduleEntryResult?> _tcs = new();
    private readonly IPopupNavigation _popupNavigation;
    private readonly bool _isWeekly;

    public ScheduleEntryEditorPopup(IPopupNavigation popupNavigation, bool isWeekly)
    {
        InitializeComponent();
        _popupNavigation = popupNavigation;
        _isWeekly = isWeekly;

        DayPickersSection.IsVisible = isWeekly;

        var days = Enum.GetValues<DayOfWeek>();
        StartDayPicker.ItemsSource = days;
        EndDayPicker.ItemsSource = days;
        StartDayPicker.SelectedIndex = 0;
        EndDayPicker.SelectedIndex = 0;

        StartTimePicker.Time = new TimeSpan(8, 0, 0);
        EndTimePicker.Time = new TimeSpan(20, 0, 0);

        KindPicker.ItemsSource = new[] { AppResources.Power, AppResources.Prefer };
        KindPicker.SelectedIndex = 0;

        MagnitudeSlider.Value = 50;
    }

    public Task<ScheduleEntryResult?> WaitForResultAsync() => _tcs.Task;

    private void OnMagnitudeChanged(object? sender, ValueChangedEventArgs e)
    {
        MagnitudeLabel.Text = $"{e.NewValue:F0}%";
    }

    private void OnSaveClicked(object? sender, EventArgs e)
    {
        var kind = KindPicker.SelectedIndex == 0
            ? ScheduleUnitKindDto.Power
            : ScheduleUnitKindDto.Prefer;

        var startTime = TimeOnly.FromTimeSpan(StartTimePicker.Time ?? TimeSpan.Zero);
        var endTime = TimeOnly.FromTimeSpan(EndTimePicker.Time ?? TimeSpan.Zero);

        var startDay = _isWeekly
            ? (DayOfWeek)(StartDayPicker.SelectedIndex)
            : DayOfWeek.Monday;
        var endDay = _isWeekly
            ? (DayOfWeek)(EndDayPicker.SelectedIndex)
            : DayOfWeek.Monday;

        _tcs.TrySetResult(new ScheduleEntryResult(
            startTime, endTime, startDay, endDay, kind, (float)MagnitudeSlider.Value));
        Close();
    }

    private void OnCancelClicked(object? sender, EventArgs e)
    {
        _tcs.TrySetResult(null);
        Close();
    }

    private void Close()
    {
        _popupNavigation.HidePopup(this).RunAsync();
    }
}

public sealed record ScheduleEntryResult(
    TimeOnly StartTime,
    TimeOnly EndTime,
    DayOfWeek StartDay,
    DayOfWeek EndDay,
    ScheduleUnitKindDto Kind,
    float Magnitude);
