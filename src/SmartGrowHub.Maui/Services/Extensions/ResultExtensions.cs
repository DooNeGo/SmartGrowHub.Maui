using System.Diagnostics.CodeAnalysis;
using SmartGrowHub.Shared.Results;

namespace SmartGrowHub.Maui.Services.Extensions;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class ResultExtensions
{
    public static Fin<Unit> ToFin(this Result result) =>
        result.IsSuccess ? Fin<Unit>.Succ(Unit.Default)
            : Fin<Unit>.Fail(Error.New(result.ErrorCode ?? 0, result.ErrorMessage ?? string.Empty));

    public static Eff<Unit> ToEff(this Result result) =>
        result.IsSuccess ? Eff<Unit>.Pure(Unit.Default)
            : Eff<Unit>.Fail(Error.New(result.ErrorCode ?? 0, result.ErrorMessage ?? string.Empty));
    
    public static IO<Unit> ToIO(this Result result) =>
        result.IsSuccess ? IO.pure(Unit.Default)
            : IO.fail<Unit>(Error.New(result.ErrorCode ?? 0, result.ErrorMessage ?? string.Empty));

    public static Fin<T> ToFin<T>(this Result<T> result) =>
        result.IsSuccess
            ? result.Data is not null
                ? Fin<T>.Succ(result.Data)
                : Fin<T>.Fail(Error.New("Null response error"))
            : Fin<T>.Fail(Error.New(result.ErrorCode ?? 0, result.ErrorMessage ?? string.Empty));

    public static Eff<T> ToEff<T>(this Result<T> result) =>
        result.IsSuccess
            ? result.Data is not null
                ? Eff<T>.Pure(result.Data)
                : Eff<T>.Fail(Error.New("Null response error"))
            : Eff<T>.Fail(Error.New(result.ErrorCode ?? 0, result.ErrorMessage ?? string.Empty));

    public static OptionT<IO, T> ToOptionTIO<T>(this Result<T> result) =>
        result.IsSuccess
            ? result.Data is not null
                ? OptionT<IO, T>.Some(result.Data)
                : OptionT<IO, T>.None
            : OptionT<IO, T>.LiftIO(IO.fail<T>(Error.New(result.ErrorCode ?? 0, result.ErrorMessage ?? string.Empty)));
}
