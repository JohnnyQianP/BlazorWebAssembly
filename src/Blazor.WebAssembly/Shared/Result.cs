using System;
using System.Collections.Generic;
using System.Text;

namespace Blazor.WebAssembly.Shared
{
    public class Result
    {
        public string Messages { get; set; }

        public bool Succeeded { get; set; } = true;
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }
    }
}
