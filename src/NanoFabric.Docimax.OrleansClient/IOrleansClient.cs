﻿using Orleans;
using System;
using System.Collections.Generic;
using System.Text;

namespace NanoFabric.Docimax.OrleansClient
{
    /// <summary>
    /// Orleans Client
    /// </summary>
    public interface IOrleansClient 
    {
        TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, string serviceId, AccessTokenType accessType = AccessTokenType.Default, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithGuidKey;

        TGrainInterface GetGrain<TGrainInterface>(long primaryKey, string serviceId, AccessTokenType accessType = AccessTokenType.Default, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithIntegerKey;

        TGrainInterface GetGrain<TGrainInterface>(string primaryKey, string serviceId, AccessTokenType accessType = AccessTokenType.Default, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithStringKey;

        TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, string keyExtension, string serviceId, AccessTokenType accessType = AccessTokenType.Default,string grainClassNamePrefix = null) where TGrainInterface : IGrainWithGuidCompoundKey;

        TGrainInterface GetGrain<TGrainInterface>(long primaryKey, string keyExtension, string serviceId, AccessTokenType accessType = AccessTokenType.Default,string grainClassNamePrefix = null) where TGrainInterface : IGrainWithIntegerCompoundKey;
    }
}
