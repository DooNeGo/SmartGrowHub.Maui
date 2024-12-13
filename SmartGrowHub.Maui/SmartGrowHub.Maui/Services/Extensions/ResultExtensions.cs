using SmartGrowHub.Shared.Results;

namespace SmartGrowHub.Maui.Services.Extensions;

public static class ResultExtensions
{
    public static Fin<Unit> ToFin(this Result result) =>
        result.Success ? FinSucc(unit)
            : FinFail<Unit>(Error.New(result.ErrorCode ?? 0, result.ErrorMessage ?? string.Empty));

    public static Eff<Unit> ToEff(this Result result) =>
        result.Success ? SuccessEff(unit)
            : FailEff<Unit>(Error.New(result.ErrorCode ?? 0, result.ErrorMessage ?? string.Empty));

    public static Fin<T> ToFin<T>(this Result<T> result) =>
        result.Success ? FinSucc(result.Data)
            : FinFail<T>(Error.New(result.ErrorCode ?? 0, result.ErrorMessage ?? string.Empty));

    public static Eff<T> ToEff<T>(this Result<T> result) =>
        result.Success ? SuccessEff(result.Data)
            : FailEff<T>(Error.New(result.ErrorCode ?? 0, result.ErrorMessage ?? string.Empty));
}
