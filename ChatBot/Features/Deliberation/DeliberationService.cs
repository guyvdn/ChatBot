using ChatBot.Features.Chat;

namespace ChatBot.Features.Deliberation;

public class DeliberationService(ChatService chatService)
{
    public async Task<DeliberationResult> GetResultAsync(string student, int age, StudentGrades grades)
    {
        var role =
            $"""
             Act as a teacher deliberating {age} year old students. 
             Speak to them in a way that is appropriate for their age. 
             Stay positive, all students worked hard! 
             Answer in json format.
             """;

        var query =
            $"""
             These are the grades of {student}: {grades}
             When the student has at least 2 scores less than 50 they will have to redo their year and get a C certificate.
             When they have only one score less than 50 they will have to do a holiday task for it and get a B certificate.
             In all other cases they pass and can go to the next year and get an A certificate.
             What is the outcome for this student? Give me the Certificate and a 2 line Note informing them of the result and what's next.
             Specify the classes they failed when applicable. 
             """;

        Console.WriteLine("-- Query --");
        Console.WriteLine(query);
        Console.WriteLine();

        var result = await chatService.QueryAsync<DeliberationResult>(role, query);

        Console.WriteLine("-- Result --");
        Console.WriteLine(result);

        return result;
    }
}