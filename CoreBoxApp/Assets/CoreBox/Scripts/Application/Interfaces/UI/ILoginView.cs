using System;

namespace CoreBox
{
    internal interface ILoginView : IBaseView
    {
        public event Action<string, string> OnLoginPressed;
    }
}