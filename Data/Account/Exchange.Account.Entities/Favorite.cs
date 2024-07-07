using System;
using System.Collections.Generic;

namespace Exchange.Account.Context;

public partial class Favorite
{
    public int Id { get; set; }

    public string Volute { get; set; } = null!;

    public string Name { get; set; } = null!;

    public Guid? Iduser { get; set; }

    public virtual User? IduserNavigation { get; set; }
}
