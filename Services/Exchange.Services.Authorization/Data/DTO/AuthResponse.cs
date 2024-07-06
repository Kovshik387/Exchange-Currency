using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Services.Authorization.Data.DTO
{
    public class AuthResponse<TData>
    {
        public TData? Data { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;
    }
}
