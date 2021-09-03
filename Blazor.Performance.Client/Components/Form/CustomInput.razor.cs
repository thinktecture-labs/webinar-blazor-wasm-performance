using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.Performance.Client.Components.Form
{
    public partial class CustomInput : IHandleEvent
    {
        [Parameter] public string Label { get; set; }
        [Parameter] public bool Required { get; set; } = false;
        [Parameter] public bool PreventRendering { get; set; } = true;

        private bool _preventRender;
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

        public Task HandleEventAsync(EventCallbackWorkItem item, object arg)
        {
            try
            {
                var task = item.InvokeAsync(arg);
                var shouldAwaitTask = task.Status != TaskStatus.RanToCompletion &&
                                      task.Status != TaskStatus.Canceled;

                if (!_preventRender)
                {
                    StateHasChanged();
                }

                return shouldAwaitTask
                    ? CallStateHasChangedOnCompletionAsync(task, _preventRender)
                    : Task.CompletedTask;
            }
            finally
            {
                _preventRender = false;
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            Console.WriteLine($"TextBox {Label} rendered....");
            base.OnAfterRender(firstRender);
        }

        private async Task CallStateHasChangedOnCompletionAsync(Task task, bool supressRender)
        {
            try
            {
                await task;
            }
            catch
            {
                if (task.IsCanceled)
                {
                    return;
                }

                throw;
            }

            if (!supressRender)
            {
                StateHasChanged();
            }
        }

        private void PreventRender()
        {
            _preventRender = true;
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
