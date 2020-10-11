using Multicad.Text.Embedding;
using System;

namespace CatenaryCAD.Models
{
    public enum HandlerMessages : byte
    {
        TryModify, Update, Remove
    }

    public abstract partial class Model : IModel
    {
        internal event Func<bool> TryModify;
        internal event Func<bool> Update;
        internal event Func<bool> Remove;

        public bool SendMessageToHandler(HandlerMessages message)
        {
            switch (message)
            {
                case HandlerMessages.TryModify:
                    return TryModify.Invoke();

                case HandlerMessages.Update:
                    return Update.Invoke();

                case HandlerMessages.Remove:
                    return Remove.Invoke();

                default:
                    throw new NotImplementedException();
            }
        }

    }
}