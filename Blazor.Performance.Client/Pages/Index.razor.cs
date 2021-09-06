using Blazor.Performance.Client.Components.Collections;
using Blazor.Performance.Client.Components.Form;
using Blazor.Performance.Client.Models;
using Blazor.Performance.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Performance.Client.Pages
{
    public partial class Index
    {
        [Inject] public DataService DataService { get; set; }
        [Inject] public IDialogService DialogService { get; set; }

        private int activeTabIndex = 0;
        private bool isLoading = false;
        private bool searching = false;
        private string searchTerm = String.Empty;
        private ICollection<Contribution> Contributions = new List<Contribution>();
        private ContributionVirtualizeWithProvider _virtualize;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            Contributions = await DataService.GetContributionsAsync();
            isLoading = false;
            await base.OnInitializedAsync();
        }

        private async Task SearchTermChanged(string term)
        {
            searching = true;
            Contributions = (await DataService.GetContributionsAsync(term)).ToList();
            searchTerm = term;
            searching = false;
        }

        private void ActiveIndexChanged(int index)
        {
            activeTabIndex = index;
        }

        private async Task ContributionClicked(int id)
        {
            var parameters = new DialogParameters();
            parameters.Add("Id", id);
            var dialogRef = DialogService.Show<EditContribution>("Simple Dialog", parameters);
            var result = await dialogRef.Result;
            if (!result.Cancelled && result.Data is Contribution con)
            {
                if (activeTabIndex <= 1)
                {
                    Contributions = await DataService.GetContributionsAsync();
                }
                else
                {
                    await _virtualize.ReloadContributions();
                }
                StateHasChanged();
            }
        }
    }
}
