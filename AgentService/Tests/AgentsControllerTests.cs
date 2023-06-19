using AgentService.AsyncDataServices;
using AgentService.Controllers;
using AgentService.Data;
using AgentService.Dtos;
using AgentService.Models;
using AgentService.SyncDataServices.Http;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace AgentService.Tests;

[TestFixture]
public class AgentsControllerTests {
    private AgentsController controller;
    private Mock<IAgentRepository> repositoryMock;
    private Mock<IMapper> mapperMock;
    private Mock<IEquipmentDataClient> equipmentDataClientMock;
    private Mock<IMessageBusClient> messageBusClientMock;
    private Mock<ILogger<AgentsController>> loggerMock;

    [SetUp]
    public void Setup() {
        repositoryMock = new Mock<IAgentRepository>();
        mapperMock = new Mock<IMapper>();
        equipmentDataClientMock = new Mock<IEquipmentDataClient>();
        messageBusClientMock = new Mock<IMessageBusClient>();
        loggerMock = new Mock<ILogger<AgentsController>>();

        controller = new AgentsController(
            repositoryMock.Object,
            mapperMock.Object,
            equipmentDataClientMock.Object,
            messageBusClientMock.Object,
            loggerMock.Object
        );
    }

    [Test]
    public void GetAllAgents_ReturnsOkResultWithAgentFetchDtos() {
        // Arrange
        var fakeAgents = new List<Agent> {
            new() { id = 1, realName = "47", codeName = "Agent 47", burnerPhone = "06336046925", securityClearance = "Orange" },
            new() { id = 2, realName = "Diana Penelope Burnwood", codeName = "Burnwood", burnerPhone = "06336375625", securityClearance = "Red" }
        };
        var mappedAgents = new List<AgentFetchDto> {
            new(1, "47", "Agent 47", "06336046925", "Orange"),
            new(2, "Diana Penelope Burnwood", "Burnwood", "06336375625", "Red")
        };

        repositoryMock.Setup(repo => repo.getAll()).Returns(fakeAgents);
        mapperMock.Setup(mapper => mapper.Map<IEnumerable<AgentFetchDto>>(fakeAgents)).Returns(mappedAgents);

        // Act
        var result = controller.getAllAgents();

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

        var okResult = (OkObjectResult)result.Result;
        var agentList = (IEnumerable<AgentFetchDto>)okResult?.Value;
        var agentFetchDtos = agentList?.ToArray() ?? Array.Empty<AgentFetchDto>();
        Assert.That(agentFetchDtos.Length, Is.EqualTo(2));
    }

    [Test]
    public void GetAgentById_ExistingId_ReturnsOkResultWithAgentFetchDto()
    {
        // Arrange
        var agentId = 1;
        var agent = new Agent { id = 1, realName = "47", codeName = "Agent 47", burnerPhone = "06336046925", securityClearance = "Orange" };
        var agentFetchDto = new AgentFetchDto(agentId, "47", "Agent 47", "06336046925", "Orange");

        repositoryMock.Setup(repo => repo.getById(agentId)).Returns(agent);
        mapperMock.Setup(mapper => mapper.Map<AgentFetchDto>(agent)).Returns(agentFetchDto);

        // Act
        var result = controller.getAgentById(agentId);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

        var okResult = (OkObjectResult)result.Result;
        var returnedAgentFetchDto = (AgentFetchDto)okResult?.Value;
        Assert.That(returnedAgentFetchDto?.id, Is.EqualTo(agentId));
    }

    [Test]
    public void GetAgentById_NonExistingId_ReturnsNotFoundResult()
    {
        // Arrange
        var nonExistingAgentId = 100;
        repositoryMock.Setup(repo => repo.getById(nonExistingAgentId)).Returns((Agent)null);

        // Act
        var result = controller.getAgentById(nonExistingAgentId);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
    }
}