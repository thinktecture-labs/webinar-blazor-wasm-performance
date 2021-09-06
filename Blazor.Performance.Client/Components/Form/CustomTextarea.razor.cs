using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazor.Performance.Client.Components.Form
{
    public partial class CustomTextarea
    {
        [Parameter] public string Label { get; set; }
        [Parameter] public bool Required { get; set; } = false;
        [Parameter] public bool PreventRendering { get; set; } = false;

        private int _valueHashCode;

        protected override bool ShouldRender()
        {
            if (PreventRendering)
            {
                var lastHashCode = _valueHashCode;
                _valueHashCode = Value?.GetHashCode() ?? 0;
                return _valueHashCode != lastHashCode;
            }

            return base.ShouldRender();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            Console.WriteLine($"Textarea {Label} rendered....");
            base.OnAfterRender(firstRender);
        }

        protected override bool TryParseValueFromString(string value, out string result,
            out string validationErrorMessage)
        {
            result = value;
            validationErrorMessage = null;
            return true;
        }
    }
}
