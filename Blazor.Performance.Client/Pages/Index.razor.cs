using Blazor.Performance.Client.Components;
using Blazor.Performance.Client.Models;
using Blazor.Performance.Client.Services;
using Microsoft.AspNetCore.Components;
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

        private bool isLoading = false;
        private string searchTerm = String.Empty;
        private ICollection<Contribution> Contributions = new List<Contribution>();
        private ICollection<Contribution> allContributions = new List<Contribution>();

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            Contributions = await DataService.GetContributionsAsync();
            allContributions = Contributions;
            isLoading = false;
            await base.OnInitializedAsync();
        }

        private void SearchTermChanged(string term)
        {
            Contributions = String.IsNullOrWhiteSpace(term) 
                ? allContributions 
                : allContributions
                    .Where(c => c.Title.Contains(term))
                    .ToList();
            searchTerm = term;
        }

        private async Task ContributionClicked(int id)
        {
            var parameters = new DialogParameters();
            parameters.Add("Id", id);
            var dialogRef = DialogService.Show<EditContribution>("Simple Dialog", parameters);
            var result = await dialogRef.Result;
            if (!result.Cancelled)
            {
                Contributions = await DataService.GetContributionsAsync();
                StateHasChanged();
            }
        }
    }
}
