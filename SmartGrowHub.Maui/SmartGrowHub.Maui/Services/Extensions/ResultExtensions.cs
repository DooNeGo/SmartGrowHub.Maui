using SmartGrowHub.Shared.Results;

namespace SmartGrowHub.Maui.Services.Extensions;

public static class ResultExtensions
{
    public static Fin<Unit> ToFin(this Result result) =>
        result.IsSuccess ? FinSucc(unit)
            : FinFail<Unit>(Error.New(result.ErrorCode ?? 0, result.ErrorMessage ?? string.Empty));

    public static Eff<Unit> ToEff(this Result result) =>
        result.IsSuccess ? SuccessEff(unit)
            : FailEff<Unit>(Error.New(result.ErrorCode ?? 0, result.ErrorMessage ?? string.Empty));

    public static Fin<T> ToFin<T>(this Result<T> result) =>
        result.IsSuccess
            ? result.Data is not null
                ? FinSucc(result.Data)
                : FinFail<T>(Error.New("Null response error"))
            : FinFail<T>(Error.New(result.ErrorCode ?? 0, result.ErrorMessage ?? string.Empty));

    public static Eff<T> ToEff<T>(this Result<T> result) =>
        result.IsSuccess
            ? result.Data is not null
                ? SuccessEff(result.Data)
                : FailEff<T>(Error.New("Null response error"))
            : FailEff<T>(Error.New(result.ErrorCode ?? 0, result.ErrorMessage ?? string.Empty));
}
