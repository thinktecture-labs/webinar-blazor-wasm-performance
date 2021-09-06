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

        private int currentContributionId;
        private string currentContributionTitle;
        private string currentSearchTerm;
        private bool shouldRender;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        protected override void OnParametersSet()
        {
            shouldRender = Contribution.Id != currentContributionId || Contribution.Title != currentContributionTitle
                || SearchTerm != currentSearchTerm;

            currentContributionId = Contribution.Id;
            currentContributionTitle = Contribution.Title;
            currentSearchTerm = SearchTerm;
        }

        protected override bool ShouldRender() => true;

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