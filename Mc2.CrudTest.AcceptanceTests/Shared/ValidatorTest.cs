using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Mc2.CrudTest.Shared.Utilities;
using Xunit;

namespace Mc2.CrudTest.AcceptanceTests.Shared
{
    public class ValidatorTest
    {
        [Fact]
        public void PhoneIsValid_IsValid_ReturnsTrue()
        {
            // Arrange
            const string phoneNo = "+989121234567"; // Mobile
            //const string phoneNo = "+16156381234"; // Fixed Line

            // Act
            var result = Validator.PhoneIsValid(phoneNo);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void PhoneIsValid_IsNotValid_ReturnsFalse()
        {
            // Arrange
            const string phoneNo = "+982188776655";

            // Act
            var result = Validator.PhoneIsValid(phoneNo);

            // Assert
            result.Should().BeFalse();
        }
    }
}
