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

        public DataService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<ICollection<Contribution>> GetContributionsAsync(
            CancellationToken cancellationToken = default)
        {
            if (_contributions == null)
            {
                _contributions = await GetCollectionAsync<Contribution>("contributions", cancellationToken);
            }

            return _contributions.ToList();
        }

        public async Task<Contribution> GetContributionByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            if (_contributions == null)
            {
                _contributions = await GetCollectionAsync<Contribution>("contributions", cancellationToken);
            }
            var c = _contributions.FirstOrDefault(c => c.Id == id);
            var cString = JsonSerializer.Serialize(c);

            return JsonSerializer.Deserialize<Contribution>(cString);
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
            if (_speakers == null)
            {
                _speakers = await GetCollectionAsync<Speaker>("speakers", cancellationToken);
            }

            return _speakers;
        }

        public async Task<string> GetSpeakerByContribution(int id, CancellationToken cancellationToken = default)
        {
            if (_speakers == null)
            {
                _speakers = await GetCollectionAsync<Speaker>("speakers", cancellationToken);
            }

            if (_contributions == null)
            {
                _contributions = await GetCollectionAsync<Contribution>("contributions", cancellationToken);
            }

            var contribution = _contributions.FirstOrDefault(c => c.Id == id);
            if (contribution == null)
            {
                return String.Empty;
            }

            var speakers = _speakers.Where(s => contribution.Speaker.Contains(s.Id));
            return String.Join(", ", speakers.OrderBy(s => s.FirstName).Select(s => $"{s.FirstName} {s.LastName}"));
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

            await Task.Delay(1000);
            return result.Items;
        }
    }
}