using System.Collections;
using discord_bot;
using Discord;

namespace discord_bot;

public class Info : Program

{
    public async Task<(List<Message> records, ulong fromMessageId)> Getmessageinfo(ulong fromMessageId,
        IMessageChannel channel, bool order)
    {
        var records = new List<Message> { };
        IEnumerable<IMessage> messages;
        if (order == true)
        {
            messages = await channel.GetMessagesAsync(fromMessageId: fromMessageId, dir: Direction.Before)
                .FlattenAsync();
        }
        else
        {
            messages = await channel.GetMessagesAsync(1)
                .FlattenAsync();
        }

        await Task.Delay(500);
        int i = 0;
        foreach (var message in messages)
        {
            fromMessageId = message.Id;
            var author = message.Author;
            var timestamp = message.Timestamp;
            var attachments = message.Attachments;
            var attachmentlist = "";
            List<string> atList = new List<string>();
            foreach (var attachment in attachments)
            {
                atList.Add(attachment.ProxyUrl);
            }

            foreach (var attachment in atList)
            {
                attachmentlist = attachmentlist + "\n" + attachment + ",";
            }

            records.Add(new Message
            {
                _userID = author.Discriminator,
                _author = author.Username,
                _timestamp = timestamp,
                _content = message.Content,
                _attachment = attachmentlist
            });
            Console.WriteLine(
                $"User:{records[i]._author} Timestamp:{records[i]._timestamp} Message: {records[i]._content} ");

            i++;
        }


        return (records, fromMessageId);
    }
}