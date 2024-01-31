namespace BlazorApp2Test.Components
{
    public class ErrorService
    {
        public ErrorService() 
        {
        }

        public event Action<string> OnShow;
        public event Action OnClose;

        public void ShowModal(string message)
        {
            OnShow?.Invoke(message);
        }

        public void CloseModal()
        {
            OnClose?.Invoke();
        }
    }
}