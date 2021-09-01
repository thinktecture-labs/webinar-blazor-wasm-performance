using Blazor.Performance.Client.Models;
using Blazor.Performance.Client.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;

namespace Blazor.Performance.Client.Components
{
    public partial class EditContribution
    {
        [Inject] public DataService DataService { get; set; }
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [Parameter] public int Id { get; set; }

        private Contribution _contribution;

        protected override async Task OnInitializedAsync()
        {
            _contribution = await DataService.GetContributionByIdAsync(Id);
            await base.OnInitializedAsync();
        }

        private void Submit()
        {
            DataService.UpdateContribution(_contribution);
            MudDialog.Close(DialogResult.Ok(true));
        }
        void Cancel() => MudDialog.Cancel();
    }
}
