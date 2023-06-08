using AgentService.Data;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace AgentService.Tests;

public class Agent
{
    private AppDbContext context;

    [SetUp]
    public void Setup() {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        context = new AppDbContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.agents.AddRange(
            new Models.Agent { realName = "47", codeName = "Agent 47", burnerPhone = "06336046925", securityClearance = "Orange" },
            new Models.Agent { realName = "Diana Penelope Burnwood", codeName = "Burnwood", burnerPhone = "06336375625", securityClearance = "Red" },
            new Models.Agent { realName = "Alexander Fanin", codeName = "Fan", burnerPhone = "06336500395", securityClearance = "Purple" }
        );
        context.SaveChanges();
    }

    [Test]
    public void GetAllAgents_ReturnsAllAgents() {
        // Arrange
        var repository = new AgentRepository(context);

        // Act
        var agents = repository.getAll();

        // Assert
        Assert.That(agents.Count(), Is.EqualTo(3));
    }
    
    [Test]
    public void GetAgentById_ReturnsAgentWithGivenId() {
        // Arrange
        var repository = new AgentRepository(context);

        // Act
        var agent = repository.getById(1);

        // Assert
        Assert.That(agent.codeName, Is.EqualTo("Agent 47"));
    }
    
    [Test]
    public void GetAgentById_ReturnsNullWhenGivenInvalidId() {
        // Arrange
        var repository = new AgentRepository(context);

        // Act
        var agent = repository.getById(4);

        // Assert
        Assert.That(agent, Is.Null);
    }
    
    [Test]
    public void AddAgent_AddsAgentToDatabase() {
        // Arrange
        var repository = new AgentRepository(context);
        var agent = new Models.Agent { realName = "Lucas Grey", codeName = "Grey", burnerPhone = "06336500395", securityClearance = "Purple" };

        // Act
        repository.create(agent);
        repository.saveChanges();
        var agents = repository.getAll();

        // Assert
        Assert.That(agents.Count(), Is.EqualTo(4));
    }
    
    [Test]
    public void DeleteAgent_RemovesAgentFromDatabase() {
        // Arrange
        var repository = new AgentRepository(context);
 
        // Act
        repository.delete(1);
        repository.saveChanges();
        var agents = repository.getAll();

        // Assert
        Assert.That(agents.Count(), Is.EqualTo(2));
    }
}