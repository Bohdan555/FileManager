using System.ComponentModel;

namespace TransactionManager.Components.Domain.Enums
{   
    public enum Status
    {
        [Description("Approved")]
        Approved = 1,
        [Description("Done")]
        Done = 2,
        [Description("Failed")]
        Failed = 3,
        [Description("Finished")]
        Finished = 4,
        [Description("Rejected")]
        Rejected = 5
    }
}
