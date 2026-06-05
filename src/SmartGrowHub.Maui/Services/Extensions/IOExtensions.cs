using System.Diagnostics.CodeAnalysis;

namespace SmartGrowHub.Maui.Services.Extensions;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class IOExtensions
{
    public static IO<T> ToIO<T>(this Option<T> option, Func<IO<T>> ifNone) =>
        option.Match(Some: IO.pure, None: ifNone);

    public static IO<T> ToIOOrFail<T>(this Option<T> option, Error error) =>
        option.ToIO(() => IO.fail<T>(error));

    public static IO<T> ToIOOrFail<T>(this OptionT<IO, T> transformer, Error error) =>
        transformer.Run().Bind(option => option.ToIOOrFail(error)).As();
    
    public static IO<T> ToIOOrFail<T>(this Option<T> option, string errorMessage) =>
        option.ToIOOrFail(Error.New(errorMessage));

    public static IO<T> ToIOOrFail<T>(this OptionT<IO, T> transformer, string errorMessage) =>
        transformer.ToIOOrFail(Error.New(errorMessage));
    
    public static IO<A> TapOnFail<A, B>(this IO<A> io, Func<Error, IO<B>> func) =>
        io.IfFail(error => func(error)
            .Bind(_ => IO.fail<A>(error))
            .IfFail(innerError => error + innerError));
}