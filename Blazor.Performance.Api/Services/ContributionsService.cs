using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Blazor.Performance.Api.Models;

namespace Blazor.Performance.Api.Services
{
    public class ContributionsService
    {
        private static Root _root;

        public Task InitAsync()
        {
            return LoadDataAsync();
        }

        public async Task<List<Contribution>> GetContributionsAsync(string searchTerm = "")
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return _root.Contributions;
            }
            return _root.Contributions.Where(c => c.Title.Contains(searchTerm, System.StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        public Contribution GetContribution(int id, CancellationToken cancellationToken)
        {
            var contribution = _root?.Contributions.FirstOrDefault(c => c.Id == id);
            return contribution;
        }

        public void AddContribution(Contribution contribution)
        {
            _root.Contributions.Add(contribution);
        }

        public void UpdateContribution(Contribution contribution)
        {
            _root.Contributions = _root.Contributions.Select((x, i) => x.Id == contribution.Id ? contribution : x).ToList();
        }

        public void RemoveContribution(int contributionId)
        {
            _root.Contributions = _root.Contributions.Where(c => c.Id == contributionId).ToList();
        }

        private async Task LoadDataAsync()
        {
            var assembly = Assembly.GetEntryAssembly();
            var resourceStream = assembly?.GetManifestResourceStream("Blazor.Performance.Api.SampleData.contributions.json");
            using var reader = new StreamReader(resourceStream, Encoding.UTF8);
            var jsonString = await reader.ReadToEndAsync();
            _root = JsonSerializer.Deserialize<Root>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var index = 1;
            _root.Contributions.ForEach(c => 
                {
                    c.Id = index;
                    index++;
                }
            );
        }
    }
}