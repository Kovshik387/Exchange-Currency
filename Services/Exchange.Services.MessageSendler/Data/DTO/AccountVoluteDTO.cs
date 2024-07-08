using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exchange.Services.MessageSendler.Data.DTO;

public class AccountVoluteDTO
{
    public AccountDTO AccountModel { get; set; } = null!;
    public List<VoluteDTO> VoluteModel { get; set; } = new();
    public string Date { get; set; } = default!;
}
