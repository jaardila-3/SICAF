namespace SICAF.Data.Interfaces;

public interface IAuditable
{
    string GetAuditDescription();
    string GetModule();
    string GetSubModule();
}