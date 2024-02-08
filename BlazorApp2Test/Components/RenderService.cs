using System;

namespace BlazorApp2Test.Components
{
    public class RenderService
    {

        public event Action? OnEventTriggered;

        public void TriggerEvent()
        {
            OnEventTriggered?.Invoke();
        }
    }
}
