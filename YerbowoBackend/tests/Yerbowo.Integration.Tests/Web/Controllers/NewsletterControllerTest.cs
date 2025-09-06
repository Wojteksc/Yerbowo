namespace Yerbowo.Integration.Tests.Web.Controllers;

public class NewsletterControllerTest : ApiTestBase
{
    private readonly HttpClient _httpClient;
    private readonly IServiceScopeFactory _scope;

    public NewsletterControllerTest(WebApplicationFactory<Startup> factory) : base(factory)
    {
        _scope = WebApplicationFactory.Services.GetRequiredService<IServiceScopeFactory>();
        _httpClient = CreateClient();
    }

    [Fact]
    public async Task Resubscribe_Should_Work()
    {
        string email = "newsletter@testemail.com";

        await ReSubscribeScenario(email);
        var outboxMessagesFirstScnenario = await GetOutboxMessages();
        outboxMessagesFirstScnenario.Should().HaveCount(2);
        outboxMessagesFirstScnenario.Select(o => o.ProccessedAt).Should().NotBeNull();

        await ReSubscribeScenario(email);
        var outboxMessagesSecondScenario = await GetOutboxMessages();
        outboxMessagesSecondScenario.Should().HaveCount(4);
        outboxMessagesFirstScnenario.Select(o => o.ProccessedAt).Should().NotBeNull();
    }

    private async Task ReSubscribeScenario(string email)
    {
        await InviteNewsletterScenario(email);

        var newsletter = await GetNewsletter(email);

        await SubscribeNewsletterScenario(newsletter);

        (await IsNewsletterSubsribed(email)).Should().BeTrue();

        await UnsubscribeNewsletterScenario(newsletter);

        (await IsNewsletterSubsribed(email)).Should().BeFalse();

        await ExecuteOutboxMessagesJob(cancelAfter: TimeSpan.FromSeconds(5));
    }

    private async Task ExecuteOutboxMessagesJob(TimeSpan cancelAfter)
    {
        await using var scope = _scope.CreateAsyncScope();
        var loggerService = scope.ServiceProvider.GetRequiredService<ILogger<OutboxMessagesJob>>();
        var interfaceConverterJsonOptions = scope.ServiceProvider.GetRequiredService<IInterfaceConverterJsonOptions>();
        var job = new OutboxMessagesJob(_scope, loggerService, interfaceConverterJsonOptions);
        try
        {
            var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(cancelAfter);
            await job.StartAsync(tokenSource.Token);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Task cancelled");
        }
        catch (Exception ex)
        {
            throw new Exception($"Something went wrong while executing {nameof(OutboxMessagesJob)}", ex);
        }
    }

    private async Task<IReadOnlyList<OutboxMessage>> GetOutboxMessages()
    {
        await using var scope = _scope.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<YerbowoContext>();
        return dbContext.OutboxMessages.ToList();
    }

    private async Task<bool> IsNewsletterSubsribed(string email) => (await GetNewsletter(email)).IsSubscribed();

    private async Task<Newsletter> GetNewsletter(string email)
    {
        using var scope = _scope.CreateScope();

        var newsletterRepository = scope.ServiceProvider.GetRequiredService<INewsletterRepository>();

        return await newsletterRepository.GetAsync(email);
    }

    private async Task InviteNewsletterScenario(string email)
    {
        var command = new InviteNewsletterCommand
        {
            Email = email
        };

        var result = await _httpClient.PostAsync($"api/newsletter/invite", command);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task SubscribeNewsletterScenario(Newsletter newsletter)
    {
        var subscribeNewsletterCommand = new SubscribeNewsletterCommand(newsletter.Email, newsletter.VerificationToken);

        var response = await _httpClient.PostAsync($"api/newsletter/subscribe", subscribeNewsletterCommand);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task UnsubscribeNewsletterScenario(Newsletter newsletter)
    {
        var subscribeNewsletterCommand = new UnsubscribeNewsletterCommand(newsletter.Email, newsletter.VerificationToken);

        var response = await _httpClient.PostAsync($"api/newsletter/unsubscribe", subscribeNewsletterCommand);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}