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
        private IEnumerable<Speaker> Speakers = new List<Speaker>();
        private IEnumerable<Contribution> Contributions = new List<Contribution>();
        private string searchTerm = String.Empty;
        private IEnumerable<Contribution> allContributions = new List<Contribution>();

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            Speakers = await DataService.GetSpeakersAsync();
            Contributions = await DataService.GetContributionsAsync();
            allContributions = Contributions;
            isLoading = false;
            await base.OnInitializedAsync();
        }

        private void SearchTermChanged(ChangeEventArgs eventArgs)
        {
            var value = eventArgs.Value.ToString();
            Contributions = String.IsNullOrWhiteSpace(value) ? allContributions : allContributions.Where(c => c.Title.Contains(value));
            searchTerm = value;
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
            }
        }
    }
}
