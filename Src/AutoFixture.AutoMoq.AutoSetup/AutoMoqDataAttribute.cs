using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.NUnit2;

namespace AutoFixture.AutoMoq.AutoSetup
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
            : this(1)
        {
        }

        public AutoMoqDataAttribute(int repeatCount)
            : base(new Fixture().Customize(new AutoMoqCustomization(new MockRelayExtras())))
        {
            Fixture.RepeatCount = repeatCount;
        }

    }

}