using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using QuestStoreNAT.web.Controllers;
using QuestStoreNAT.web.DatabaseLayer;
using QuestStoreNAT.web.Models;
using QuestStoreNAT.web.Services;
using Xunit;
namespace QuestStoreNAT.web.Tests
{
    public class ArtifactManagementShould
    {
        private ArtifactManagement _artifactManagement;
        
        public ArtifactManagementShould()
        {
            var _mockGroupTransactionDAO = new Mock<GroupTransactionDAO>();
            _mockGroupTransactionDAO.Setup(x => x.FindOneRecordBy(It.IsAny<int>())).Returns(new GroupTransaction() { ID = 1 });

            _artifactManagement = new ArtifactManagement(_mockGroupTransactionDAO.Object);
        }
        [Fact]
        public void Check_IfGroupTransactionExist_ReturnFalse()
        {
            var result = _artifactManagement.CheckingIfTransactionForBoughtGroupArtifactExist(1);

            Assert.False(result);
        }
    }
}
