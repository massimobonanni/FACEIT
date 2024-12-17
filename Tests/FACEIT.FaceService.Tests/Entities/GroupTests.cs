using System;
using System.Collections.Generic;
using System.Text.Json;
using FACEIT.FaceService.Entities;
using Xunit;

namespace FACEIT.FaceService.Tests.Entities
{
    public class GroupTests
    {
        [Fact]
        public void ToCoreGroup_WithValidUserData_ReturnsCoreGroupWithProperties()
        {
            // Arrange
            var group = new Group
            {
                LargePersonGroupId = "testGroupId",
                Name = "Test Group",
                UserData = JsonSerializer.Serialize(new Dictionary<string, string>
                {
                    { "key1", "value1" },
                    { "key2", "value2" }
                })
            };

            // Act
            var coreGroup = group.ToCoreGroup();

            // Assert
            Assert.Equal("testGroupId", coreGroup.Id);
            Assert.Equal("Test Group", coreGroup.Name);
            Assert.NotNull(coreGroup.Properties);
            Assert.Equal(2, coreGroup.Properties.Count);
            Assert.Equal("value1", coreGroup.Properties["key1"]);
            Assert.Equal("value2", coreGroup.Properties["key2"]);
        }

        [Fact]
        public void ToCoreGroup_WithNullUserData_ReturnsCoreGroupWithNullProperties()
        {
            // Arrange
            var group = new Group
            {
                LargePersonGroupId = "testGroupId",
                Name = "Test Group",
                UserData = null
            };

            // Act
            var coreGroup = group.ToCoreGroup();

            // Assert
            Assert.Equal("testGroupId", coreGroup.Id);
            Assert.Equal("Test Group", coreGroup.Name);
            Assert.Null(coreGroup.Properties);
        }

        [Fact]
        public void ToCoreGroup_WithEmptyUserData_ReturnsCoreGroupWithNullProperties()
        {
            // Arrange
            var group = new Group
            {
                LargePersonGroupId = "testGroupId",
                Name = "Test Group",
                UserData = string.Empty
            };

            // Act
            var coreGroup = group.ToCoreGroup();

            // Assert
            Assert.Equal("testGroupId", coreGroup.Id);
            Assert.Equal("Test Group", coreGroup.Name);
            Assert.Null(coreGroup.Properties);
        }

        [Fact]
        public void ToCoreGroup_WithInvalidUserData_ReturnsCoreGroupWithNullProperties()
        {
            // Arrange
            var group = new Group
            {
                LargePersonGroupId = "testGroupId",
                Name = "Test Group",
                UserData = "invalid json"
            };

            // Act
            var coreGroup = group.ToCoreGroup();

            // Assert
            Assert.Equal("testGroupId", coreGroup.Id);
            Assert.Equal("Test Group", coreGroup.Name);
            Assert.Null(coreGroup.Properties);
        }
    }
}
