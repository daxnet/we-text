// ---------------------------------------------------------------------------------------------------------------                                                                                       
//                                                                                                    
//     XNSPZ.    qXFXZ:   LPPN@:             N0kXSk5kSPkPqM:                                 .        
//     @B@B@.   B@B@B@7   B@B@B             r@@@B@B@B@B@B@B                              UM@@@        
//     B@B@B   G@B@B@B:  B@B@@              uBMEqB@B@@E8MMS                              B@@@J        
//     BB@B@  L@B@O@@@: :@@@B    r8@B@B@Mi       @B@B5       :P@B@B@BY  L@B@BB   @B@B@u@B@B@B@B@      
//     G@B@B  @@@iL@@B, @B@B.  :@B@B2j@B@@i     EB@B@.      @B@BN7@@@BX  @@B@B  @B@BO O@@B@B@B@E      
//     0B@BN @B@M J@B@ rB@B7  ,@B@B. :B@@@:     B@B@B      @@@B7  B@B@S   B@B@M@B@B,   :B@B@r         
//     F@B@2E@@B  UB@BvB@Bq   @B@B@B@B@BO:     r@B@@L     XB@B@B@B@BB7    vB@B@B@B     F@B@B          
//     5@@B@B@B.  v@B@B@B@    B@B@O            @B@B@      M@B@B:         Z@@B@B@B@i    @B@BM          
//     2@B@B@B;   vB@B@B@     2B@@@J:iPBZ     .B@@@@      :B@@@q:i2B@  7@B@BB @@@B@.  .B@B@B@B        
//     uB@B@@X    r@B@B@,      rB@B@@@B@.     X@B@Br       ,M@B@B@B@; M@B@BO  i@B@B@   XB@B@BM   
//
// WeText - A simple application that demonstrates the DDD, CQRS, Event Sourcing and Microservices architecture.
//
// Copyright (C) by daxnet 2016
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//    http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ---------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WeText.Common;
using WeText.Common.Config;
using WeText.Common.Messaging;
using WeText.Common.Querying;

namespace WeText.Services.Common
{
    /// <summary>
    /// Represents that the implemented classes are Web API controllers
    /// that can provide the access to some basic facilities for microservice
    /// scenarios.
    /// </summary>
    /// <typeparam name="TService">The type of the microservice.</typeparam>
    public abstract class MicroserviceApiController<TService> : ApiController
        where TService : Microservice
    {
        private readonly WeTextConfiguration configuration;
        private readonly ICommandSender commandSender;
        private readonly ITableDataGateway tableDataGateway;

        /// <summary>
        /// Initializes a new instance of <c>MicroserviceApiController{TService}</c> class.
        /// </summary>
        /// <param name="configuration">The configuration instance of WeText application.</param>
        /// <param name="commandSender">The command sender instance.</param>
        /// <param name="tableGatewayRegistration">The table gateway registration.</param>
        protected MicroserviceApiController(WeTextConfiguration configuration,
            ICommandSender commandSender,
            IEnumerable<Lazy<ITableDataGateway, NamedMetadata>> tableGatewayRegistration)
        {
            this.configuration = configuration;
            this.commandSender = commandSender;
            this.tableDataGateway = tableGatewayRegistration.First(x => x.Metadata.Name == $"{typeof(TService).FullName}.TableDataGateway").Value;
        }

        /// <summary>
        /// Gets the instance of <see cref="WeTextConfiguration"/>.
        /// </summary>
        /// <value>
        /// The configuration instance of WeText application.
        /// </value>
        protected WeTextConfiguration WeTextConfiguration => configuration;

        /// <summary>
        /// Gets the instance of <see cref="ICommandSender"/> which can send
        /// the command messages to the command bus.
        /// </summary>
        /// <value>
        /// The command sender instance.
        /// </value>
        protected ICommandSender CommandSender => commandSender;

        /// <summary>
        /// Gets the instance of <see cref="ITableDataGateway"/> which provides
        /// the basic operations on the relational query database.
        /// </summary>
        /// <value>
        /// The table data gateway instance.
        /// </value>
        protected ITableDataGateway TableDataGateway => tableDataGateway;
    }
}
