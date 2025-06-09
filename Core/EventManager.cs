using System;
using System.Collections.Generic;

namespace Cubie;

public class EventManager
{
    private readonly List<string> _eventNames = new()
    {
        "Speed Solve",
        "Blindfold Challenge",
        "One-Handed Solve",
        "Fewest Moves",
        "Multi-Cube Relay"
    };

    private Random random = new();
    private List<Competition> _todayEvent;

    public void CreateEvent()
    {
        TodayEvents = new List<Competition>();
        int eventCount = 3;
        for (int i = 0; i < eventCount; i++)
        {
            string name = EventNames[random.Next(EventNames.Count)];
            Globals.Difficulty difficulty = (Globals.Difficulty)random.Next(3);

            //randomize the time
            int hour = random.Next(9, 18); //from 9am to 6pm
            int minute = random.Next(0, 2) * 30; // 0 or 30min
            TimeSpan time = new TimeSpan(hour, minute, 0);
            TodayEvents.Add(new Competition(name, difficulty, time));
        }
    }
    public List<Competition> TodayEvents
    {
        get { return _todayEvent; }
        private set { _todayEvent = value; }
    }
    private List<string> EventNames
    {
        get { return _eventNames; }
    }

}