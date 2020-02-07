using AlumniMessaging.ViewModels;
using System;
using AlumniMessaging.Services;
using FluentAssertions;
using Xunit;

namespace AlumniMessaging.Tests
{
    public class ContactsViewModelTest
    {
        [Theory]
        [InlineData("0716475048", "AGM Kavinda 9")]
        [InlineData("0716475048", " AGM Kavinda 9")]
        [InlineData("0716475048", "AgM  Kavinda 9")]
        [InlineData("0716475048", " AgM  Kavinda 09 ")]
        [InlineData("0716475048", " AgM  Kavinda  09 ")]
        //[InlineData("0716475048", " AgM  Kavinda  09 Additional data")]
        //[InlineData("0716475048", " AgM  Kavinda  09 Additional data 123")]
        public void ParseContact(string sender, string text)
        {
            var sut = new ContactsViewModel(null, null);
            var message = new ReceivedTextMessage() {Sender = sender, Text = text};

            var actual = sut.ParseContact(message);
            actual.Should().NotBeNull();
            actual.Batch.Should().Be(9);
            actual.Mobile.Should().Be("0716475048");
            actual.Name.Should().Be("Kavinda");
        }

        [Fact]
        public void ParseContact_EmptyText_SenderRead()
        {
            var sut = new ContactsViewModel(null, null);
            var message = new ReceivedTextMessage() { Sender = "0716475048", Text = "" };

            var actual = sut.ParseContact(message);
            actual.Should().NotBeNull();
            actual.Batch.Should().Be(0);
            actual.Mobile.Should().Be("0716475048");
            actual.Name.Should().BeEmpty();
        }

        [Fact]
        public void ParseContact_NoBatch_ReadSenderAndName()
        {
            var sut = new ContactsViewModel(null, null);
            var message = new ReceivedTextMessage() { Sender = "0716475048", Text = "AGM Kavinda Gayashan  " };

            var actual = sut.ParseContact(message);
            actual.Should().NotBeNull();
            actual.Batch.Should().Be(0);
            actual.Mobile.Should().Be("0716475048");
            actual.Name.Should().Be("Kavinda Gayashan");
        }

        [Fact]
        public void ParseContact_MobileWithCountryCode()
        {
            var sut = new ContactsViewModel(null, null);
            var message = new ReceivedTextMessage() { Sender = "+94716475048", Text = "AGM Kavinda 9" };

            var actual = sut.ParseContact(message);
            actual.Should().NotBeNull();
            actual.Batch.Should().Be(9);
            actual.Mobile.Should().Be("0716475048");
            actual.Name.Should().Be("Kavinda");
        }

        [Fact]
        public void ParseContact_MobileWithForeignCountryCode_CountryCodePreserved()
        {
            var sut = new ContactsViewModel(null, null);
            var message = new ReceivedTextMessage() { Sender = "+67716475048", Text = "AGM Kavinda 9" };

            var actual = sut.ParseContact(message);
            actual.Should().NotBeNull();
            actual.Batch.Should().Be(9);
            actual.Mobile.Should().Be("+67716475048");
            actual.Name.Should().Be("Kavinda");
        }
    }
}
