using Cashify.Domain.Common.Base;
using Cashify.Domain.Common.Enum;

namespace Cashify.Domain.Models;

public class User : BaseEntity
{
    public string Username { get; set; }

    public string Password { get; set; }

    public Currency Currency { get; set; }
}