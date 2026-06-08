using Microsoft.Extensions.DependencyInjection;
using RentalManager.AI;
using RentalManager.Authorization;
using RentalManager.Conversation;
using RentalManager.Intents;
using RentalManager.Intents.Handlers;

namespace RentalManager.DI.AI
{
    public static class AIServicesExtensions
    {
        public static IServiceCollection AddAIServices(
            this IServiceCollection services)
        {
            // AI client
            services.AddScoped<IAIClient, OpenAIClient>();

            // Conversation system
            services.AddScoped<ConversationManager>();
            services.AddScoped<IConversationStore, InMemoryConversationStore>();

            // Intent system
            services.AddScoped<IIntentHandler, HelpIntentHandler>();
            services.AddScoped<IIntentHandler, GreetingIntentHandler>();
            services.AddScoped<IIntentHandler, ViewBalanceIntentHandler>();
            services.AddScoped<IIntentResolver, IntentResolver>();

            services.AddScoped<KeywordIntentResolver>();
            services.AddScoped<IntentAuthorizationService>();

            return services;
        }
    }
}