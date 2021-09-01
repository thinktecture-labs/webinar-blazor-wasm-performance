using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Blazor.Performance.Client.Components
{
    public partial class CustomTextarea : IHandleEvent
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

        protected override void OnParametersSet()
        {
            PreventRender();
            base.OnParametersSet();
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
