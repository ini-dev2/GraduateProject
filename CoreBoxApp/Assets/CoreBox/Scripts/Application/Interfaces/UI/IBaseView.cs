namespace CoreBox
{
    internal interface IBaseView
    {
        public void OnSuccess(string message);
        public void OnError(string message);
    }
}