﻿using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Victoria.Helpers;
using Victoria.Entities;

namespace Victoria
{
    public sealed class LavaRestClient
    {
        private readonly (string Host, int Port, string Password) _rest;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeSettings"></param>
        public LavaRestClient(string host = default, int? port = default, string password = default)
        {
            _rest.Host = host ?? "127.0.0.1";
            _rest.Port = port ?? 2333;
            _rest.Password = password ?? "youshallnotpass";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Task<SearchResult> SearchSoundcloudAsync(string query)
        {
            return TracksRequestAsync($"scsearch:{query}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Task<SearchResult> SearchYoutubeAsync(string query)
        {
            return TracksRequestAsync($"ytsearch:{query}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<SearchResult> TracksRequestAsync(string query)
        {
            var url = $"http://{_rest.Host}:{_rest.Port}/loadtracks?identifier={WebUtility.UrlEncode(query)}";
            var request = await HttpHelper.Instance
                .WithCustomHeaders(("Authorization", _rest.Password))
                .GetStringAsync(url).ConfigureAwait(false);
            return JsonConvert.DeserializeObject<SearchResult>(request);
        }
    }
}