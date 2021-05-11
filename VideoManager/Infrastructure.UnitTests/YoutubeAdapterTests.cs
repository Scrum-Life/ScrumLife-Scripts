using FluentAssertions;
using VideoManager.Infrastructure.YouTube;
using Xunit;

namespace Infrastructure.UnitTests
{
    public class YoutubeAdapterTests
    {
        [Theory]
        [InlineData("http://www.youtube.com/watch?v=0zM3nApSvMg&feature=feedrec_grec_index", "0zM3nApSvMg")]
        [InlineData("http://www.youtube.com/user/IngridMichaelsonVEVO#p/a/u/1/QdK8U-VIH_o", "QdK8U-VIH_o")]
        [InlineData("http://www.youtube.com/v/0zM3nApSvMg?fs=1&amp;hl=en_US&amp;rel=0", "0zM3nApSvMg")]
        [InlineData("http://www.youtube.com/watch?v=0zM_nApSvMg#t=0m10s", "0zM_nApSvMg")]
        [InlineData("http://www.youtube.com/embed/0zM3nApSvMg?rel=0", "0zM3nApSvMg")]
        [InlineData("http://www.youtube.com/watch?v=0zM3nApSvMg", "0zM3nApSvMg")]
        [InlineData("http://youtu.be/T-vVit0swpc", "T-vVit0swpc")]
        public void GetVideoIdFromUrl_WhenValidUrl_ReturnsExpectedId(string url, string expectedId)
        {
            YoutubeAdapter.GetVideoIdFromUrl(url).Should().Be(expectedId);
        }

        [Theory]
        [InlineData("https://vimeo.com/45901503")]
        [InlineData("https://www.dailymotion.com/video/x815aw2")]
        /*[InlineData("http://www.dummy.com/watch?v=0zM3nApSvMg")]*/ //TODO gérer ce cas, il ne faudrait pas pouvoir prendre n'importe quoi comme URL
        /*[InlineData("http://www.youtube.com/xyzv/0zM3nApSvMg?fs=1&amp;hl=en_US&amp;rel=0")]*/ //TODO la regex est trop faible à ce niveau-là
        [InlineData("")]
        [InlineData(null)]
        public void GetVideoIdFromUrl_WhenNonYoutubeUrl_ReturnsNull(string url)
        {
            YoutubeAdapter.GetVideoIdFromUrl(url).Should().BeNull();
        }
    }
}