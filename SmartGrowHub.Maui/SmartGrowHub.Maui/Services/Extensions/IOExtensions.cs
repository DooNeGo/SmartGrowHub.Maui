using System.Diagnostics.CodeAnalysis;

namespace SmartGrowHub.Maui.Services.Extensions;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class IOExtensions
{
    public static Eff<T> ToEff<T>(this IO<T> io) => LanguageExt.Eff<T>.LiftIO(io);
    
    public static K<Eff, T> ToEff<T>(this K<IO, T> io) => LanguageExt.Eff<T>.LiftIO(io.As());

    public static OptionT<Eff, T> ToEff<T>(this OptionT<IO, T> transformer) => transformer.MapT(io => io.ToEff());

    public static OptionT<IO, T> ToIO<T>(this OptionT<Eff, T> transformer) => transformer.MapT(eff => eff.RunIO());
    
    public static IO<T> ToFailIO<T>(this OptionT<IO, T> transformer, Error error) =>
        transformer.Run().As().Bind(option => option.Match(
            Some: IO.pure,
            None: IO.fail<T>(error)));
    
    public static IO<T> ToFailIO<T>(this Option<T> option, Error error) =>
        option.Match(Some: IO.pure, None: IO.fail<T>(error));
    
    public static IO<A> TapOnFail<A, B>(this IO<A> io, Func<Error, IO<B>> func) =>
        io.IfFail(error => func(error).Bind(_ => IO.fail<A>(error)));
}