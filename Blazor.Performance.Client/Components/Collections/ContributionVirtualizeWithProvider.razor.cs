using Blazor.Performance.Client.Models;
using Blazor.Performance.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Performance.Client.Components.Collections
{
    public partial class ContributionVirtualizeWithProvider
    {
        [Inject] public DataService DataService { get; set; }
        [Parameter] public string SearchTerm { get; set; }
        [Parameter] public bool Searching { get; set; }
        [Parameter] public EventCallback<int> ContributionClicked { get; set; }

        private Virtualize<Contribution> _virtualize;
        private string _searchTerm;

        protected override async Task OnParametersSetAsync()
        {
            if (_searchTerm != SearchTerm)
            {
                _searchTerm = SearchTerm;
                await ReloadContributions();
            }

            await base.OnParametersSetAsync();
        }

        public async Task ReloadContributions()
        {
            if (_virtualize != null)
            {
                await _virtualize?.RefreshDataAsync();
            }
        }

        private async ValueTask<ItemsProviderResult<Contribution>> LoadContributions(
            ItemsProviderRequest request)
        {
            try
            {
                var count = await DataService.GetContributionCountAsync(SearchTerm, request.CancellationToken);

                var numContributions = Math.Min(request.Count, count - request.StartIndex);
                var contributions =
                    await DataService.GetContributionsAsync(SearchTerm, request.StartIndex, numContributions,
                        request.CancellationToken);
                return new ItemsProviderResult<Contribution>(contributions, count);
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine("Current request was canceled.");
                return new ItemsProviderResult<Contribution>(new List<Contribution>(), 0);
            }
        }

        private void ContributionItemClicked(int id)
        {
            ContributionClicked.InvokeAsync(id);
        }
    }
}
