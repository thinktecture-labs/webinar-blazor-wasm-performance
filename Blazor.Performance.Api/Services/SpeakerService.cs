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
    public class SpeakerService
    {
        private static Root _root;

        public Task InitAsync()
        {
            return LoadDataAsync();
        }

        public async Task<List<Speaker>> GetSpeakersAsync()
        {
            return _root.Speaker;
        }

        public async Task<Speaker> GetSpeakerAsync(int id, CancellationToken cancellationToken)
        {
            var conf = _root?.Speaker.FirstOrDefault(c => c.Id == id);
            return conf;
        }

        private async Task LoadDataAsync()
        {
            var assembly = Assembly.GetEntryAssembly();
            var resourceStream = assembly?.GetManifestResourceStream("Blazor.Performance.Api.SampleData.speaker.json");
            using var reader = new StreamReader(resourceStream, Encoding.UTF8);
            var jsonString = await reader.ReadToEndAsync();
            _root = JsonSerializer.Deserialize<Root>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}