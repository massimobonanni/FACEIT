using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FACEIT.Core.Tests.Extensions
{
    public class IEnumerableExtensionsTests
    {
        [Fact]
        public void ToProperties_ShouldReturnNull_WhenSourceIsNull()
        {
            // Arrange
            IEnumerable<string> source = null;
            
            // Act
            var result = source.ToProperties();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ToProperties_ShouldReturnEmptyDictionary_WhenSourceIsEmpty()
        {
            // Arrange
            IEnumerable<string> source = new string[] { };

            // Act
            var result = source.ToProperties();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Theory]
        [MemberData(nameof(GetValidSourceData))]
        public void ToProperties_ShouldReturnDictionary_WhenSourceIsValid(IEnumerable<string> source, IDictionary<string, string> expected)
        {
            // Act
            var result = source.ToProperties();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expected.Count, result.Count);
            foreach (var kvp in expected)
            {
                Assert.Equal(kvp.Value, result[kvp.Key]);
            }
        }

        [Theory]
        [MemberData(nameof(GetNotValidSourceData))]
        public void ToProperties_ShouldThrowException_WhenSourceIsInvalid(IEnumerable<string> source)
        {
            // Act & Assert
            Assert.Throws<IndexOutOfRangeException>(() => source.ToProperties());
        }

        public static IEnumerable<object[]> GetValidSourceData()
        {
            yield return new object[]
            {
                new List<string> { "key1:value1", "key2:value2", "key3:value3" },
                new Dictionary<string, string>
                {
                    { "key1", "value1" },
                    { "key2", "value2" },
                    { "key3", "value3" }
                }
            };

            yield return new object[]
            {
                new List<string> { "key1:value1", "key2:value2" },
                new Dictionary<string, string>
                {
                    { "key1", "value1" },
                    { "key2", "value2" }
                }
            };

            yield return new object[]
            {
                new List<string> { "key1:value1" },
                new Dictionary<string, string>
                {
                    { "key1", "value1" }
                }
            };

            yield return new object[]
            {
                new List<string> { "key1:" },
                new Dictionary<string, string>
                {
                    { "key1", "" }
                }
            };
        }

        public static IEnumerable<object[]> GetNotValidSourceData()
        {
            yield return new object[]
            {
                new List<string> { "notvalid", "key2:value2", "key3:value3" }
            };

            yield return new object[]
            {
                new List<string> { "key1:value1", "notvalid", "key3:value3" }
            };

            yield return new object[]
            {
                new List<string> { "key1:value1", "key2:value2", "notvalid" }
            };
        }
    }
}
