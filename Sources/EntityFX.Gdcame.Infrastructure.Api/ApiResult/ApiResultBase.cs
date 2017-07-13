using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFX.Gdcame.Infrastructure.Api.ApiResult
{
    public class ApiResultBase
    {
    }

    public class ApiErrorResult : ApiResultBase
    {
        public string Message { get; set; }

        public ApiErrorData[] ErrorDetails { get; set; }
    }

    public class ApiErrorData
    {
        public int Code { get; set; }

        public string Message { get; set; }
    }
}
