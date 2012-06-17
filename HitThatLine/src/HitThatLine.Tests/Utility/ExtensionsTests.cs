using FubuMVC.Core.Assets.Files;
using FubuMVC.Core.Urls;
using FubuMVC.Core.View;
using HitThatLine.Utility;
using Moq;
using NUnit.Framework;

namespace HitThatLine.Tests.Utility
{
    public class ExtensionsTests
    {
        [TestFixture]
        public class When_asked_for_svg_image
        {
            [Test]
            public void Should_create()
            {
                var fubuPage = new Mock<IFubuPage>();
                var urlRegistry = new Mock<IUrlRegistry>();

                fubuPage
                    .Setup(x => x.Urls)
                    .Returns(urlRegistry.Object);
                urlRegistry
                    .Setup(x => x.UrlForAsset(AssetFolder.images, "hitthatline.svg"))
                    .Returns(@"/HitThatLine/_content/images/hitthatline.svg");

                var svgImage = fubuPage.Object.SvgImage("hitthatline.svg").ToPrettyString();
                svgImage.ShouldEqual(@"<object data=""/HitThatLine/content/images/hitthatline.svg"" type=""image/svg+xml"">

</object>");
            }

            [Test]
            public void Should_create_with_fallback()
            {
                var fubuPage = new Mock<IFubuPage>();
                var urlRegistry = new Mock<IUrlRegistry>();

                fubuPage
                    .Setup(x => x.Urls)
                    .Returns(urlRegistry.Object);
                urlRegistry
                    .Setup(x => x.UrlForAsset(AssetFolder.images, "hitthatline.svg"))
                    .Returns(@"/HitThatLine/_content/images/hitthatline.svg");
                urlRegistry
                    .Setup(x => x.UrlForAsset(AssetFolder.images, "hitthatline.png"))
                    .Returns(@"/HitThatLine/_content/images/hitthatline.png");

                var svgImage = fubuPage.Object.SvgImage("hitthatline.svg", "hitthatline.png").ToPrettyString();
                svgImage.ShouldEqual(@"<object data=""/HitThatLine/content/images/hitthatline.svg"" type=""image/svg+xml"">
  <img src=""/HitThatLine/_content/images/hitthatline.png"" />
</object>");
            }
        }
    }
}