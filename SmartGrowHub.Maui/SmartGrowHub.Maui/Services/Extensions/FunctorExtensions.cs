using System.Diagnostics.CodeAnalysis;

namespace SmartGrowHub.Maui.Services.Extensions;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class FunctorExtensions
{
    public static K<F, Unit> ToUnit<F, A>(this K<F, A> fa) where F : Functor<F> => fa.Map(_ => Unit.Default);

    public static IO<Unit> ToUnit<T>(this IO<T> io) => io.Kind().ToUnit().As();
    
    public static Eff<Unit> ToUnit<T>(this Eff<T> eff) => eff.Kind().ToUnit().As();
}