using System;
using System.Linq;
using NUnit.Framework;

namespace Documentation
{
    [TestFixture]
    public class Specifier_should
    {
        private ISpecifier descriptor;

        [SetUp]
        protected virtual void Setup()
        {
            descriptor = new Specifier<VkApi>();
        }

        private const string authorizeMethodName = "Authorize";
        private const string selectAudioMethodName = "SelectAudio";
        private const string countAudioMethodName = "GetTotalAudioCount";

        [Test]
        public void GetApiMethodNamesWithDescription()
        {
            var actual = descriptor.GetApiMethodNames();
            CollectionAssert.AreEquivalent(
                new[] {authorizeMethodName, selectAudioMethodName, countAudioMethodName},
                actual);
        }

        [Test]
        public void GetApiDescription()
        {
            var description = descriptor.GetApiDescription();
            Assert.AreEqual("Vk API client", description);
        }

        [Test]
        public void GetApiMethodDescriptionRandomName()
        {
            var description = descriptor.GetApiMethodDescription(Guid.NewGuid().ToString());
            Assert.IsNull(description);
        }

        [Test]
        public void GetApiMethodDescription()
        {
            var description = descriptor.GetApiMethodDescription(authorizeMethodName);
            Assert.AreEqual("Authorize user. Returns true if authorized", description);
        }

        [Test]
        public void GetApiMethodParamNames()
        {
            var description = descriptor.GetApiMethodParamNames(authorizeMethodName);
            CollectionAssert.AreEquivalent(new[] {"login", "password", "allowNoname"}, description);
        }

        [Test]
        public void GetApiMethodParamDescriptionWithoutDescription()
        {
            var description = descriptor.GetApiMethodParamDescription(authorizeMethodName, "allowNoname");
            Assert.IsNull(description);
        }

        [Test]
        public void GetApiMethodParamDescription()
        {
            var description = descriptor.GetApiMethodParamDescription(selectAudioMethodName, "batchSize");
            Assert.AreEqual("number of audios to return", description);
        }

        [Test]
        public void GetApiMethodParamFullDescriptionRandomName()
        {
            var paramName = Guid.NewGuid().ToString();
            var description = descriptor.GetApiMethodParamFullDescription(authorizeMethodName, paramName);
            Assert.IsNotNull(description);
            Assert.IsNull(description.MinValue);
            Assert.IsNull(description.MaxValue);
            Assert.IsFalse(description.Required);
            var expected = new CommonDescription(paramName);
            AssertCommonDescriptionAreEquals(expected, description.ParamDescription);
        }

        [Test]
        public void GetApiMethodParamFullDescriptionNotAllAttributes()
        {
            var description = descriptor.GetApiMethodParamFullDescription(authorizeMethodName, "login");
            Assert.IsNotNull(description);
            Assert.IsNull(description.MinValue);
            Assert.IsNull(description.MaxValue);
            Assert.IsTrue(description.Required);
            var expected = new CommonDescription("login");
            AssertCommonDescriptionAreEquals(expected, description.ParamDescription);
        }

        [Test]
        public void GetApiMethodParamFullDescription()
        {
            var description = descriptor.GetApiMethodParamFullDescription(selectAudioMethodName, "batchSize");
            Assert.IsNotNull(description);
            Assert.AreEqual(1, description.MinValue);
            Assert.AreEqual(100, description.MaxValue);
            Assert.IsTrue(description.Required);
            var expected = new CommonDescription("batchSize", "number of audios to return");
            AssertCommonDescriptionAreEquals(expected, description.ParamDescription);
        }

        [Test]
        public void GetFullApiMethodDescriptionAuthorizeRandomName()
        {
            var description = descriptor.GetFullApiMethodDescription("Authorize2");
            Assert.IsNull(description);
        }

