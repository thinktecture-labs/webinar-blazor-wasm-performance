using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Blazor.Performance.Client.Models;
using Blazor.Performance.Client.Services;
using Microsoft.AspNetCore.Components;

namespace Blazor.Performance.Client.Components.Form
{
    public partial class SpeakerSelect
    {
        [Inject] public DataService DataService { get; set; }
        [Parameter] public HashSet<int> Values { get; set; }
        [Parameter] public EventCallback<HashSet<int>> ValuesChanged { get; set; }

        private int value { get; set; } = 0;
        private List<Speaker> _speakers = new();

        protected override async Task OnInitializedAsync()
        {
            _speakers = (await DataService.GetSpeakersAsync()).ToList();
            await base.OnInitializedAsync();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }

        private async Task SpeakersChanged(HashSet<int> values)
        {
            Values = values;
            await ValuesChanged.InvokeAsync(values);
        }
        
        private string GetMultiSelectionText(List<string> selectedValues)
        {
            if (selectedValues.Any())
            {
                var selectedSpeaker = _speakers.Where(s => selectedValues.Contains($"{s.Id}"));
                return String.Join(", ", selectedSpeaker.Select(s => $"{s.FirstName} {s.LastName}"));
            }
            return String.Empty;
        }
    }
}