using System;
using Xunit;
using QuestStoreNAT.web.Services;
namespace QuestStoreNAT.web.Tests
{
    public class LevelStudentShould
    {
        [Fact]
        public void ReturnCorrectLevelStudent()
        {
            //Arrange
            var levelStudent = new LevelStudent();

            //Act
            int result = levelStudent.levelStudent(500);

            //Assert
            Assert.Equal(5, result);
        }
    }
}
