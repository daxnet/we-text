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
using WeText.Common.Events;

namespace WeText.Common
{
    /// <summary>
    /// Represents that the implemented classes are aggregate roots.
    /// </summary>
    /// <typeparam name="TKey">The type of the aggregate root key.</typeparam>
    /// <seealso cref="WeText.Common.IEntity{TKey}" />
    public interface IAggregateRoot<TKey> : IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets the uncommitted events stored in current aggregate root.
        /// </summary>
        /// <value>
        /// The uncommitted events.
        /// </value>
        IEnumerable<IDomainEvent> UncommittedEvents { get; }

        /// <summary>
        /// Reapplies the events on current aggregate root.
        /// </summary>
        /// <param name="events">The events.</param>
        void Replay(IEnumerable<IDomainEvent> events);
    }
}
