namespace Ameko.AssCS;

/// <summary>
/// A timestamp
/// </summary>
/// <author>9volt</author>
public class AssTime
{
    public int Hour { get; set; }
    public int Minute { get; set; }
    public int Second { get; set; }
    public int Hundredths { get; set; }

    /// <summary>
    /// Get the value of this AssTime instance in milliseconds
    /// </summary>
    /// <returns>Milliseconds</returns>
    public long Milliseconds()
    {
        return Hundredths * 10 +
               Second * 1000 +
               Minute * 1000 * 60 +
               Hour * 1000 * 60 * 60;
    }

    /// <summary>
    /// Create a timestamp
    /// </summary>
    /// <param name="data">Timestamp in <c>HH:MM:SS.ms</c> format</param>
    /// <returns>New AssTime instance</returns>
    public static AssTime Make(string? data)
    {
        var tokens = data.Split(new char[] { ':', '.' });
        return new AssTime
        {
            Hour = int.Parse(tokens[0]),
            Minute = int.Parse(tokens[1]),
            Second = int.Parse(tokens[2]),
            Hundredths = int.Parse(tokens[3]),
        };
    }

    /// <summary>
    /// Override the + operator for adding 2 AssTime objects together.
    /// It is assumed that both are positive.
    /// </summary>
    /// <param name="time1">AssTime object a</param>
    /// <param name="time2">AssTime object b</param>
    /// <returns>AssTime object with the sum</returns>
    public static AssTime operator +(AssTime time1, AssTime time2)
    {
        var hundredths = time1.Hundredths + time2.Hundredths;
        var second = time1.Second + time2.Second;
        var minute = time1.Minute + time2.Minute;
        var hour = time1.Hour + time2.Hour;
        while (hundredths > 99)
        {
            second += 1;
            hundredths -= 100;
        }

        while (second > 59)
        {
            minute += 1;
            second -= 60;
        }

        while (minute > 59)
        {
            hour += 1;
            minute -= 60;
        }

        return new AssTime
        {
            Hour = hour,
            Minute = minute,
            Second = second,
            Hundredths = hundredths
        };
    }

    /// <summary>
    /// Add seconds.hundredths to an AssTime object.
    /// It is assumed that the seconds are positive.
    /// </summary>
    /// <param name="time1">AssTime to add to</param>
    /// <param name="seconds">Double seconds.hundredths</param>
    /// <returns>AssTime object containing the sum</returns>
    public static AssTime operator +(AssTime time1, double seconds)
    {
        var secs = (int) Math.Floor(seconds);
        var hundredths = (int) (seconds - secs);
        var temp = new AssTime
        {
            Hour = 0,
            Minute = 0,
            Second = secs,
            Hundredths = hundredths
        };
        return time1 + temp;
    }
    
    /// <summary>
    /// Override the - operator for subtracting 2 AssTime objects together.
    /// It is assumed that both are positive.
    /// </summary>
    /// <param name="time1">AssTime object a</param>
    /// <param name="time2">AssTime object b</param>
    /// <returns>AssTime object with the difference</returns>
    public static AssTime operator -(AssTime time1, AssTime time2)
    {
        var hundredths = time1.Hundredths - time2.Hundredths;
        var second = time1.Second - time2.Second;
        var minute = time1.Minute - time2.Minute;
        var hour = time1.Hour - time2.Hour;
        while (hundredths < 0)
        {
            second -= 1;
            hundredths += 100;
        }

        while (second < 0)
        {
            minute -= 1;
            second += 60;
        }

        while (minute < 0)
        {
            hour -= 1;
            minute += 60;
        }

        if (hour < 0) throw new AssException("AssTime value cannot be negative!");

        return new AssTime
        {
            Hour = hour,
            Minute = minute,
            Second = second,
            Hundredths = hundredths
        };
    }

    /// <summary>
    /// Subtract seconds.hundredths from an AssTime object.
    /// It is assumed that the seconds are positive.
    /// </summary>
    /// <param name="time1">AssTime to subtract from</param>
    /// <param name="seconds">Double seconds.hundredths</param>
    /// <returns>AssTime object containing the difference</returns>
    public static AssTime operator -(AssTime time1, double seconds)
    {
        var secs = (int) Math.Floor(seconds);
        var hundredths = (int) (seconds - secs);
        var temp = new AssTime
        {
            Hour = 0,
            Minute = 0,
            Second = secs,
            Hundredths = hundredths
        };
        return time1 - temp;
    }

    public static bool operator ==(AssTime time1, AssTime time2)
    {
        return time1.Hour == time2.Hour &&
               time1.Minute == time2.Minute &&
               time1.Second == time2.Second &&
               time1.Hundredths == time2.Hundredths;
    }

    public static bool operator !=(AssTime time1, AssTime time2)
    {
        return !(time1 == time2);
    }

    public static bool operator >(AssTime time1, AssTime time2)
    {
        return time1.Milliseconds() > time2.Milliseconds();
    }

    public static bool operator <(AssTime time1, AssTime time2)
    {
        return time1.Milliseconds() < time2.Milliseconds();
    }

    public static bool operator >=(AssTime time1, AssTime time2)
    {
        return time1.Milliseconds() >= time2.Milliseconds();
    }
    
    public static bool operator <=(AssTime time1, AssTime time2)
    {
        return time1.Milliseconds() <= time2.Milliseconds();
    }

    public override string ToString()
    {
        return $"{Hour}:{Minute:D2}:{Second:D2}.{Hundredths:D2}";
    }

    public override bool Equals(object obj) => (obj is AssTime other) && this == other;
    public override int GetHashCode() => 13 * ToString().GetHashCode();
}