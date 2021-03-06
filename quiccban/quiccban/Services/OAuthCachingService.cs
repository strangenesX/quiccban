﻿using Discord;
using Discord.Rest;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace quiccban.Services
{
    public class OAuthCachingService
    {
        private MemoryCache cache;

        public OAuthCachingService()
        {
            cache = new MemoryCache(new MemoryCacheOptions { });
        }

        public async Task<DiscordRestClient> GetOrCreateClient(string accessToken) =>
            await cache.GetOrCreateAsync(accessToken, async factory =>
            {
                factory.AbsoluteExpiration = DateTimeOffset.UtcNow.AddDays(1);

                var restClient = new DiscordRestClient();

                await restClient.LoginAsync(TokenType.Bearer, accessToken);

                return restClient;

            });

        public DiscordRestClient Remove(string accessToken)
        {
            if (cache.TryGetValue(accessToken, out DiscordRestClient client))
            {
                cache.Remove(accessToken);
                return client;
            }
            return null;

        }



    }
}
