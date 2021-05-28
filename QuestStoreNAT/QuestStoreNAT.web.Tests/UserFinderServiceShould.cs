using Xunit;
using Moq;
using Moq.Protected;
using QuestStoreNAT.web.Services;
using QuestStoreNAT.web.Models;
using System;

namespace QuestStoreNAT.web.Tests
{
    public class UserFinderServiceShould
    {
        private Mock<UserFinderService> _findIUserMockSuccess;
        private Mock<UserFinderService> _findIUserMockFail;

        public UserFinderServiceShould()
        {
            _findIUserMockSuccess = new Mock<UserFinderService>();
            _findIUserMockSuccess.CallBase = true;
            _findIUserMockSuccess.Protected().Setup<IUser>("FindStudentBy", new object[] { 1 }).Returns(new Student());
            _findIUserMockSuccess.Protected().Setup<IUser>("FindMentorBy", new object[] { 1 }).Returns(new Mentor());
            _findIUserMockSuccess.Protected().Setup<IUser>("FindAdminBy", new object[] { 1 }).Returns(new Admin());

            _findIUserMockFail = new Mock<UserFinderService>();
            _findIUserMockFail.CallBase = true;
            _findIUserMockFail.Protected().Setup<IUser>("FindStudentBy", new object[] { 2 }).Returns((Student)null);
            _findIUserMockFail.Protected().Setup<IUser>("FindMentorBy", new object[] { 2 }).Returns((Mentor)null);
            _findIUserMockFail.Protected().Setup<IUser>("FindAdminBy", new object[] { 2 }).Returns((Admin)null);
        }

        [Fact]
        public void ReturnStudent_When_RoleIsStudentAndIdIsValid()
        {
            var sut = _findIUserMockSuccess.Object.RetrieveUser(Role.Student, 1);
            Assert.IsType<Student>(sut);
        }

        [Fact]
        public void ReturnMentor_When_RoleIsMentorAndIdIsValid()
        {
            var sut = _findIUserMockSuccess.Object.RetrieveUser(Role.Mentor, 1);
            Assert.IsType<Mentor>(sut);
        }

        [Fact]
        public void ReturnAdmin_When_RoleIsAdminAndIdIsValid()
        {
            var sut = _findIUserMockSuccess.Object.RetrieveUser(Role.Admin, 1);
            Assert.IsType<Admin>(sut);
        }

        [Fact]
        public void ReturnNull_When_RoleIsStudentAndIdIsInvalid()
        {
            var sut = _findIUserMockFail.Object.RetrieveUser(Role.Student, 2);
            Assert.Null(sut);
        }

        [Fact]
        public void ReturnNull_When_RoleIsMentorAndIdIsInvalid()
        {
            var sut = _findIUserMockFail.Object.RetrieveUser(Role.Mentor, 2);
            Assert.Null(sut);
        }

        [Fact]
        public void ReturnNull_When_RoleIsAdminAndIdIsInvalid()
        {
            var sut = _findIUserMockFail.Object.RetrieveUser(Role.Admin, 2);
            Assert.Null(sut);
        }

        [Fact]
        public void ReturnNull_When_RoleIsNone()
        {
            var sut = _findIUserMockFail.Object.RetrieveUser(Role.None, 2);
            Assert.Null(sut);
        }

        [Fact]
        public void ThrowArguementException_When_IdIsLessThan0()
        {
            Assert.Throws<ArgumentException>(() => _findIUserMockSuccess.Object.RetrieveUser(Role.Student, -1));
        }
    }
}
