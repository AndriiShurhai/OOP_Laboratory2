using System;

class MyTime
{
    public int Hour { get; private set; }
    public int Minute { get; private set; }
    public int Second { get; private set; }

    public MyTime() : this(0, 0, 0) { }

    public MyTime(int h, int m, int s)
    {
        int totalSeconds = (h * 3600) + (m * 60) + s;
        SetFromSeconds(totalSeconds);
    }

    private void SetFromSeconds(int totalSeconds)
    {
        int secPerDay = 60 * 60 * 24;
        totalSeconds %= secPerDay;
        if (totalSeconds < 0) totalSeconds += secPerDay;

        Hour = totalSeconds / 3600;
        Minute = (totalSeconds / 60) % 60;
        Second = totalSeconds % 60;
    }

    public override string ToString()
    {
        return $"{Hour:D2}:{Minute:D2}:{Second:D2}";
    }

    public int TimeSinceMidnight()
    {
        return Hour * 3600 + Minute * 60 + Second;
    }

    public void AddSeconds(int seconds)
    {
        int totalSeconds = TimeSinceMidnight() + seconds;
        SetFromSeconds(totalSeconds);
    }

    public void AddOneSecond()
    {
        SetFromSeconds(TimeSinceMidnight() + 1);
    }

    public void AddOneMinute()
    {
        SetFromSeconds(TimeSinceMidnight() + 60);
    }

    public void AddOneHour()
    {
        SetFromSeconds(TimeSinceMidnight() + 3600);
    }

    public int Difference(MyTime other)
    {
        return this.TimeSinceMidnight() - other.TimeSinceMidnight();
    }

    public string WhatLesson()
    {
        int timeInSeconds = TimeSinceMidnight();

        int[] startTimes = {
            8 * 3600,               // Початок 1-ї пари: 08:00
            9 * 3600 + 20 * 60,     // Кінець 1-ї пари: 09:20
            9 * 3600 + 40 * 60,     // Початок 2-ї пари: 09:40
            11 * 3600,              // Кінець 2-ї пари: 11:00
            11 * 3600 + 20 * 60,    // Початок 3-ї пари: 11:20
            12 * 3600 + 40 * 60,    // Кінець 3-ї пари: 12:40
            13 * 3600,              // Початок 4-ї пари: 13:00
            14 * 3600 + 20 * 60,    // Кінець 4-ї пари: 14:20
            14 * 3600 + 40 * 60,    // Початок 5-ї пари: 14:40
            16 * 3600,              // Кінець 5-ї пари: 16:00
            16 * 3600 + 10 * 60,    // Початок 6-ї пари: 16:10
            17 * 3600 + 30 * 60,    // Кінець 6-ї пари: 17:30
            17 * 3600 + 40 * 60,    // Початок 7-ї пари: 17:40
            19 * 3600               // Кінець 7-ї пари: 19:00
        };

        string[] lessons = {
            "1-а пара", "перерва між 1-ю та 2-ю парами",
            "2-а пара", "перерва між 2-ю та 3-ю парами",
            "3-я пара", "перерва між 3-ю та 4-ю парами",
            "4-а пара", "перерва між 4-ю та 5-ю парами",
            "5-а пара", "перерва між 5-ю та 6-ю парами",
            "6-а пара", "перерва між 6-ю та 7-ю парами",
            "7-а пара", "пари вже скінчилися"
        };

        for (int i = 0; i < startTimes.Length - 1; i += 2)
        {
            if (timeInSeconds < startTimes[i])
                return i == 0 ? "пари ще не почалися" : lessons[i - 1];
            else if (timeInSeconds < startTimes[i + 1])
                return lessons[i];
        }

        return "пари вже скінчилися";
    }
}
