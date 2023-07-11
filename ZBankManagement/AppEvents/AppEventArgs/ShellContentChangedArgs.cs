using System;

namespace ZBank.AppEvents
{
    public class ShellContentChangedArgs
    {
        public Type PageType { get; set; }

        public object Params { get; set; }
    }
}