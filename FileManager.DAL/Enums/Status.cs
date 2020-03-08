using System.ComponentModel;

namespace TransactionManager.DAL.Enums
{
    public enum Status
    {
        [Description("Approved")]
        Approved = 1,
        [Description("Failed")]
        Failed = 2,
        [Description("Finished")]
        Finished = 3,
        [Description("Rejected")]
        Rejected = 4,
        [Description("Done")]
        Done = 5
    }
}
