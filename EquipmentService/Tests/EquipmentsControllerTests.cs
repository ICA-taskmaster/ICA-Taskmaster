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
public class EquipmentsControllerTests {
    private EquipmentsController controller;
    private Mock<IEquipmentRespository> repositoryMock;
    private Mock<IMapper> mapperMock;
    private Mock<IConfiguration> configurationMock;
    
    [SetUp]
    public void Setup() {
        repositoryMock = new Mock<IEquipmentRespository>();
        mapperMock = new Mock<IMapper>();
        configurationMock = new Mock<IConfiguration>();
        
        controller = new EquipmentsController(
            repositoryMock.Object, 
            mapperMock.Object, 
            configurationMock.Object
        );
    }
    
    [Test]
    public void getEquipmentsForAgent_ReturnsOkResult() {
        // Arrange
        const int agentId = 7;
        var fakeEquipments = new List<Equipment> {
            new () {
                id = 1,
                item = "ICA19 Silverballer",
                description = "The Silverballer is a semi-automatic pistol...",
                status = "Factory new",
                agentId = 7
            },
            new () {
                id = 2,
                item = "Sieger 300",
                description = "The Sieger 300 is a sniper rifle...",
                status = "Field tested",
                agentId = 7
            }
        };
        
        // Map the fakeEquipments list to EquipmentFetchDto
        var mappedEquipments  = new List<EquipmentFetchDto> {
            new (1, "ICA19 Silverballer", "The Silverballer is a semi-automatic pistol...", "Factory new", "some-url", 7),
            new (2, "Sieger 300", "The Sieger 300 is a sniper rifle...", "Field tested", "some-url", 7)
        };
        // Mock the repository behavior
        repositoryMock.Setup(repo => repo.agentExists(agentId)).Returns(true);
        repositoryMock.Setup(repo => repo.getEquipmentsForAgent(agentId)).Returns(fakeEquipments);
        mapperMock.Setup(mapper => mapper.Map<IEnumerable<EquipmentFetchDto>>(fakeEquipments)).Returns(mappedEquipments);

        // Act
        var result = controller.getEquipmentsForAgent(agentId);

        // Assert
        Assert.That(result, Is.InstanceOf<ActionResult<IEnumerable<EquipmentFetchDto>>>());
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

        var okResult = (OkObjectResult)result.Result;
        var equipmentList = (IEnumerable<EquipmentFetchDto>)okResult?.Value;
        var equipmentFetchDtos = equipmentList?.ToArray() ?? Array.Empty<EquipmentFetchDto>();
        Assert.That(equipmentFetchDtos.Length, Is.EqualTo(2));
    }
    
    [Test]
    public void getEquipmentsForAgent_ReturnsNotFoundResult() {
        // Arrange
        const int agentId = 7;
        // Mock the repository behavior
        repositoryMock.Setup(repo => repo.agentExists(agentId)).Returns(false);

        // Act
        var result = controller.getEquipmentsForAgent(agentId);

        // Assert
        Assert.That(result, Is.InstanceOf<ActionResult<IEnumerable<EquipmentFetchDto>>>());
        Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
    }
}