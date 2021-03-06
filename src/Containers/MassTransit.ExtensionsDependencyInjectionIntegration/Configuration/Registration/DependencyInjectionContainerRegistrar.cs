// Copyright 2007-2019 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the
// License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, either express or implied. See the License for the
// specific language governing permissions and limitations under the License.
namespace MassTransit.ExtensionsDependencyInjectionIntegration.Configuration.Registration
{
    using Courier;
    using MassTransit.Registration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Saga;
    using ScopeProviders;
    using Scoping;


    public class DependencyInjectionContainerRegistrar :
        IContainerRegistrar
    {
        readonly IServiceCollection _collection;

        public DependencyInjectionContainerRegistrar(IServiceCollection collection)
        {
            _collection = collection;
        }

        public void RegisterConsumer<T>()
            where T : class, IConsumer
        {
            _collection.AddScoped<T>();
        }

        public void RegisterSaga<T>()
            where T : class, ISaga
        {
        }

        public void RegisterExecuteActivity<TActivity, TArguments>()
            where TActivity : class, ExecuteActivity<TArguments>
            where TArguments : class
        {
            _collection.TryAddScoped<TActivity>();

            _collection.AddTransient<IExecuteActivityScopeProvider<TActivity, TArguments>,
                DependencyInjectionExecuteActivityScopeProvider<TActivity, TArguments>>();
        }

        public void RegisterCompensateActivity<TActivity, TLog>()
            where TActivity : class, CompensateActivity<TLog>
            where TLog : class
        {
            _collection.TryAddScoped<TActivity>();

            _collection.AddTransient<ICompensateActivityScopeProvider<TActivity, TLog>,
                DependencyInjectionCompensateActivityScopeProvider<TActivity, TLog>>();
        }
    }
}
