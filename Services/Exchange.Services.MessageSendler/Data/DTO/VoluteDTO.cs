using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Services.MessageSendler.Data.DTO;

public class VoluteDTO
{
    public string CharCode { get; set; } = default!;
    public int Nominal { get; set; }
    public string Name { get; set; } = default!;
    public decimal Value { get; set; } = default;
}
