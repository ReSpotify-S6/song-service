
using SongService.Messaging;

namespace SongService.Services;

public class DeletedResourceListener(IEventListener eventListener, ISongService songService) : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        eventListener.Subscribe<string>("deleted-blobs", songService.OnDeletedAudioOrImage);
        return Task.CompletedTask;
    }
}
