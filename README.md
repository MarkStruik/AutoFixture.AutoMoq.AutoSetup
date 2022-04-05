# AutoFixture.AutoMoq.AutoSetup
 
extending AutoFixture.AutoMoq to automatically setup all calls on a mocked interface or abstract class

NuGet: https://www.nuget.org/packages/AutoFixture.AutoMoq.AutoSetup/

**** they ( https://github.com/AutoFixture/AutoFixture ) added this ( though a bit more advanced ) to autofixture.automoq in version 3.20. Ill leave this up for reference. But no updates will be added. You can still ask questions about the way it is implemented and i will try to answer them the best i can

# My thinking

I added a question to stackoverflow. Seeing as i created a solution to 'fix' the problem i wanted a better way of updating the code for that solution.

( http://stackoverflow.com/questions/25207585/autofixture-automoq-nunit-autodata-frozen-object-not-returned-by-automoq-inter )

I started out With the MockRelay class in the AutoFixture.AutoMoq nuget package. I created an overload MockRelayExtras and i add that to the AutoMoqCustomozation.

Source for AutoFixture => https://github.com/AutoFixture/AutoFixture

What this will do is reflect on the interface or abstract class and setup the mock accordingly

ex.

```C#
Mock.Get(TInterface.Instance)
    .Setup( i => i.Method( It.IsAny<TParamType>() ) )
    .Returns( () => fixture.Create<TMethodResult>() );
```

For now this will only work for Interfaces and Abstract classes ( without constructors ) 


## Unit tests with NUnit
This wil enable something like this:

```C#
[TestFixture]
public class MockRelayExtrasTests
{
    public class RelayTestClass
    {
        public int Id { get; set; }
    }

    public class RandomInputData
        {
            public int Prop1 { get; set; }
            public string Prop2 { get; set; }
        }

    public interface ISingleResultNoInput
    {
        RelayTestClass GetItemBySearch(RandomInputData data);
    }

    [Test, AutoMoqData]
    public void AutoMockInterface_InterfaceIsSetupAndReturnsFrozenResult(
        [Frozen] RelayTestClass frozenItem, 
        ISingleResultNoInput sut, 
        RandomInputData input)
    {
        frozenItem.Id = input.Prop1;
        RelayTestClass result = sut.GetItemBySearch(input);
        Assert.That(result.Id, Is.EqualTo(input.Prop1));
    }
}
```

## To be continued

Its a start. Furthermore i hope other people will like, enjoy and use this?

Happy coding!
