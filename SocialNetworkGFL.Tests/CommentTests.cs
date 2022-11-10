using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Domain.Context;
using BusinessLogic.Services;

namespace SocialNetworkGFL.Tests
{
    [TestClass]
    public class CommentTests
    {
        [TestMethod]
        public void AddComment()
        {
            var mockSet = new Mock<DbSet<Comment>>();
            var mockContext = new Mock<SocialNetworkContext>();
            mockContext.Setup(m => m.Comments).Returns(mockSet.Object);

            var service = new CommentService(mockContext.Object);
            var comment = new Comment
            {
                Content = "Hello world",
            };

            service.AddComment(comment);
            mockSet.Verify(m => m.Add(It.IsAny<Comment>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
        }
    }
}
