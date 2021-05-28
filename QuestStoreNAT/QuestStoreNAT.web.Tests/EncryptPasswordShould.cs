using LoginForm.Services;
using Xunit;

namespace QuestStoreNAT.web.Tests
{
    public class EncryptPasswordShould
    {
        [Fact]
        public void ProduceUniqueSALTs()
        {
            var salt1 = EncryptPassword.CreateSALT();
            var salt2 = EncryptPassword.CreateSALT();

            Assert.NotEqual(salt1, salt2);
        }
    }
}
