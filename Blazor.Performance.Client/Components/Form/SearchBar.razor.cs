using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace Blazor.Performance.Client.Components.Form
{
    public partial class SearchBar : IDisposable
    {
        [Inject] public IJSRuntime JS { get; set; }

        [Parameter] public string SearchTerm { get; set; }

        [Parameter] public EventCallback<string> SearchTermChanged { get; set; }


        private ElementReference _searchBarElement;
        private DotNetObjectReference<SearchBar> _selfReference;
        private int _valueHashCode;

        public void Dispose() => _selfReference?.Dispose();

        [JSInvokable]
        public void HandleOnInput(string value)
        {
            Console.WriteLine($"TextChanged {SearchTerm}. JS Value {value}");
            if (SearchTerm != value)
            {
                SearchTermChanged.InvokeAsync(value);
                StateHasChanged();
            }
        }

        protected override bool ShouldRender()
        {
            var lastHashCode = _valueHashCode;
            _valueHashCode = SearchTerm?.GetHashCode() ?? 0;
            return _valueHashCode != lastHashCode;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _selfReference = DotNetObjectReference.Create(this);
                // Only notify every 500 ms
                await JS.InvokeVoidAsync("onDebounceInput",
                    _searchBarElement, _selfReference, 500);
            }
        }
    }
}
