// See https://aka.ms/new-console-template for more information
using System.Security.AccessControl;
using Discord;
using Discord.WebSocket;
using System.Text.Json;
using System.IO;
using System.Runtime.CompilerServices;

public class Config
{
    public string token { get; set; } = "INSERT_TOKEN_HERE";
    public UInt64 pizzatilaus_msg { get; set; };
    public UInt64 pizzatilaus_channel_id { get; set; };

    public UInt64 pizzatilaus_log_channel_id { get; set; };

}
public class Program
{
    private static Config config = new Config();
    private static readonly string CONFIGNAME = "config.json";
    private static DiscordSocketClient _client;

    public static async Task Main()
    {
        await Setup();

        _client = new DiscordSocketClient();

        _client.Log += Log;

        //  You can assign your bot token to a string, and pass that in to connect.
        //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.
        var token = config.token;

        // Some alternative options would be to keep your token in an Environment Variable or a standalone file.
        // var token = Environment.GetEnvironmentVariable("NameOfYourEnvironmentVariable");
        // var token = File.ReadAllText("token.txt");
        // var token = JsonConvert.DeserializeObject<AConfigurationClass>(File.ReadAllText("config.json")).Token;

        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();

        // Block this task until the program is closed.
        await Task.Delay(-1);
    }

    private static Task Setup()
    {
        //Check for Config file
        if (File.Exists("config.json"))
        {
            //If does, read for config
            string jsonstring = File.ReadAllText(CONFIGNAME);
            config = JsonSerializer.Deserialize<Config>(jsonstring);
        }
        else
        {
            //If doesn't exist, create one
            File.WriteAllText(CONFIGNAME, JsonSerializer.Serialize(config));
            Log(new LogMessage(LogSeverity.Critical, "Setup", "Created config file for you to be filled (:"));
            Environment.Exit(1);
        }
        return Task.CompletedTask;
    }

    private static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}