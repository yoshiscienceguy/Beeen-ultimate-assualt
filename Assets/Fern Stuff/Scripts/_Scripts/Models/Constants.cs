using System.Collections.Generic;

public class Constants {
    public const string JoinKey = "j";
    public const string DifficultyKey = "d";
    public const string GameTypeKey = "t";

    public static readonly List<string> GameTypes = new() { "Battle Royal", "Team Death Match"};
    public static readonly List<string> Difficulties = new() { "This box is currently usless", "why did you click it it is clearly usless" };
}