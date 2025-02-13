using System.Diagnostics.CodeAnalysis;

namespace SmartGrowHub.Maui.Services.Extensions;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class IOExtensions
{
    public static Eff<T> ToEff<T>(this IO<T> io) => LanguageExt.Eff<T>.LiftIO(io);
    
    public static K<Eff, T> ToEff<T>(this K<IO, T> io) => LanguageExt.Eff<T>.LiftIO(io.As());

    public static OptionT<Eff, T> ToEff<T>(this OptionT<IO, T> transformer) => transformer.MapT(io => io.ToEff());

    public static OptionT<IO, T> ToIO<T>(this OptionT<Eff, T> transformer) => transformer.MapT(eff => eff.RunIO());
}