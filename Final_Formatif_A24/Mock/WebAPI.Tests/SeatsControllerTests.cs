using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using WebAPI.Exceptions;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Tests;

[TestClass]
public class SeatsControllerTests
{
    [TestMethod]
    public void ReserveSeat()
    {
        Mock<SeatsService> serviceMock = new Mock<SeatsService>();
        Mock<SeatsController> controller = new Mock<SeatsController>(serviceMock.Object) { CallBase = true };

        var seatnumber = 1;
        var userid = "test1";

        var expectedSeat = new Seat { Number = seatnumber, ExamenUserId = userid };

        serviceMock
          .Setup(s => s.ReserveSeat(It.IsAny<string>(), It.IsAny<int>()))
          .Returns(expectedSeat);
        controller.Setup(c => c.UserId).Returns(userid);

        var actionresult = controller.Object.ReserveSeat(seatnumber);

        OkObjectResult result = actionresult.Result as OkObjectResult;
        Assert.IsNotNull(result);
        var returnedSeat = result.Value as Seat;
        Assert.IsNotNull(returnedSeat);
        Assert.AreEqual(expectedSeat.Number, returnedSeat.Number);
        Assert.AreEqual(expectedSeat.ExamenUserId, returnedSeat.ExamenUserId);
    }

    [TestMethod]
    public void UnauthorizedSeat()
    {
        Mock<SeatsService> serviceMock = new Mock<SeatsService>();
        Mock<SeatsController> controller = new Mock<SeatsController>(serviceMock.Object) { CallBase = true };

        var seatnumber = 1;
        var userid = "test1";
        serviceMock
          .Setup(s => s.ReserveSeat(It.IsAny<string>(), It.IsAny<int>()))
          .Throws(new SeatAlreadyTakenException());

        controller.Setup(c => c.UserId).Returns(userid);

        var actionresult = controller.Object.ReserveSeat(seatnumber);

        var result  = actionresult.Result as UnauthorizedResult;
        Assert.IsNotNull(result);
        Assert.AreEqual(401, result.StatusCode);
        Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
    }

    [TestMethod]
    public void NotFoundSeat()
    {
        Mock<SeatsService> serviceMock = new Mock<SeatsService>();
        Mock<SeatsController> controller = new Mock<SeatsController>(serviceMock.Object) { CallBase = true };

        var seatnumber = 111;
        var userid = "test1";
        serviceMock
          .Setup(s => s.ReserveSeat(It.IsAny<string>(), It.IsAny<int>()))
          .Throws(new SeatOutOfBoundsException());

        controller.Setup(c => c.UserId).Returns(userid);

        var actionresult = controller.Object.ReserveSeat(seatnumber);

        var result = actionresult.Result as NotFoundObjectResult;
        Assert.AreEqual("Could not find " + seatnumber, result.Value);
        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }
    [TestMethod]
    public void BadRequestSeat()
    {
        Mock<SeatsService> serviceMock = new Mock<SeatsService>();
        Mock<SeatsController> controller = new Mock<SeatsController>(serviceMock.Object) { CallBase = true };

        var seatnumber = 1;
        var userid = "test1";
        serviceMock
          .Setup(s => s.ReserveSeat(It.IsAny<string>(), It.IsAny<int>()))
          .Throws(new UserAlreadySeatedException());

        controller.Setup(c => c.UserId).Returns(userid);

        var actionresult = controller.Object.ReserveSeat(seatnumber);

        var result = actionresult.Result as BadRequestResult;
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, typeof(BadRequestResult));

    }

}
