using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imago.IO
{
    public enum ResultCode { ok, failed, unauthorized };

    public class Result<T> where T : class
    {
        public T Value { get; set; }
        public ResultCode Code { get; set; } = ResultCode.ok;
        public string Message { get; set; }
    }
}
