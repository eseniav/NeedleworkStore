using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeedleworkStore.Classes;
using System;

namespace NeedleworkStore.Tests
{
    [TestClass]
    public class CheckCardTests
    {
        [TestMethod]
        public void ContainsOnlyDigits_ValidInput_ReturnsTrue()
        {
            Assert.IsTrue(CheckCard.ContainsOnlyDigits("123456"));
        }

        [TestMethod]
        public void ContainsOnlyDigits_InvalidInput_ReturnsFalse()
        {
            Assert.IsFalse(CheckCard.ContainsOnlyDigits("123a56"));
        }

        [TestMethod]
        public void ContainsOnlyDigits_EmptyInput_ReturnsFalse()
        {
            Assert.IsFalse(CheckCard.ContainsOnlyDigits(""));
        }

        [TestMethod]
        public void IsValidMonth_ValidMonth_ReturnsTrue()
        {
            Assert.IsTrue(CheckCard.IsValidMonth("12"));
        }

        [TestMethod]
        public void IsValidMonth_InvalidMonth_ReturnsFalse()
        {
            Assert.IsFalse(CheckCard.IsValidMonth("13"));
        }

        [TestMethod]
        public void IsValidMonth_NonDigits_ReturnsFalse()
        {
            Assert.IsFalse(CheckCard.IsValidMonth("ab"));
        }

        [TestMethod]
        public void IsValidYear_ValidYear_ReturnsTrue()
        {
            int currentYear = DateTime.Now.Year % 100;
            Assert.IsTrue(CheckCard.IsValidYear(currentYear.ToString()));
        }

        [TestMethod]
        public void IsValidYear_PastYear_ReturnsFalse()
        {
            int pastYear = DateTime.Now.Year % 100 - 1;
            Assert.IsFalse(CheckCard.IsValidYear(pastYear.ToString()));
        }

        [TestMethod]
        public void IsNotExpired_ValidFutureDate_ReturnsTrue()
        {
            int nextYear = DateTime.Now.Year % 100 + 1;
            Assert.IsTrue(CheckCard.IsNotExpired("12", nextYear.ToString()));
        }

        [TestMethod]
        public void IsNotExpired_PastDate_ReturnsFalse()
        {
            int pastYear = DateTime.Now.Year % 100 - 1;
            Assert.IsFalse(CheckCard.IsNotExpired("12", pastYear.ToString()));
        }

        [TestMethod]
        public void CheckCardNumber_ValidCard_ReturnsTrue()
        {
            Assert.IsTrue(CheckCard.CheckCardNumber("1234567890123456"));
        }

        [TestMethod]
        public void CheckCardNumber_ShortCard_ReturnsFalse()
        {
            Assert.IsFalse(CheckCard.CheckCardNumber("1234567890"));
        }
    }
}