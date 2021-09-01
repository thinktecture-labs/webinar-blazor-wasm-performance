using System;
using System.Threading.Tasks;
using Blazor.Performance.Client.Models;
using Microsoft.AspNetCore.Components;

namespace Blazor.Performance.Client.Components
{
    public partial class TableRow
    {
        [Parameter] public Contribution Contribution { get; set; }
        [Parameter] public string SearchTerm { get; set; }
        [Parameter] public EventCallback ContributionClicked { get; set; }

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