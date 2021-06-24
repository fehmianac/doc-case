using System;
using Slot.Gateway.Service.Date;
using Xunit;

namespace Slot.Gateway.Tests.Service
{
    public class DateServiceTests
    {
        [Fact]
        public void FindMondayOfWeek_WhenCalledWithMondayDay()
        {
            var mondayDate = new DateTime(2021, 06, 14);
            var dateService = new DateService();
            var calculatedMonday = dateService.FindMondayOfWeek(mondayDate);
            Assert.Equal(mondayDate, calculatedMonday);
        }

        [Fact]
        public void FindMondayOfWeek_WhenCalledWithMiddleOfWeekDay()
        {
            var mondayDate = new DateTime(2021, 06, 16);
            var dateService = new DateService();
            var calculatedMonday = dateService.FindMondayOfWeek(mondayDate);
            Assert.Equal(new DateTime(2021,06,14), calculatedMonday);
        }
        
        [Fact]
        public void FindNextMondayOfWeek_WhenCalledWithMondayDay()
        {
            var mondayDate = new DateTime(2021, 06, 14);
            var dateService = new DateService();
            var calculatedMonday = dateService.FindNextMonday(mondayDate);
            Assert.Equal(new DateTime(2021,06,21), calculatedMonday);
        }
        
        [Fact]
        public void FindNextMondayOfWeek_WhenCalledWithMiddleOfWeekDay()
        {
            var mondayDate = new DateTime(2021, 06, 16);
            var dateService = new DateService();
            var calculatedMonday = dateService.FindNextMonday(mondayDate);
            Assert.Equal(new DateTime(2021,06,21), calculatedMonday);
        }
        
        [Fact]
        public void FindPrevMondayOfWeek_WhenCalledWithMondayDay()
        {
            var mondayDate = new DateTime(2021, 06, 14);
            var dateService = new DateService();
            var calculatedMonday = dateService.FindPrevMonday(mondayDate);
            Assert.Equal(new DateTime(2021,06,7), calculatedMonday);
        }
        
        [Fact]
        public void FindPrevMondayOfWeek_WhenCalledWithMiddleOfWeekDay()
        {
            var mondayDate = new DateTime(2021, 06, 16);
            var dateService = new DateService();
            var calculatedMonday = dateService.FindPrevMonday(mondayDate);
            Assert.Equal(new DateTime(2021,06,7), calculatedMonday);
        }
        
        
        //
        // [Theory, AutoData]
        // public async Task AddProductAsync_Should_Return_CartError_When_Djini_Returns_Not_Ok(AddProductModel addProductModel, CancellationToken cancellationToken)
        // {
        //     using var mock = AutoMock.GetStrict();
        //
        //     var apiCallResponse = _fixture.Create<ApiCallResponse<DjiniBasketModel>>();
        //     apiCallResponse.StatusCode = HttpStatusCode.BadRequest;
        //
        //     mock.Mock<IBasketRepository>().Setup(x => x.BasketExistsForUserAsync(addProductModel.BasketId, addProductModel.UserId, cancellationToken)).ReturnsAsync(true);
        //     mock.Mock<IDjiniClient>().Setup(x => x.GetBasketAsync(It.Is<Guid>(m => m == addProductModel.BasketId), cancellationToken)).ReturnsAsync(apiCallResponse);
        //     mock.Mock<ILocalizationHelper>().Setup(x => x.GetLocalizedString("CartError")).Returns("CartError");
        //
        //     var productService = mock.Create<ProductService>();
        //
        //     var response = await productService.AddProductAsync(addProductModel, cancellationToken);
        //
        //     Assert.Contains(response.Messages, q => q.Message == "CartError");
        //     Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        //
        //     mock.Mock<IDjiniClient>().Verify(x => x.GetBasketAsync(It.Is<Guid>(m => m == addProductModel.BasketId), cancellationToken), Times.Once);
        //     mock.Mock<IBasketRepository>().Verify(x => x.BasketExistsForUserAsync(addProductModel.BasketId, addProductModel.UserId, cancellationToken), Times.Once);
        // }
    }
}