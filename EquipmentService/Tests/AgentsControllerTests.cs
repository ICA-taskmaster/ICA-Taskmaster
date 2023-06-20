using AutoMapper;
using EquipmentService.Controllers;
using EquipmentService.Data;
using EquipmentService.Dtos;
using EquipmentService.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace EquipmentService.Tests;

[TestFixture]
public class AgentsControllerTests {
    private AgentsController agentsController;
    private Mock<IEquipmentRespository> repositoryMock;
    private Mock<IMapper> mapperMock;

    [SetUp]
    public void Setup() {
        repositoryMock = new Mock<IEquipmentRespository>();
        mapperMock = new Mock<IMapper>();
        agentsController = new AgentsController(repositoryMock.Object, mapperMock.Object);
    }

    [Test]
    public void GetAgents_ReturnsOkResultWithMappedAgents() {
        // Arrange
        var agents = new List<Agent>();
        repositoryMock.Setup(repo => repo.getAll()).Returns(agents);
            
        var mappedAgents = new List<AgentFetchDto>();
        mapperMock.Setup(mapper => mapper.Map<IEnumerable<AgentFetchDto>>(agents)).Returns(mappedAgents);

        // Act
        var result = agentsController.getAgents();

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = (OkObjectResult)result.Result;
        Assert.AreEqual(mappedAgents, okResult?.Value);
    }
    
    [Test]
    public void TestInboundConnection_ReturnsOkResult() {
        // Arrange
        // Act
        var result = agentsController.testInboundConnection();

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;
        Assert.AreEqual("--> inbound test of from agents controller", okResult?.Value);
    }
}