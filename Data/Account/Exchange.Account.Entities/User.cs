using System;
using System.Collections.Generic;

namespace Exchange.Account.Context;

public partial class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string Email { get; set; } = null!;
    public bool Accept { get; set; }
    public Guid  IdPhoto { get; set; }
    
    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
}
