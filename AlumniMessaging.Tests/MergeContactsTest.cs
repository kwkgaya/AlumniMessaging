using System.Linq;
using AlumniMessaging.Models;
using AlumniMessaging.ViewModels;
using FluentAssertions;
using Xunit;

namespace AlumniMessaging.Tests
{
    public class MergeContactsTest
    {
        [Fact]
        public void MergeContacts()
        {
            var oldList = new[] {new Contact {Mobile = "0716475048"}};
            var newList = new[] {new Contact {Mobile = "0773036716"}};
            var sut = new ContactsViewModel(null, null);

            var merged = sut.MergeContacts(newList, oldList).ToList();
            merged.Should().NotBeNull();
            merged.Should().HaveCount(2);
            merged[0].Mobile.Should().Be(newList[0].Mobile);
            merged[1].Mobile.Should().Be(oldList[0].Mobile);
        }

        [Fact]
        public void MergeContacts_Duplicates_Removed()
        {
            var oldList = new[] { new Contact { Mobile = "0716475048" }, new Contact { Mobile = "0776475048" , Name = "Kavinda 1"} };
            var newList = new[] { new Contact { Mobile = "0773036716" }, new Contact { Mobile = "0776475048", Name = "Kavinda 2" } };
            var sut = new ContactsViewModel(null, null);

            var merged = sut.MergeContacts(newList, oldList).ToList();
            merged.Should().NotBeNull();
            merged.Should().HaveCount(3);
            merged[0].Mobile.Should().Be(newList[0].Mobile);
            merged[1].Mobile.Should().Be(newList[1].Mobile);
            merged[1].Name.Should().Be(newList[1].Name);
            merged[2].Mobile.Should().Be(oldList[0].Mobile);
        }

        [Fact]
        public void MergeContacts_DuplicatesWithCountrycode_Removed()
        {
            var oldList = new[] { new Contact { Mobile = "0716475048" }, new Contact { Mobile = "0776475048", Name = "Kavinda 1" } };
            var newList = new[] { new Contact { Mobile = "0773036716" }, new Contact { Mobile = "+94776475048", Name = "Kavinda 2" } };
            var sut = new ContactsViewModel(null, null);

            var merged = sut.MergeContacts(newList, oldList).ToList();
            merged.Should().NotBeNull();
            merged.Should().HaveCount(3);
            merged[0].Mobile.Should().Be(newList[0].Mobile);
            merged[1].Mobile.Should().Be(newList[1].Mobile);
            merged[1].Name.Should().Be(newList[1].Name);
            merged[2].Mobile.Should().Be(oldList[0].Mobile);
        }
    }
}