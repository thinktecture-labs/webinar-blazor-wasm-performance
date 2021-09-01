using System;
using System.Threading.Tasks;
using Blazor.Performance.Client.Models;
using Blazor.Performance.Client.Services;
using Microsoft.AspNetCore.Components;

namespace Blazor.Performance.Client.Components
{
    public partial class TableRow
    {
        [Inject] public DataService DataService { get; set; }
        [Parameter] public Contribution Contribution { get; set; }
        [Parameter] public string SearchTerm { get; set; }
        [Parameter] public EventCallback ContributionClicked { get; set; }

        private string speakers = String.Empty;

        protected override async Task OnInitializedAsync()
        {
            speakers = await DataService.GetSpeakerByContribution(Contribution.Id);
            await base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            speakers = await DataService.GetSpeakerByContribution(Contribution.Id);
            await base.OnParametersSetAsync();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            Console.WriteLine($"Row {Contribution.Id} rendered");
            base.OnAfterRender(firstRender);
        }

        private async Task RowClicked()
        {
            await ContributionClicked.InvokeAsync();
        }
    }
}