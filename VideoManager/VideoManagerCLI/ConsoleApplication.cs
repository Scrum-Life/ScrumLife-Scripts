using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using VideoManager.Application.ApplicationService;

namespace VideoManager.CLI
{
    internal class ConsoleApplication
    {
        private readonly ILogger<ConsoleApplication> _logger;
        private readonly IApplicationService _appService;

        public ConsoleApplication(ILogger<ConsoleApplication> logger, IApplicationService appService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appService = appService ?? throw new ArgumentNullException(nameof(appService));
        }

        public async Task Run()
        {
            _logger.LogDebug("Run application");

            using (FileStream videoStream = new FileStream(@"C:\Users\admin\Videos\ForBiggerBlazes.mp4", FileMode.Open))
            {
                //await _appService.UploadVideoAsync(videoStream);
                await _appService.GetRecords();
               // System.Collections.Generic.IDictionary<string, string> cat = await _appService.GetVideoCategories();
            }

            //await _appService.UploadCaptionToVideoAsync("T-vVit0swpc", "fr", File.ReadAllBytes(@"C:\Users\admin\Documents\Scrum Life\test.srt"));
        }
    }
}
