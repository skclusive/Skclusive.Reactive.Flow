using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Skclusive.Reactive.Flow
{
    public static class ReactiveFlowExtension
    {
        public static void TryAddReactiveFlowServices(this IServiceCollection services)
        {
            services.TryAddScoped<IActionStream, ActionStream>();

            services.TryAddScoped<IActionDispatcher>(sp => sp.GetRequiredService<IActionStream>());

            services.TryAddScoped<IActionObservable>(sp => sp.GetRequiredService<IActionStream>());

            services.TryAddScoped<IEpicFlow, EpicFlow>();

            services.TryAddScoped<IEffectFlow, EffectFlow>();

            services.TryAddScoped<IReactiveFlow, ReactiveFlow>();
        }
    }
}
