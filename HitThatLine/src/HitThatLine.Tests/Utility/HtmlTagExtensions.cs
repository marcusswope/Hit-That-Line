using FubuMVC.Core.Assets.Files;
using FubuMVC.Core.Urls;
using FubuMVC.Core.View;
using HtmlTags;
using Moq;
using NUnit.Framework;
using HitThatLine.Utility;

namespace HitThatLine.Tests.Utility
{
    [TestFixture]
    public class HtmlTagExtensions
    {
        [TestFixture]
        public class Grid
        {
            [Test]
            public void Adds960GridClass()
            {
                var tag = new HtmlTag("div");
                tag.Grid(12);
                tag.HasClass("grid_12").ShouldBeTrue();

                tag.Grid();
                tag.HasClass("grid").ShouldBeTrue();
            }
        }

        [TestFixture]
        public class Prefix
        {
            [Test]
            public void Adds960GridClass()
            {
                var tag = new HtmlTag("div");
                tag.Prefix(12);
                tag.HasClass("prefix_12").ShouldBeTrue();
            }
        }

        [TestFixture]
        public class Suffix
        {
            [Test]
            public void Adds960GridClass()
            {
                var tag = new HtmlTag("div");
                tag.Suffix(12);
                tag.HasClass("suffix_12").ShouldBeTrue();
            }
        }

        [TestFixture]
        public class Clear
        {
            [Test]
            public void CreatesDivWithClearClass()
            {
                var tag = (null as IFubuPage).Clear();
                tag.TagName().ShouldEqual("div");
                tag.HasClass("clear").ShouldBeTrue();
            }

            [Test]
            public void CreatesDivWithClearClassAndHeight()
            {
                var tag = (null as IFubuPage).Clear(10);
                tag.Style("height").ShouldEqual("10px");
            }
        }

        [TestFixture]
        public class SvgImage
        {
            [Test]
            public void Creates()
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
            public void CreatesWithFallback()
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

        [TestFixture]
        public class Autofocus
        {
            [Test]
            public void AddsAutofocusAttribute()
            {
                var tag = new HtmlTag("input");
                tag.Autofocus();
                tag.Attr("autofocus").ShouldEqual("autofocus");
            }
        }

        [TestFixture]
        public class Placeholder
        {
            [Test]
            public void AddsPlaceholderAttribute()
            {
                var tag = new HtmlTag("input");
                tag.Placeholder("dummy text");
                tag.Attr("placeholder").ShouldEqual("dummy text");
            }
        }

        [TestFixture]
        public class TextBoxOnRight
        {
            [Test]
            public void Adds960GridClassAndtextboxonrightClass()
            {
                var tag = new HtmlTag("label");
                tag.TextBoxOnRight();
                tag.HasClass("grid").ShouldBeTrue();
                tag.HasClass("textbox-on-right").ShouldBeTrue();
            }
        }

        [TestFixture]
        public class Autocomplete
        {
            [Test]
            public void SetsOn()
            {
                var tag = new HtmlTag("input");
                tag.Autocomplete(true);
                tag.Attr("autocomplete").ShouldEqual("on");
            }

            [Test]
            public void SetsOff()
            {
                var tag = new HtmlTag("input");
                tag.Autocomplete(false);
                tag.Attr("autocomplete").ShouldEqual("off");
            }
        }
    }
}