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
    public class QuestControllerShould
    {
        private readonly QuestController _sutController;
        private readonly Mock<ILogger<QuestController>> _mockLogger;
        private readonly Mock<IDB_GenericInterface<Quest>> _mockQuestDao;
        private readonly Mock<HttpResponse> _mockHttpResponse;

        public QuestControllerShould()
        {
            _mockLogger = new Mock<ILogger<QuestController>>();
            var mockICurrentSession = new Mock<ICurrentSession>();
            _mockQuestDao = new Mock<IDB_GenericInterface<Quest>>();

            _mockHttpResponse = new Mock<HttpResponse>();
            _mockHttpResponse.SetupAllProperties();
            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupAllProperties();
            mockHttpContext.SetupGet(x => x.Response).Returns(_mockHttpResponse.Object);

            var mockTempData = new Mock<ITempDataDictionary>();

            _sutController = new QuestController( 
                _mockLogger.Object,
                mockICurrentSession.Object,
                _mockQuestDao.Object
                )
            {
                TempData = mockTempData.Object,
                ControllerContext = new ControllerContext {HttpContext = mockHttpContext.Object}
            };
        }

        [Fact]
        public void ReturnDeleteView_When_PassedValidIdTo_DeleteQuest()
        {
            _mockQuestDao.Setup(x => x.FindOneRecordBy(It.IsAny<int>())).Returns(new Quest() { Id = 1 });
             
            var actionResult = _sutController.DeleteQuest(1) as ViewResult;  

            Assert.NotNull(actionResult);
            Assert.Equal("DeleteQuest", actionResult.ViewName);
        }

        [Fact]
        public void ReturnDeleteViewWithModel_When_PassedValidIdTo_DeleteQuest()
        {
            _mockQuestDao.Setup(x => x.FindOneRecordBy(It.IsAny<int>())).Returns(new Quest() { Id = 1 });

            var actionResult = _sutController.DeleteQuest(1) as ViewResult;
            var quest = actionResult.ViewData.Model;

            Assert.NotNull(actionResult);
            Assert.IsType<Quest>(quest);
        }

        [Fact]
        public void ReturnNotFoundView_When_ModelIsNullIn_DeleteQuest()
        {
            _mockQuestDao.Setup(x => x.FindOneRecordBy(It.IsAny<int>())).Returns((Quest)null);

            var actionResult = _sutController.DeleteQuest(1) as ViewResult;

            Assert.NotNull(actionResult);
            Assert.Equal("NotFound", actionResult.ViewName);
        }

        [Fact]
        public void SetStatusCodeTo404_When_ModelIsNullIn_DeleteQuest()
        {
            _mockQuestDao.Setup(x => x.FindOneRecordBy(It.IsAny<int>())).Returns((Quest)null);

            _sutController.DeleteQuest(1);

            _mockHttpResponse.VerifySet(x => x.StatusCode = 404);
        }

        [Fact]
        public void RedirectToActionViewAllQuests_WhenValidQuestIsPassedTo_DeleteQuest()
        {
            _mockQuestDao.Setup(x => x.FindOneRecordBy(It.IsAny<int>())).Returns(new Quest() { Id = 1 });

            var actionResult = (RedirectToActionResult)_sutController.DeleteQuest(new Quest() { Id = 1 });

            Assert.NotNull(actionResult);
            Assert.Equal("ViewAllQuests", actionResult.ActionName);
        }

        [Fact]
        public void ReturnNotFoundView_When_PassedNegativeIdTo_DeleteQuest()
        {
            var actionResult = _sutController.DeleteQuest(-1) as ViewResult;

            Assert.Equal("NotFound", actionResult.ViewName);
        }

        [Fact]
        public void ReturnErrorView_When_PassedNullQuestTo_DeleteQuest()
        {
            var actionResult = _sutController.DeleteQuest(null) as ViewResult;

            Assert.Equal("Error", actionResult.ViewName);
        }

        [Fact]
        public void CallQuestDAO_When_DeletingValidQuestIn_DeleteQuest()
        {
            _mockQuestDao.Setup(x => x.FindOneRecordBy(It.IsAny<int>())).Returns(new Quest() { Id = 1 });
            
            _sutController.DeleteQuest(new Quest() { Id = 1 });

            _mockQuestDao.Verify(x => x.DeleteRecord(1), Times.Once);
        }

        [Fact]
        public void CallQuestDAO_When_RetrievingAllQuestsIn_ViewAllQuests()
        {
            _sutController.ViewAllQuests();

            _mockQuestDao.Verify(x=>x.FetchAllRecords(), Times.Once);
        }

        [Fact]
        public void ReturnViewAllQuestsView_When_SuccessfulDbRetrievalIn_ViewAllQuests()
        {
            _mockQuestDao.Setup(x => x.FetchAllRecords()).Returns(new List<Quest>());

            var actionResult = _sutController.ViewAllQuests() as ViewResult;

            Assert.Equal("ViewAllQuests", actionResult.ViewName);
        }

        [Fact]
        public void ReturnErrorView_When_DbReturnsNoQuestsIn_ViewAllQuests()
        {
            _mockQuestDao.Setup(x => x.FetchAllRecords()).Returns((List<Quest>)null);

            var actionResult = _sutController.ViewAllQuests() as ViewResult;

            Assert.Equal("Error", actionResult.ViewName);
        }

        [Fact]
        public void LogError_When_DbReturnsNoQuestsIn_ViewAllQuests()
        {
            _mockQuestDao.Setup(x => x.FetchAllRecords()).Returns((List<Quest>)null);
            
            _sutController.ViewAllQuests();

            _mockLogger.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()), 
                Times.Once);
        }

        [Fact]
        public void ReturnAddView_When_Calls_AddQuest()
        {
            var actionResult = _sutController.AddQuest() as ViewResult;

            Assert.Equal("AddQuest", actionResult.ViewName);
        }

        [Fact]
        public void RedirectToViewAllQuests_When_ValidQuestIsPassedTo_AddQuest()
        {
            var actionResult = (RedirectToActionResult) _sutController.AddQuest(new Quest());

            Assert.Equal("ViewAllQuests", actionResult.ActionName);
        }

        [Fact]
        public void AddQuest_When_ValidQuestIsPassedTo_AddQuest()
        {
            var sampleQuest = new Quest();

            _sutController.AddQuest(sampleQuest);

            _mockQuestDao.Verify(x=>x.AddRecord(sampleQuest));
        }

        [Fact]
        public void ReturnErrorView_When_NullIsPassedTo_AddQuest()
        {
            var actionResult = _sutController.AddQuest(null) as ViewResult;

            Assert.Equal("Error", actionResult.ViewName);
        }

        [Fact]
        public void LogError_When_NullIsPassedTo_AddQuest()
        {
            _sutController.AddQuest(null);

            _mockLogger.Verify(x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact]
        public void SetStatusCodeTo406_When_NullIsPassedTo_AddQuest()
        {
            _sutController.AddQuest(null);

            _mockHttpResponse.VerifySet(x => x.StatusCode = 406);
        }

        [Fact]
        public void ReturnErrorView_When_InvalidModelIsPassedTo_AddQuest()
        {
            _sutController.ModelState.AddModelError("test","test");

            var actionResult = _sutController.AddQuest(new Quest()) as ViewResult;

            Assert.Equal("Error", actionResult.ViewName);
        }

        [Fact]
        public void LogError_When_InvalidModelIsPassedTo_AddQuest()
        {
            _sutController.ModelState.AddModelError("test", "test");

            _sutController.AddQuest(new Quest());

            _mockLogger.Verify(x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact]
        public void SetStatusCodeTo406_InvalidModelIsPassedTo_AddQuest()
        {
            _sutController.ModelState.AddModelError("test", "test");

            _sutController.AddQuest(new Quest());

            _mockHttpResponse.VerifySet(x => x.StatusCode = 406);
        }

        [Fact]
        public void RedirectToViewAllQuests_When_ValidModelIsPassedTo_AddQuest()
        {
            _sutController.ModelState.Clear();

            var actionResult = (RedirectToActionResult)_sutController.AddQuest(new Quest());

            Assert.Equal("ViewAllQuests", actionResult.ActionName);
        }

        [Fact]
        public void ReturnEditView_When_PassedValidIdTo_EditAction()
        {
            _mockQuestDao.Setup(x => x.FindOneRecordBy(It.IsAny<int>())).Returns(new Quest() { Id = 1 });

            var actionResult = _sutController.EditQuest(1) as ViewResult;

            Assert.NotNull(actionResult);
            Assert.Equal("EditQuest", actionResult.ViewName);
        }

        [Fact]
        public void ReturnEditViewWithModel_When_PassedValidIdTo_EditQuest()
        {
            _mockQuestDao.Setup(x => x.FindOneRecordBy(It.IsAny<int>())).Returns(new Quest() { Id = 1 });

            var actionResult = _sutController.EditQuest(1) as ViewResult;
            var quest = actionResult.ViewData.Model;

            Assert.NotNull(actionResult);
            Assert.IsType<Quest>(quest);
        }

        [Fact]
        public void ReturnNotFoundView_When_ModelIsNullIn_EditQuest()
        {
            _mockQuestDao.Setup(x => x.FindOneRecordBy(It.IsAny<int>())).Returns((Quest)null);

            var actionResult = _sutController.EditQuest(1) as ViewResult;

            Assert.NotNull(actionResult);
            Assert.Equal("NotFound", actionResult.ViewName);
        }

        [Fact]
        public void SetStatusCodeTo404_When_ModelIsNullIn_EditQuest()
        {
            _mockQuestDao.Setup(x => x.FindOneRecordBy(It.IsAny<int>())).Returns((Quest)null);

            _sutController.EditQuest(1);

            _mockHttpResponse.VerifySet(x => x.StatusCode = 404);
        }

        [Fact]
        public void RedirectToActionViewAllQuests_WhenValidQuestIsPassedTo_EditQuest()
        {
            _mockQuestDao.Setup(x => x.FindOneRecordBy(It.IsAny<int>())).Returns(new Quest() { Id = 1 });

            var actionResult = (RedirectToActionResult)_sutController.EditQuest(new Quest() { Id = 1 });

            Assert.NotNull(actionResult);
            Assert.Equal("ViewAllQuests", actionResult.ActionName);
        }

        [Fact]
        public void ReturnNotFoundView_When_PassedNegativeIdTo_EditQuest()
        {
            var actionResult = _sutController.EditQuest(-1) as ViewResult;

            Assert.Equal("NotFound", actionResult.ViewName);
        }

        [Fact]
        public void ReturnErrorView_When_PassedNullQuestTo_EditQuest()
        {
            var actionResult = _sutController.EditQuest(null) as ViewResult;

            Assert.Equal("Error", actionResult.ViewName);
        }

        [Fact]
        public void SetStatusCodeTo406_InvalidModelIsPassedTo_EditQuest()
        {
            _sutController.ModelState.AddModelError("test", "test");

            _sutController.EditQuest(new Quest());

            _mockHttpResponse.VerifySet(x => x.StatusCode = 406);
        }

        [Fact]
        public void EditQuest_When_ValidQuestIsPassedTo_EditQuest()
        {
            var sampleQuest = new Quest();

            _sutController.EditQuest(sampleQuest);

            _mockQuestDao.Verify(x => x.UpdateRecord(sampleQuest));
        }

        [Fact]
        public void LogError_When_InvalidModelIsPassedTo_EditQuest()
        {
            _sutController.ModelState.AddModelError("test", "test");

            _sutController.EditQuest(new Quest());

            _mockLogger.Verify(x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact]
        public void LogError_When_NullIsPassedTo_EditQuest()
        {
            _sutController.EditQuest(null);

            _mockLogger.Verify(x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }
    }
}
