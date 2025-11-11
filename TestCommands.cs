using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;

public class TestCommands
{
    public TestCommands(ulong guildId)
    {
        _guildId = guildId;
    }
    private ulong _guildId;
    public const string name = "ping";

    public async Task Ready()
    {
        // Test slash commands
        var guild = Program._client.GetGuild(_guildId);
        var guildCommand = new SlashCommandBuilder();
        guildCommand.WithName(name);
        guildCommand.WithDescription("Guild Test Command");

        try
        {
            await guild.CreateApplicationCommandAsync(guildCommand.Build());
        }
        catch (HttpException ex)
        {
            var json = JsonConvert.SerializeObject(ex.Errors, Formatting.Indented);
            Console.WriteLine(json);
        }
    }

    public async Task HandleCommand(SocketSlashCommand command)
    {
        await command.RespondAsync("Pong!");
    }
}