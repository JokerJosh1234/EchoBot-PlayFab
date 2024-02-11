#define Playfab_Integration
using PlayFab;
using PlayFab.AdminModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Playfab
{
    
    public static class Playfab
    {
        public const string TitleId = "";
        public const string X_SecretKey = "";
        public const string CurrencyCode = "";
        public const string Catalog = "";

        public static void Init()
        {
            PlayFabSettings.staticSettings.TitleId = TitleId;
            PlayFabSettings.staticSettings.DeveloperSecretKey = X_SecretKey;
        }

        public static class PlayerItemManagement
        {
            public static async Task<PlayFabResult<GetUserInventoryResult>> GetUserInventory(string PlayFabId) =>
                await PlayFabAdminAPI.GetUserInventoryAsync(new GetUserInventoryRequest { PlayFabId = PlayFabId });
            public static async Task<PlayFabResult<RevokeInventoryResult>> RevokeInventoryItem(string PlayFabId, string ItemInstanceId) =>
                await PlayFabAdminAPI.RevokeInventoryItemAsync(new RevokeInventoryItemRequest { PlayFabId = PlayFabId, ItemInstanceId = ItemInstanceId });

            // add your own stuff to suit your needs
        }

        public static class AccountManagement
        {
            public static async Task<PlayFabResult<BanUsersResult>> BanUser(BanRequest request) =>
                await PlayFabAdminAPI.BanUsersAsync(new BanUsersRequest { Bans = new List<BanRequest> { request } });

            public static async Task<PlayFabResult<GetUserBansResult>> GetUserBans(string PlayFabId) =>
                await PlayFabAdminAPI.GetUserBansAsync(new GetUserBansRequest { PlayFabId = PlayFabId });

            public static async Task<PlayFabResult<GetPlayerProfileResult>> GetPlayerProfile(string PlayFabId, PlayerProfileViewConstraints constraints = null) =>
                await PlayFabAdminAPI.GetPlayerProfileAsync(new GetPlayerProfileRequest { PlayFabId = PlayFabId, ProfileConstraints = constraints });

            public static async Task<PlayFabResult<RevokeAllBansForUserResult>> RevokeAllBansForUser(string PlayFabId) =>
                await PlayFabAdminAPI.RevokeAllBansForUserAsync(new RevokeAllBansForUserRequest { PlayFabId = PlayFabId });

            // add your own stuff to suit your needs
        }
    }
}