        [Test]
        public void GetFullApiMethodDescriptionAuthorize()
        {
            var description = descriptor.GetFullApiMethodDescription(authorizeMethodName);
            Assert.IsNotNull(description);
            var expected = new ApiMethodDescription
            {
                MethodDescription = new CommonDescription(authorizeMethodName,
                    "Authorize user. Returns true if authorized"),
                ParamDescriptions = new[]
                {
                    new ApiParamDescription
                    {
                        ParamDescription = new CommonDescription("login"),
                        Required = true
                    },
                    new ApiParamDescription
                    {
                        ParamDescription = new CommonDescription("password"),
                        Required = true
                    },
                    new ApiParamDescription
                    {
                        ParamDescription = new CommonDescription("allowNoname"),
                    },
                }
            };
            AssertDescriptionAreEquals(expected, description);
        }

        [Test]
        public void GetFullApiMethodDescriptionSelectAudio()
        {
            var description = descriptor.GetFullApiMethodDescription(selectAudioMethodName);
            Assert.IsNotNull(description);
            var expected = new ApiMethodDescription
            {
                MethodDescription = new CommonDescription(selectAudioMethodName,
                    "Gets user audio tracks. If userId is not presented gets authorized user audio tracks"),
                ParamDescriptions = new[]
                {
                    new ApiParamDescription
                    {
                        ParamDescription = new CommonDescription("userId"),
                    },
                    new ApiParamDescription
                    {
                        ParamDescription = new CommonDescription("batchSize", "number of audios to return"),
                        Required = true,
                        MinValue = 1,
                        MaxValue = 100
                    },
                },
                ReturnDescription = new ApiParamDescription
                {
                    ParamDescription = new CommonDescription()
                }
            };
            AssertDescriptionAreEquals(expected, description);
        }

        [Test]
        public void GetFullApiMethodDescriptionCountAudio()
        {
            var description = descriptor.GetFullApiMethodDescription(countAudioMethodName);
            Assert.IsNotNull(description);
            var expected = new ApiMethodDescription
            {
                MethodDescription = new CommonDescription(countAudioMethodName,
                    "Gets user audio tracks count. If userId is not presented gets authorized user audio tracks"),
                ParamDescriptions = new[]
                {
                    new ApiParamDescription
                    {
                        ParamDescription = new CommonDescription("userId"),
                    },
                },
                ReturnDescription = new ApiParamDescription
                {
                    Required = true,
                    ParamDescription = new CommonDescription(),
                    MinValue = 0,
                    MaxValue = int.MaxValue / 2
                }
            };
            AssertDescriptionAreEquals(expected, description);
        }

        #region AssertHelpers

        private static void AssertDescriptionAreEquals(ApiMethodDescription expected, ApiMethodDescription actual)
        {
            AssertCommonDescriptionAreEquals(expected.MethodDescription, actual.MethodDescription);

            AssertParamDescriptionAreEquals(expected.ReturnDescription, actual.ReturnDescription);

            if (expected.ParamDescriptions == null && actual.ParamDescriptions == null)
            {
                return;
            }

            Assert.AreEqual(expected.ParamDescriptions?.Length, actual.ParamDescriptions?.Length);

            var expectedParamDescriptions = expected.ParamDescriptions.OrderBy(x => x.ParamDescription?.Name).ToArray();
            var actualParamDescriptions = actual.ParamDescriptions.OrderBy(x => x.ParamDescription?.Name).ToArray();

            for (var i = 0; i < expected.ParamDescriptions.Length; i++)
            {
                AssertParamDescriptionAreEquals(expectedParamDescriptions[i], actualParamDescriptions[i]);
            }
        }

        private static void AssertParamDescriptionAreEquals(ApiParamDescription expected, ApiParamDescription actual)
        {
            Assert.AreEqual(expected?.MaxValue, actual?.MaxValue);
            Assert.AreEqual(expected?.MinValue, actual?.MinValue);
            Assert.AreEqual(expected?.Required, actual?.Required);

            AssertCommonDescriptionAreEquals(expected?.ParamDescription, actual?.ParamDescription);
        }

        private static void AssertCommonDescriptionAreEquals(CommonDescription expected, CommonDescription actual)
        {
            Assert.AreEqual(expected?.Name, actual?.Name);
            Assert.AreEqual(expected?.Description, actual?.Description);
        }

        #endregion
    }
}