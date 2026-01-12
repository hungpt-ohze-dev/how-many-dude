using System;

public static class DateUtils
{
    public static DateTime Now => DateTime.UtcNow;

    public static bool IsNewDay()
    {
        string lastDateStr = DataManager.Save.User.lastPlayTime;
        DateTime todayUTC = Now.Date;

        if (string.IsNullOrEmpty(lastDateStr))
        {
            SaveCurrentDate();
            return false; // First time or data missing
        }

        DateTime lastDate = DateTime.Parse(lastDateStr);
        bool isNewDay = todayUTC > lastDate;

        if (isNewDay)
        {
            SaveCurrentDate(); // Update only if a full day has passed
        }

        return isNewDay;
    }

    public static void SaveCurrentDate()
    {
        var userSave = DataManager.Save.User;
        userSave.lastPlayTime = DateUtils.Now.ToString();
        userSave.Save();
    }
}
