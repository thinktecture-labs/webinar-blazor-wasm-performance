using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Blazor.Performance.Client.Components
{
    public partial class SearchBar
    {
        [Inject] public IJSRuntime JS { get; set; }

        [Parameter] public string SearchTerm { get; set; }

        [Parameter] public EventCallback<string> SearchTermChanged { get; set; }       

        private void InputChanged(ChangeEventArgs eventArgs)
        {
            SearchTerm = eventArgs.Value.ToString();
            SearchTermChanged.InvokeAsync(SearchTerm);
        }
    }
}
