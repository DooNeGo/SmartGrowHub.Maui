using System.Diagnostics.CodeAnalysis;
using SmartGrowHub.Shared.Results;

namespace SmartGrowHub.Maui.Services.Extensions;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class ResultExtensions
{
    extension(Result result)
    {
        public Fin<Unit> ToFin() =>
            result.IsSuccess
                ? Fin.Succ(Unit.Default)
                : Fin.Fail<Unit>(Error.New(result.ErrorCode ?? 0, result.ErrorMessage ?? string.Empty));

        public IO<Unit> ToIO() =>
            result.IsSuccess
                ? IO.pure(Unit.Default)
                : IO.fail<Unit>(Error.New(result.ErrorCode ?? 0, result.ErrorMessage ?? string.Empty));
    }

    extension<T>(Result<T> result)
    {
        public Fin<T> ToFin() =>
            result.IsSuccess
                ? result.Data is not null
                    ? Fin.Succ(result.Data)
                    : Fin.Fail<T>(Error.New("Null response error"))
                : Fin.Fail<T>(Error.New(result.ErrorCode ?? 0, result.ErrorMessage ?? string.Empty));

        public OptionT<IO, T> ToOptionTIO() =>
            result.IsSuccess
                ? result.Data is not null
                    ? OptionT.Some<IO, T>(result.Data)
                    : OptionT<IO, T>.None
                : OptionT.lift(IO.fail<T>(Error.New(result.ErrorCode ?? 0, result.ErrorMessage ?? string.Empty)));
        
        public IO<T> ToIO() =>
            result.IsSuccess
                ? result.Data is not null
                    ? IO.pure(result.Data)
                    : IO.fail<T>("Data was null")
                : IO.fail<T>(Error.New(result.ErrorCode ?? 0, result.ErrorMessage ?? "Unsuccessful response"));
    }
}
