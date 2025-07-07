using SICAF.Common.Models;

namespace SICAF.Common.Interfaces;

public interface IAuditContextProvider
{
    AuditContext GetCurrentContext();
}