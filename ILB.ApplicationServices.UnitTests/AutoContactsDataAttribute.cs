using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Xunit;

namespace ILB.ApplicationServices.UnitTests
{
    /*public class AutoContactsDataAttribute : AutoDataAttribute
    {
        public AutoContactsDataAttribute() : base(new Fixture().Customize(new AutoMoqCustomization()))
        {
        }
    }
}*/


    public class AutoContactsDataAttribute : AutoDataAttribute
    {
        public AutoContactsDataAttribute()
            : base(new Fixture().Customize(new MyWebApiCustomization()))
        {
        }
    }

    public class MyWebApiCustomization : CompositeCustomization
{
    public MyWebApiCustomization()
        : base(
            new ValidationResultsCustomization(),
            new AutoMoqCustomization()
        )
    {
    }

    private class ValidationResultsCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Inject(new ValidationService());
        }
    }
}
}
