using Microsoft.AspNetCore.Components;
using System;

namespace Blazor.Performance.Client.Components.Form
{
    public partial class CustomTextarea
    {
        [Parameter] public string Label { get; set; }
        [Parameter] public bool Required { get; set; } = false;


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
