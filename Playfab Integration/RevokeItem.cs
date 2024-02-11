using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayFab.AdminModels;
using PlayFab;
using System.Diagnostics;



namespace Bot.Playfab.Commands
{
    public class RevokeItem : SlashCommand
    {
        public RevokeItem()
        {
            command.Name = "pf-remove-item";
            command.Description = "Remove item from players inventory";

            command.WithDefaultMemberPermissions(GuildPermission.Administrator);

            command.AddOption("playfabid", ApplicationCommandOptionType.String, "Player that you want to remove an item from", true);

            command.WithDMPermission(false);
        }

        public override async void HandleExecute(SocketSlashCommand command)
        {
            PlayFabResult<GetUserInventoryResult> inventory = await Playfab.PlayerItemManagement.GetUserInventory((string)command.GetOption("playfabid").Value);

            if (inventory.Result.Inventory.Count < 1)
            {
                await command.RespondAsync("No items to remove", ephemeral: true);
                return;
            }

            var componentBuilder = new ComponentBuilder();
            SelectMenuBuilder menuBuilder = null;
            int itemCount = 0;
            int menuCount = 0;

            foreach (ItemInstance item in inventory.Result.Inventory)
            {
                if (itemCount % 25 == 0)
                {
                    if (menuBuilder != null)
                        componentBuilder.WithSelectMenu(menuBuilder);

                    menuBuilder = new SelectMenuBuilder()
                        .WithPlaceholder($"Select an item (Page {++menuCount})")
                        .WithCustomId($"inventory-revoke-select-{menuCount}")
                        .WithMinValues(1)
                        .WithMaxValues(1);
                }

                menuBuilder.AddOption(item.DisplayName, $"{item.ItemId}:{item.ItemInstanceId}", item.DisplayName);
                itemCount++;
            }

            if (menuBuilder != null)
                componentBuilder.WithSelectMenu(menuBuilder);

            await Reply("Select an item to remove", components: componentBuilder.Build(), ephemeral: true);
        }

        public override async void OnSelectMenuExecute(SocketMessageComponent arg)
        {
            await Playfab.PlayerItemManagement.RevokeInventoryItem(arg.Data.Values.First().Split(':')[0], arg.Data.Values.First().Split(':')[1]);

            await Reply($"Revoked item!", ephemeral: true);
        }
    }

}