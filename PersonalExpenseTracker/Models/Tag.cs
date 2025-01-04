using PersonalExpenseTracker.Models.Base;
using PersonalExpenseTracker.Models.Constant;

namespace PersonalExpenseTracker.Models;

public class Tag : BaseEntity<Guid>
{
    public string Name { get; set; }

    public string TextColor { get; set; }

    public string BackgroundColor { get; set; }

    public bool IsDefault {get; set;} 
}
