﻿using Ocelot.Authentication.Middleware;
using Ocelot.Authorisation.Middleware;
using Ocelot.Cache.Middleware;
using Ocelot.Claims.Middleware;
using Ocelot.DownstreamRouteFinder.Middleware;
using Ocelot.DownstreamUrlCreator.Middleware;
using Ocelot.Headers.Middleware;
using Ocelot.Middleware;
using Ocelot.Middleware.Pipeline;
using Ocelot.QueryStrings.Middleware;
using Ocelot.RateLimit.Middleware;
using Ocelot.Request.Middleware;
using Ocelot.RequestId.Middleware;
using Ocelot.Responder.Middleware;
using System;
using System.Threading.Tasks;
using Predicate = System.Func<Ocelot.Middleware.DownstreamContext, bool>;


public static class OcelotPipelineConfigurationExtensions
{

    public static OcelotPipelineConfiguration AddOrleansHttpGateway(this OcelotPipelineConfiguration config)
    {
        config.MapWhenOcelotPipeline.Add(builder => builder.AddOrleansHttpGateway(config));
        return config;
    }

    private static Predicate AddOrleansHttpGateway(this IOcelotPipelineBuilder builder, OcelotPipelineConfiguration pipelineConfiguration)
    {
        // Allow the user to respond with absolutely anything they want.
        builder.UseIfNotNull(pipelineConfiguration.PreErrorResponderMiddleware);

        // This is registered first so it can catch any errors and issue an appropriate response
        builder.UseResponderMiddleware();

        // Then we get the downstream route information
        builder.UseDownstreamRouteFinderMiddleware();

        // Now we have the ds route we can transform headers and stuff?
        builder.UseHttpHeadersTransformationMiddleware();

        // Initialises downstream request
        builder.UseDownstreamRequestInitialiser();

        // We check whether the request is ratelimit, and if there is no continue processing
        builder.UseRateLimiting();

        // This adds or updates the request id (initally we try and set this based on global config in the error handling middleware)
        // If anything was set at global level and we have a different setting at re route level the global stuff will be overwritten
        // This means you can get a scenario where you have a different request id from the first piece of middleware to the request id middleware.
        builder.UseRequestIdMiddleware();

        // Allow pre authentication logic. The idea being people might want to run something custom before what is built in.
        builder.UseIfNotNull(pipelineConfiguration.PreAuthenticationMiddleware);

        // Now we know where the client is going to go we can authenticate them.
        // We allow the ocelot middleware to be overriden by whatever the
        // user wants
        if (pipelineConfiguration.AuthenticationMiddleware == null)
        {
            builder.UseAuthenticationMiddleware();
        }
        else
        {
            builder.Use(pipelineConfiguration.AuthenticationMiddleware);
        }

        // The next thing we do is look at any claims transforms in case this is important for authorisation
        builder.UseClaimsBuilderMiddleware();

        // Allow pre authorisation logic. The idea being people might want to run something custom before what is built in.
        builder.UseIfNotNull(pipelineConfiguration.PreAuthorisationMiddleware);

        // Now we have authenticated and done any claims transformation we 
        // can authorise the request
        // We allow the ocelot middleware to be overriden by whatever the
        // user wants
        if (pipelineConfiguration.AuthorisationMiddleware == null)
        {
            builder.UseAuthorisationMiddleware();
        }
        else
        {
            builder.Use(pipelineConfiguration.AuthorisationMiddleware);
        }

        // Now we can run any header transformation logic
        builder.UseHttpRequestHeadersBuilderMiddleware();

        // Allow the user to implement their own query string manipulation logic
        builder.UseIfNotNull(pipelineConfiguration.PreQueryStringBuilderMiddleware);

        // Now we can run any query string transformation logic
        builder.UseQueryStringBuilderMiddleware();

        // This takes the downstream route we retrieved earlier and replaces any placeholders with the variables that should be used
        builder.UseDownstreamUrlCreatorMiddleware();

        // Not sure if this is the best place for this but we use the downstream url 
        // as the basis for our cache key.
        builder.UseOutputCacheMiddleware();

        //We fire off the request and set the response on the scoped data repo
        builder.UseOrleansRequesterMiddleware();


        return (context) =>
        {
            return context.DownstreamReRoute.DownstreamScheme.Equals("orleans", StringComparison.OrdinalIgnoreCase);
        };
    }

    private static void UseIfNotNull(this IOcelotPipelineBuilder builder,
         Func<DownstreamContext, Func<Task>, Task> middleware)
    {
        if (middleware != null)
        {
            builder.Use(middleware);
        }
    }
}

