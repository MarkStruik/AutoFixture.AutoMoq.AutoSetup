using System.Collections.Generic;
using System.Linq;
using AutoFixture.AutoMoq.AutoSetup;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.NUnit2;

namespace AutoFixture.AutoMoq.AutoSetupTests
{
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
            RelayTestClass SingleResultNoInput();
            IEnumerable<RelayTestClass> ListResultNoInput();
            RelayTestClass GetItemById(int id);
            RelayTestClass GetItemById(int id, string randomSearch);
            RelayTestClass GetItemBySearch(RandomInputData data);
        }

        [Test, AutoMoqData]
        public void Create_AutoMockInterfaceIEnumerableResults_InterfaceIsSetupWithIEnumerable(
            [Frozen] RelayTestClass itemTemplate,
            [Frozen] ISingleResultNoInput sut,
            [Frozen] IFixture fixture
            )
        {
            List<RelayTestClass> result = sut.ListResultNoInput().ToList();
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(itemTemplate));
        }

        [Test, AutoMoqData]
        public void Create_AutoMockInterfaceMethodsAutoMoqData_InterfaceIsSetup(
            [Frozen] RelayTestClass frozenItem, [Frozen] ISingleResultNoInput sut, int expectedId)
        {
            frozenItem.Id = expectedId;
            RelayTestClass result = sut.SingleResultNoInput();
            Assert.That(result.Id, Is.EqualTo(expectedId));
        }

        [Test, AutoMoqData]
        public void Create_AutoMockInterfaceMethodParameter_InterfaceIsSetup(
            [Frozen] RelayTestClass frozenItem, [Frozen] ISingleResultNoInput sut, int id)
        {
            frozenItem.Id = id;
            RelayTestClass result = sut.GetItemById(id);
            Assert.That(result.Id, Is.EqualTo(id));
        }

        [Test, AutoMoqData]
        public void Create_AutoMockInterfaceMethodsAutoMoqDataMultipleInput_InterfaceIsSetup(
            [Frozen] RelayTestClass frozenItem, [Frozen] ISingleResultNoInput sut, int id, string searchString)
        {
            frozenItem.Id = id;
            RelayTestClass result = sut.GetItemById(id, searchString);
            Assert.That(result.Id, Is.EqualTo(id));
        }

        [Test, AutoMoqData]
        public void Create_AutoMockInterfaceMethodsAutoMoqDataComplexInput_InterfaceIsSetup(
            [Frozen] RelayTestClass frozenItem, [Frozen] ISingleResultNoInput sut, RandomInputData input)
        {
            frozenItem.Id = input.Prop1;
            RelayTestClass result = sut.GetItemBySearch(input);
            Assert.That(result.Id, Is.EqualTo(input.Prop1));
        }
    }
}