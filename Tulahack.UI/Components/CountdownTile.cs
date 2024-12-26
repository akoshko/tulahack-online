using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;

namespace Tulahack.UI.Components;

public partial class CountdownTile : UserControl
{
    /// <summary>
    /// Defines the Deadline (<see cref="Deadline"/>) property.
    /// </summary>
    public static readonly StyledProperty<DateTime> DeadlineProperty =
        AvaloniaProperty.Register<DashboardTile, DateTime>(nameof(Deadline), defaultValue: DateTime.Now.AddDays(1));

    /// <summary>
    /// DateTime property determining countdown end.
    /// Result time interval is calculated as Deadline - DateTime.Now
    /// </summary>
    public DateTime Deadline
    {
        get => GetValue(DeadlineProperty);
        set => SetValue(DeadlineProperty, value);
    }
    
    /// <summary>
    /// Defines the <see cref="Interval"/> property.
    /// </summary>
    public static readonly StyledProperty<int> IntervalProperty =
        AvaloniaProperty.Register<DashboardTile, int>(nameof(Interval), defaultValue: 500);

    /// <summary>
    /// Tile header text
    /// </summary>
    public int Interval
    {
        get => GetValue(IntervalProperty);
        set => SetValue(IntervalProperty, value);
    }
    
    private DispatcherTimer? _timer;
    private TimeSpan _time;

    public CountdownTile()
    {
        InitializeComponent();

        TimerTextBlock.Text = $"--- days\n" +
                              $"--- hours\n" +
                              $"--- minutes\n" +
                              $"and --- seconds";
        
        DeadlineProperty.Changed.Subscribe(_ => SetupTimer());
    }

    private void SetupTimer()
    {
        _time = Deadline - DateTime.Now;
        var interval = Interval;
        _timer = new DispatcherTimer(new TimeSpan(0, 0, 0, 0, interval), DispatcherPriority.Normal, delegate
        {
            TimerTextBlock.Text = $"{_time:%d} days\n" +
                                  $"{_time:hh} hours\n" +
                                  $"{_time:mm} minutes\n" +
                                  $"and {_time:ss} seconds";
            if (_time == TimeSpan.Zero) _timer?.Stop();
            _time = _time.Add(TimeSpan.FromMilliseconds(-interval));
        });

        _timer.Start();
    }
}