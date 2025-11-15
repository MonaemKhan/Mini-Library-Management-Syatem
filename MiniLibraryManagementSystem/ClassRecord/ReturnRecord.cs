using EnumClasses;

namespace ClassRecord
{
    public record ReturnRecord(
        dynamic Result,
        string Message,
        ResultStatus Status
        );
}
