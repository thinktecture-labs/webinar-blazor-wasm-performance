using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Blazor.Performance.Client.Models;
using Blazor.Performance.Client.Utils;

namespace Blazor.Performance.Client.Services
{
    public class DataService
    {
        private readonly HttpClient _client;
        private static IEnumerable<Contribution> _contributions;
        private static IEnumerable<Speaker> _speakers;
        private static IEnumerable<Conference> _conferences;


        public DataService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<Contribution>> GetContributionsAsync(
            CancellationToken cancellationToken = default)
        {
            if (_contributions == null)
            {
                _contributions = await GetCollectionAsync<Contribution>("contributions", cancellationToken);
            }

            return _contributions;
        }

        public async Task<Contribution> GetContributionByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            if (_contributions == null)
            {
                _contributions = await GetCollectionAsync<Contribution>("contributions", cancellationToken);
            }

            return _contributions.FirstOrDefault(c => c.Id == id);
        }

        public void UpdateContribution(Contribution contribution)
        {
            _contributions = _contributions.Select((x, i) => x.Id == contribution.Id ? contribution : x);
        }
        
        public void RemoveContributionAsync(int contributionId)
        {
            _contributions = _contributions.Where(c => c.Id == contributionId);
        }

        public async Task<IEnumerable<Conference>> GetConferencesAsync(CancellationToken cancellationToken = default)
        {
            return await GetCollectionAsync<Conference>("conferences", cancellationToken);
        }

        public async Task<IEnumerable<Speaker>> GetSpeakersAsync(CancellationToken cancellationToken = default)
        {
            return await GetCollectionAsync<Speaker>("speakers", cancellationToken);
        }

        private async Task<IEnumerable<T>> GetCollectionAsync<T>(string path,
            CancellationToken cancellationToken = default)
        {
            var jsonString = await _client.GetStringAsync($"sample-data/{path}.json", cancellationToken);
            var result = JsonSerializer.Deserialize<ApiRootResult<T>>(jsonString,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (result == null)
            {
                return Enumerable.Empty<T>();
            }

            return result.Items;
        }
    }
}