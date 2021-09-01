using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Performance.Client.Components.Form
{
    public partial class CustomInput
    {
        [Parameter] public string Label { get; set; }
        [Parameter] public bool Required { get; set; } = false;


        protected override void OnAfterRender(bool firstRender)
        {
            Console.WriteLine($"TextBox {Label} rendered....");
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
