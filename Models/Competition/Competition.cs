using System;

namespace Cubie;

public class Competition
{
    private string _eventName;
    private Globals.Difficulty _difficulty;
    private bool _isRegistered = false;
    private TimeSpan _startTime;
    public Competition(string name, Globals.Difficulty difficulty, TimeSpan startTime)
    {
        EventName = name;
        Difficulty = difficulty;
        StartTime = startTime;
        IsRegistered = false;
    }
    public string GetFormattedTime()
    {
        return StartTime.ToString(@"hh\:mm") + (StartTime.Hours >= 12 ? " PM" : " AM");
    }
    public string GetDisplayText()
    {
        return $"{EventName} ----- {Difficulty} <<{GetFormattedTime()}>>";
    }
    //properties
    public string EventName
    {
        get { return _eventName; }
        set { _eventName = value; }
    }
    public Globals.Difficulty Difficulty
    {
        get { return _difficulty; }
        set { _difficulty = value; }
    }
    public bool IsRegistered
    {
        get { return _isRegistered; }
        set { _isRegistered = value; }
    }
    public TimeSpan StartTime
    {
        get { return _startTime; }
        set { _startTime = value; }
    }
}