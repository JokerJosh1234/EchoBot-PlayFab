using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayFab.AdminModels;
using Discord;
using PlayFab;

namespace Bot.Playfab.Commands
{

    public class Ban : SlashCommand
    {
        public Ban()
        {
            //required
            command.Name = "pf-ban";
            command.Description = "Ban someone from the game (aka playfab)";

            //no one without Administrator perms can see the command or use it.
            command.WithDefaultMemberPermissions(GuildPermission.Administrator);

            //no one will be able to see or use this command in DM's
            command.WithDMPermission(false);
        }

        public override async void HandleExecute(SocketSlashCommand command)
        {
            var mb = new ModalBuilder()
                .WithTitle("Ban")
                .WithCustomId("ban:ban-menu")
                .AddTextInput("PlayFab Id", "playfab_id", TextInputStyle.Short, "0000000000000000", required: true)
                .AddTextInput("Reason", "ban_reason", TextInputStyle.Paragraph, "Bad at game", required: true)
                .AddTextInput("Length (Hours)", "ban_length", TextInputStyle.Short, "0", required: true);


            await command.RespondWithModalAsync(mb.Build());
        }

        public override async void OnModalSubmit(SocketModal arg)
        {
            SocketModalData model = arg.Data;

            PlayFabResult<BanUsersResult> result = await Playfab.AccountManagement.BanUser(new BanRequest
            {
                PlayFabId = model.Get("playfab_id"),
                Reason = model.Get("ban_reason"),
                DurationInHours = (uint)Convert.ToInt64(model.Get("ban_length"))
            });

            await Reply($"Banned {result.Result.BanData[0].PlayFabId}", ephemeral: true);
        }
    }

}
