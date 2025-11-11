// See https://aka.ms/new-console-template for more information
using System.Security.AccessControl;
using Discord;
using Discord.WebSocket;
using System.Text.Json;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;
using Discord.Net;
using Newtonsoft.Json;

public class Config
{
    public string token { get; set; } = "INSERT_TOKEN_HERE";
    public UInt64 pizzatilaus_msg { get; set; }
    public UInt64 pizzatilaus_channel_id { get; set; }
    public UInt64 pizzatilaus_log_channel_id { get; set; }
}
public static class Program
{
    private static Config config = new Config();
    const string CONFIGNAME = "config.json";
    const ulong GUILDID = 1022557961203236976;
    public static DiscordSocketClient _client;

    // COMMAND CLASSES
    static TestCommands testCom;

    public static async Task Main()
    {
        await Setup();

        _client = new DiscordSocketClient();
        _client.Log += Log;

        _client.Ready += Client_Ready;
        _client.SlashCommandExecuted += SlashCommandHandler;

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

    public static async Task Client_Ready()
    {
        testCom = new TestCommands(GUILDID);
        await testCom.Ready();
    }

    public static async Task NukeCommands()
    {
        // NUKE ALL COMMANDS
        var guild = _client.GetGuild(GUILDID);
        await guild.DeleteApplicationCommandsAsync();

        var commands = await _client.GetGlobalApplicationCommandsAsync();
        foreach(var command in commands)
        {
            await command.DeleteAsync();
        }

        Console.WriteLine("NUKED ALL COMMANDS");
    }

    private static Task Setup()
    {
        //Check for Config file
        if (File.Exists(CONFIGNAME))
        {
            //If does, read for config
            string jsonstring = File.ReadAllText(CONFIGNAME);
            config = System.Text.Json.JsonSerializer.Deserialize<Config>(jsonstring);
        }
        else
        {
            //If doesn't exist, create one
            File.WriteAllText(CONFIGNAME, System.Text.Json.JsonSerializer.Serialize(config));
            Log(new LogMessage(LogSeverity.Critical, "Setup", "Created config file for you to be filled (:"));
            Environment.Exit(1);
        }
        return Task.CompletedTask;
    }

    private static async Task SlashCommandHandler(SocketSlashCommand command)
    {
        switch (command.Data.Name)
        {
            case TestCommands.name:
                await testCom.HandleCommand(command);
                break;
        }
    }

    private static Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }
}