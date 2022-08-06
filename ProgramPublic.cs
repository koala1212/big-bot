using System.Globalization;
using CsvHelper;
using Discord;
using Discord.Net;
using Discord.WebSocket;

namespace discord_bot;

public class Program
{
    public static Task Main(string[] args) => new Program().MainAsync();

    private DiscordSocketClient _client;

    public class Message
    {
        public string _author { get; set; }
        public string _userID { get; set; }
        public DateTimeOffset _timestamp { get; set; }
        public string _attachment { get; set; }
        public string _content { get; set; }
    }

    public async Task MainAsync()
    {
        _client = new DiscordSocketClient();
        _client.Log += Log;


        var token = "TYPE_BOT_TOKEN_HERE";


        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();
        _client.Ready += _client_Ready;
        Console.WriteLine(_client.ConnectionState);


        await Task.Delay(-1);
    }

    private Task Log(LogMessage msg)
    {
        Console.WriteLine(msg.ToString());
        return Task.CompletedTask;
    }

    private async Task _client_Ready()
    {
        var channel = _client.GetChannel(CHANNEL ID HERE) as IMessageChannel;
        ulong fromMessageId = 0;

        var frommessage = channel.GetMessagesAsync(1).FlattenAsync().Result;

        var records = new List<Message> { };
        foreach (var message in frommessage)
        {
            fromMessageId = message.Id;
            break;
        }

        await new Info().Getmessageinfo(fromMessageId, channel, false);
        bool go = true;
        while (go)
        {
            try
            {
                var output = await new Info().Getmessageinfo(fromMessageId, channel, true);
                fromMessageId = output.fromMessageId;
                foreach (var outputRecord in output.records) records.Add(outputRecord);

                await using var writer = new StreamWriter("PATH TO CSV FILE");
                await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                await csv.WriteRecordsAsync(records);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message, Console.BackgroundColor == ConsoleColor.DarkRed);
                go = false;
            }
        }
    }
}