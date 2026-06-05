using System.Diagnostics.CodeAnalysis;

namespace SmartGrowHub.Maui.Services.Extensions;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class IOExtensions
{
    extension<T>(Option<T> option)
    {
        public IO<T> ToIO(Func<IO<T>> ifNone) =>
            option.Match(Some: IO.pure, None: ifNone);

        public IO<T> ToIOOrFail(Error error) =>
            option.ToIO(() => IO.fail<T>(error));

        public IO<T> ToIOOrFail(string errorMessage) =>
            option.ToIOOrFail(Error.New(errorMessage));
    }
    
    extension<T>(Fin<T> fin)
    {
        public IO<T> ToIO() => fin.Match(IO.pure, IO.fail<T>);
    }

    extension<T>(OptionT<IO, T> transformer) where T : notnull
    {
        public IO<T> ToIOOrFail(Error error) =>
            transformer.Run().Bind(option => option.ToIOOrFail(error)).As();

        public IO<T> ToIOOrFail(string errorMessage) =>
            transformer.ToIOOrFail(Error.New(errorMessage));
    }

    public static IO<A> TapOnFail<A, B>(this IO<A> io, Func<Error, IO<B>> func) =>
        io.IfFail(error => func(error)
            .Bind(_ => IO.fail<A>(error))
            .IfFail(innerError => error + innerError));
}