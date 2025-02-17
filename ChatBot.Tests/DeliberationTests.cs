using ChatBot.Features.Chat;
using ChatBot.Features.Deliberation;
using Microsoft.Extensions.Options;

namespace ChatBot.Tests;

public class DeliberationTests
{
    private static ChatService _chatService = null!;
    private static DeliberationService _deliberationService = null!;

    [Before(Class)]
    public static void SetUp()
    {
        var options = Configuration.GetOptions<ChatCompletionsOptions>(nameof(ChatCompletionsOptions));
        _chatService = new ChatService(Options.Create(options));
        _deliberationService = new DeliberationService(_chatService);
    }

    [Test, NotInParallel]
    public async Task StudentFailedTest()
    {
        // Arrange
        var grades = new StudentGrades(Math: 95, Science: 85, English: 75, History: 65, Geography: 55, Art: 45, Music: 35);

        // Act
        var result = await _deliberationService.GetResultAsync("Jan", 12, grades);

        // Assert
        await Assert.That(result.Certificate).IsEqualTo("C");
        await Assert.That(result.Note).IsNotEmpty();
    }

    [Test, NotInParallel]
    public async Task StudentHolidayTaskTest()
    {
        // Arrange
        var grades = new StudentGrades(Math: 90, Science: 80, English: 70, History: 60, Geography: 70, Art: 80, Music: 30);
        
        // Act
        var result = await _deliberationService.GetResultAsync("Karen", 8, grades);

        // Assert
        await Assert.That(result.Certificate).IsEqualTo("B");
        await Assert.That(result.Note).IsNotEmpty();
    }

    [Test, NotInParallel]
    public async Task StudentPassedTest()
    {
        // Arrange
        var grades = new StudentGrades(Math: 90, Science: 80, English: 70, History: 60, Geography: 70, Art: 60, Music: 70);
        
        // Act
        var result = await _deliberationService.GetResultAsync("Johan", 6, grades);

        // Assert
        await Assert.That(result.Certificate).IsEqualTo("A");
        await Assert.That(result.Note).IsNotEmpty();
    }

    [After(Class)]
    public static void CleanUp()
    {
        _chatService.Dispose();
    }
}
