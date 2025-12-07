using System;

namespace CoreBox
{
    internal interface IRegisterView : IBaseView
    {
        public event Action<string, string, string> OnRegisterPressed;
    }
}