using System;

public sealed class AccountNotFoundException : Exception
{
    public Guid LookUpId { get; private set; }

    public AccountNotFoundException(Guid id) : base(ToMessage(id))
    {
    }

    public AccountNotFoundException(Guid id, Exception inner) : base(ToMessage(id), inner)
    {
    }

    private static string ToMessage(Guid id)
    {
        return $"Account with id = '{id:N}' not found";
    }
}
