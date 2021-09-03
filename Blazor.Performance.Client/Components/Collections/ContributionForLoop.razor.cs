using Blazor.Performance.Client.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Performance.Client.Components.Collections
{
    public partial class ContributionForLoop
    {
        [Parameter] public IEnumerable<Contribution> Contributions { get; set; }
        [Parameter] public string SearchTerm { get; set; }
        [Parameter] public bool Searching { get; set; }
        [Parameter] public EventCallback<int> ContributionClicked { get; set; }

        private void ContributionItemClicked(int id)
        {
            ContributionClicked.InvokeAsync(id);
        }
    }
}
