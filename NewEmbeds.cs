//For Warn 
var eb = new EmbedBuilder();
eb.WithAuthor("Amethyst Network", "imgURL", null);
eb.WithDescription($"{request.OriginalMessage.Author.Mention} has warned {user.Id.Mention}.");
eb.AddField("Reason", reason);
await SendEmbedToLogChannel(gid, eb);

var ebdm = new EmbedBuilder();
ebdm.WithAuthor("Amethyst Network", "imgURL", null);
ebdm.WithDescription("You have been issued a warning in our server. Please read the rules to prevent future offenses. More information this warn can be found below.\n\n");
ebdm.AddInlineField("Issuer", request.OriginalMessage.Author.Mention);
ebdm.AddInlineField("Date", date);
ebdm.AddField("Reason", reason);
IDMChannel dm = await (user as SocketUser).GetOrCreateDMChannelAsync();
dm.SendMessageAsync("", false, ebdm);
