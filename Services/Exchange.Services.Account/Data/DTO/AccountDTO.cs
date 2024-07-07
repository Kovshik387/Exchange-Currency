﻿namespace Exchange.Services.Account.Data.DTO;

public class AccountDTO
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string Email { get; set; } = null!;
    public bool Accept { get; set; }

    public ICollection<FavoriteDTO> Favorites { get; set; } = null!;
}
