// Tutorial: Hello World - Edit your code step
// Using C# interpolated strings ($ prefix) instead of string concatenation

string aFriend = "Isabella";
Console.WriteLine($"Hello, {aFriend}!");

// Calculate days until next Christmas
DateTime today = DateTime.Today;
DateTime christmas = new DateTime(today.Year, 12, 25);

// If Christmas has already passed this year, use next year's
if (today > christmas)
{
    christmas = christmas.AddYears(1);
}

int daysUntilChristmas = (christmas - today).Days;
Console.WriteLine($"There are {daysUntilChristmas} days until Christmas!");

