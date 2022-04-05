using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Moq;
using Moq.Language.Flow;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Kernel;

namespace AutoFixture.AutoMoq.AutoSetup
{
    public class MockRelayExtras : MockRelay, ISpecimenBuilder
    {
        public new object Create(object request, ISpecimenContext context)
        {
            object result = base.Create(request, context);

            var requestType = request as Type;
            if (requestType != null && result != null)
            {
                SetupMockWithInterface(requestType, result, context);
            }

            return result;
        }

        public static void SetupMockWithInterface(Type requestType, object result, ISpecimenContext context)
        {
            typeof(MockRelayExtras).GetMethod("SetupMockWithInterfaceGeneric")
                .MakeGenericMethod(requestType).Invoke(null, new[] { result, context });
        }

        public static void SetupMockWithInterfaceGeneric<TType>(TType result, ISpecimenContext context)
            where TType : class
        {
            //Mock.Get(stubMock).Setup(r => r.GetObject(It.IsAny<int>())).Returns(() => myObject);
            Mock<TType> genericMock = Mock.Get(result);

            foreach (MethodInfo methodInfo in typeof(TType).GetMethods())
            {
                // ISetup<TType,TMethodResult> setup = genericMock.Setup<TType,TMethodResult>( methodInfo )
                object setup =
                    typeof(MockRelayExtras).GetMethod("SetupAsGenericFunc")
                        .MakeGenericMethod(typeof(TType), methodInfo.ReturnType)
                        .Invoke(null, new object[] { methodInfo, genericMock });

                // setup.Returns( () => context.Create<TResult>() )
                typeof(MockRelayExtras).GetMethod("SetupReturnsAsGenericFunc")
                    .MakeGenericMethod(typeof(TType), methodInfo.ReturnType)
                    .Invoke(null, new[] { setup, context });
            }
        }

        public static ISetup<TMock, TResult> SetupAsGenericFunc<TMock, TResult>(MethodInfo method, Mock<TMock> mock)
            where TMock : class
        {
            ParameterExpression input = Expression.Parameter(typeof(TMock));

            ParameterInfo[] parameters = method.GetParameters();

            Expression<Func<TMock, TResult>> setup = null;

            if (parameters.Length > 0)
            {
                IEnumerable<MethodCallExpression> properties =
                    parameters.Select(
                        pi => Expression.Call(typeof(It), "IsAny", new[] { pi.ParameterType }, new Expression[0]));
                setup = Expression.Lambda<Func<TMock, TResult>>(
                    Expression.Call(input, method, properties), input);
            }
            else
            {
                setup = Expression.Lambda<Func<TMock, TResult>>(
                    Expression.Call(input, method), input);
            }


            return mock.Setup(setup);
        }

        public static void SetupReturnsAsGenericFunc<TMock, TResult>(ISetup<TMock, TResult> setup,
            ISpecimenContext context)
            where TMock : class
        {
            setup.Returns(() => context.Create<TResult>());
        }
    }
}