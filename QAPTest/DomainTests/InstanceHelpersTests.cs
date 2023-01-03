using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QAPTest.DomainTests
{
    [TestFixture]
    public class InstanceHelpersTests
    {
        [Test]
        public void CheckGenerateHashcode_CheckHashCode()
        {
            var permutation = new[] { 0, 1, 2 };
            var hashCode = InstanceHelpers.GenerateHashCode(permutation);

            Assert.That(hashCode, Is.EqualTo(36));
        }
    }
}
