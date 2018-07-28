﻿
using Microsoft.Extensions.Logging;
using Orleans.Authentication;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Orleans.Authorization
{
    public class AuthorizeGrainFiltered : IIncomingGrainCallFilter
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly ILogger Logger;
        private readonly IAuthenticationSchemeProvider Schemes;
        public const string UserPrincipalKey = "default_UserPrincipalKey";

        public AuthorizeGrainFiltered(ILogger<AuthorizeGrainFiltered> _logger, IAuthenticationSchemeProvider _schemes, IServiceProvider _services)
        {
            this.ServiceProvider = _services;
            this.Logger = _logger;
            this.Schemes = _schemes;
        }
        public async Task Invoke(IIncomingGrainCallContext context)
        {
            AuthorizeAttribute authAttr = this.FindAuthorizeAttribute(context);
            if (authAttr == null)
            {
                await context.Invoke();
                return;
            }

            //获取授权的方案
            var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticate == null)
            {
                await context.Invoke();
                return;
            }

            //授权处理
            var authContext = new AuthenticateContext(ServiceProvider);
            var result = await authContext.AuthenticateAsync(defaultAuthenticate.Name);
            if (result?.Succeeded != true)
            {
                this.Logger.LogError(result.Failure, result.Failure.Message);
                throw new AuthenticationException("Authorization Token is invalid");
            }

            //验证 Policy
            if (this.VerifyPolicy(context, result.Principal, authAttr))
                AuthorizeContext.User = result.Principal;

            await context.Invoke();
        }
        private AuthorizeAttribute FindAuthorizeAttribute(IIncomingGrainCallContext context)
        {
        
            //检查context.Grain必须继承 Grain
            if (!typeof(Grain).IsAssignableFrom(context.Grain.GetType()))
                return null;
            try
            {
                if (context.ImplementationMethod == null)
                    return null;
            }
            catch (Exception)
            {
                return null;
            }

            AuthorizeAttribute authAttr = context.ImplementationMethod.GetCustomAttribute<AuthorizeAttribute>();
            if (authAttr == null)
            {
                //判断方法是否允许匿名
                var allowAnonymous = context.ImplementationMethod.GetCustomAttribute<AllowAnonymousAttribute>();
                if (allowAnonymous != null)
                    return null;

                //获取类是否标示AuthorizeAttribute
                authAttr = context.ImplementationMethod.DeclaringType.GetCustomAttribute<AuthorizeAttribute>();
                if (authAttr == null)
                    return null;
            }
            this.Logger.LogDebug($"{context.ImplementationMethod.Name} Need authorization " );
            return authAttr;
        }


        private bool VerifyPolicy(IIncomingGrainCallContext context, ClaimsPrincipal principal, AuthorizeAttribute authAttr)
        {
            string policy = string.Empty;
            if (!string.IsNullOrEmpty(authAttr.Roles))
                policy = authAttr.Roles;
            else if (!string.IsNullOrEmpty(authAttr.Policy))
                policy = authAttr.Policy;

            if (!string.IsNullOrEmpty(policy))
            {
                var claim = principal.FindFirst( f=>f.Type == authAttr.Policy);
                if (claim == null)
                {
                    this.Logger.LogError("Authorization ClaimsPrincipal Does not contain " + policy);
                    throw new AuthenticationException("Authorization ClaimsPrincipal Does not contain " + policy);
                }
            }
            return true;
        }
       
         private object GetPrimaryKey(IAddressable grainReference)
        {
            var key = grainReference.GetPrimaryKeyString();
            if (key != null)
                return key;
            if (grainReference.IsPrimaryKeyBasedOnLong())
            {
                var key1 = grainReference.GetPrimaryKeyLong();
                if (key1 > 0)
                    return key1;
            }
            return grainReference.GetPrimaryKey();

        }
    }
}
