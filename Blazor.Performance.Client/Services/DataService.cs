using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Blazor.Performance.Client.Models;

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

        public async Task<ICollection<Contribution>> GetContributionsAsync(string searchTerm = "", int skip = 0, int take = Int32.MaxValue,
            CancellationToken cancellationToken = default)
        {
            return (await GetCollectionAsync<Contribution>($"contributions?searchTerm={searchTerm}&skip={skip}&take={take}", cancellationToken)).ToList();
        }

        public async Task<int> GetContributionCountAsync(string searchTerm = "", CancellationToken cancellationToken = default)
        {
            var request = new HttpRequestMessage(HttpMethod.Head, $"contributions?searchTerm={searchTerm}");

            using var httpResponse = await _client.SendAsync(request, cancellationToken);
            var countHeader = httpResponse.Headers.GetValues("X-Contribution-Count")?.FirstOrDefault();
            var count = 0;
            if (!string.IsNullOrWhiteSpace(countHeader))
            {
                int.TryParse(countHeader, out count);
            }
            Console.WriteLine($"ContributionCount: {count}");
            return count;
        }

        public async Task<Contribution> GetContributionByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"Load Contribution for Id: {id}");
            return await _client.GetFromJsonAsync<Contribution>($"contributions/{id}", cancellationToken);
        }

        public Task UpdateContribution(Contribution contribution, CancellationToken cancellationToken = default)
        {
            return _client.PutAsJsonAsync<Contribution>($"contributions/{contribution.Id}", contribution, cancellationToken);
        }
        
        public Task RemoveContributionAsync(int contributionId, CancellationToken cancellationToken = default)
        {
            return _client.DeleteAsync($"contributions/{contributionId}", cancellationToken);
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
            var result = await _client.GetFromJsonAsync<IEnumerable<T>>($"{path}", cancellationToken);
            return result;
        }
    }
}