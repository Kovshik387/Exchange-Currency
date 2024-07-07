using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Services.EmailAction.Data
{
    public class ExchangeModel
    {
        public string date = null!;
        public List<VoluteModel> Volute { get; set; } = null!;
    }
}
