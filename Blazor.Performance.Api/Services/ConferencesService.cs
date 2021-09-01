using Blazor.Performance.Api.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Blazor.Performance.Api.Services
{
    public class ConferencesService
    {
        private static Root _root;

        public Task InitAsync()
        {
            return LoadDataAsync();
        }

        public async Task<List<Conference>> GetConferencesAsync()
        {
            await Task.Delay(250);
            return _root.Conferences;
        }

        public async Task<Conference> GetConferenceAsync(int id, CancellationToken cancellationToken)
        {
            var conf = _root?.Conferences.FirstOrDefault(c => c.Id == id);
            return conf;
        }

        private async Task LoadDataAsync()
        {
            var assembly = Assembly.GetEntryAssembly();
            var resourceStream = assembly?.GetManifestResourceStream("Blazor.Performance.Api.SampleData.conferences.json");
            using var reader = new StreamReader(resourceStream, Encoding.UTF8);
            var jsonString = await reader.ReadToEndAsync();
            _root = JsonSerializer.Deserialize<Root>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}