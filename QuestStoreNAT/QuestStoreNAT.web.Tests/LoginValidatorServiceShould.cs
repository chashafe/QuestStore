using Xunit;
using Moq;
using QuestStoreNAT.web.Services;
using QuestStoreNAT.web.Models;
using QuestStoreNAT.web.DatabaseLayer;

namespace QuestStoreNAT.web.Tests
{
    public class LoginValidatorServiceShould
    {
        private readonly LoginValidatorService sut;
        private Mock<Credentials> mockEnteredValidCredentials;
        private Mock<Credentials> mockEnteredInvalidCredentials;
        private Mock<Credentials> mockRetrievedCredentials;
        private string fakeValidEmail = "atena@olimp.com";
        private string fakeValidPassword = "zeus";
        private string fakeHashedPasswordFromDb = "ycTWNLiQIMwmq/T5FsltPLTU";
        private string fakeHashedSALTFromDb = "FqrPYHwX3ZHpnnDc8wD/J7yvgeDq9OC1";

        public LoginValidatorServiceShould()
        {
            SetUpMockEnteredValidCredentials();
            SetUpMockEnteredInvalidCredentials();
            SetUpMockRetrievedCredentials();

            var mockCredentialsDAO = new Mock<CredentialsDAO>();
            mockCredentialsDAO.Setup(x => x.FindOneRecordBy(fakeValidEmail)).Returns(mockRetrievedCredentials.Object);

            sut = new LoginValidatorService(mockCredentialsDAO.Object);
        }

        private void SetUpMockRetrievedCredentials()
        {
            mockRetrievedCredentials = new Mock<Credentials>();
            mockRetrievedCredentials.SetupAllProperties();
            mockRetrievedCredentials.Setup(x => x.Password).Returns(fakeHashedPasswordFromDb);
            mockRetrievedCredentials.Setup(x => x.SALT).Returns(fakeHashedSALTFromDb);
        }

        private void SetUpMockEnteredInvalidCredentials()
        {
            mockEnteredInvalidCredentials = new Mock<Credentials>();
            mockEnteredInvalidCredentials.SetupAllProperties();
        }

        private void SetUpMockEnteredValidCredentials()
        {
            mockEnteredValidCredentials = new Mock<Credentials>();
            mockEnteredValidCredentials.SetupAllProperties();
            mockEnteredValidCredentials.Setup(x => x.Email).Returns(fakeValidEmail);
            mockEnteredValidCredentials.Setup(x => x.Password).Returns(fakeValidPassword);
        }

        [Fact]
        public void ReturnTrue_When_PassedValidCredentials()
        {
            bool actual = sut.IsValidPasswordHASH(mockEnteredValidCredentials.Object);
            Assert.True(actual);
        }

        [Fact]
        public void ReturnFalse_When_PassedInvalidPassword()
        {
            mockEnteredInvalidCredentials.Setup(x => x.Email).Returns(fakeValidEmail);
            mockEnteredInvalidCredentials.Setup(x => x.Password).Returns("wrongPassword");
            bool actual = sut.IsValidPasswordHASH(mockEnteredInvalidCredentials.Object);
            Assert.False(actual);
        }

        [Fact]
        public void ReturnFalse_When_PassedInvalidEmail()
        {
            mockEnteredInvalidCredentials.Setup(x => x.Email).Returns("NoSuch@email.com");
            mockEnteredInvalidCredentials.Setup(x => x.Password).Returns(fakeValidPassword);
            bool actual = sut.IsValidPasswordHASH(mockEnteredInvalidCredentials.Object);
            Assert.False(actual);
        }

        [Fact]
        public void ReturnFalse_When_PassedEmptyPassword()
        {
            mockEnteredInvalidCredentials.Setup(x => x.Email).Returns(fakeValidEmail);
            mockEnteredInvalidCredentials.Setup(x => x.Password).Returns("");
            bool actual = sut.IsValidPasswordHASH(mockEnteredInvalidCredentials.Object);
            Assert.False(actual);
        }

        [Fact]
        public void ReturnFalse_When_PassedEmptyEmail()
        {
            mockEnteredInvalidCredentials.Setup(x => x.Email).Returns("");
            mockEnteredInvalidCredentials.Setup(x => x.Password).Returns(fakeValidPassword);
            bool actual = sut.IsValidPasswordHASH(mockEnteredInvalidCredentials.Object);
            Assert.False(actual);
        }

        [Fact]
        public void ReturnFalse_When_PassedNullCredentials()
        {
            bool actual = sut.IsValidPasswordHASH(null);
            Assert.False(actual);
        }

        [Fact]
        public void ReturnFalse_When_PassedNullEmail()
        {
            mockEnteredInvalidCredentials.Setup(x => x.Email).Returns((string)null);
            mockEnteredInvalidCredentials.Setup(x => x.Password).Returns(fakeValidPassword);
            bool actual = sut.IsValidPasswordHASH(mockEnteredInvalidCredentials.Object);
            Assert.False(actual);
        }

        [Fact]
        public void ReturnFalse_When_PassedNullPassword()
        {
            mockEnteredInvalidCredentials.Setup(x => x.Email).Returns(fakeValidEmail);
            mockEnteredInvalidCredentials.Setup(x => x.Password).Returns((string)null);
            bool actual = sut.IsValidPasswordHASH(mockEnteredInvalidCredentials.Object);
            Assert.False(actual);
        }
    }
}
