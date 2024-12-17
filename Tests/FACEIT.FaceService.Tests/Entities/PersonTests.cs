using System;
using System.Collections.Generic;
using System.Text.Json;
using FACEIT.FaceService.Entities;
using Xunit;

namespace FACEIT.FaceService.Tests.Entities
{
    public class PersonTests
    {
        [Fact]
        public void ToCorePerson_WithValidUserData_ReturnsCorePersonWithProperties()
        {
            // Arrange
            var person = new Person
            {
                Id = "123",
                Name = "John Doe",
                UserData = JsonSerializer.Serialize(new Dictionary<string, string> { { "key1", "value1" }, { "key2", "value2" } }),
                PersistedFaceIds = new List<string> { "faceId1", "faceId2" }
            };

            // Act
            var corePerson = person.ToCorePerson();

            // Assert
            Assert.Equal(person.Id, corePerson.Id);
            Assert.Equal(person.Name, corePerson.Name);
            Assert.Equal(2, corePerson.Properties.Count);
            Assert.Equal("value1", corePerson.Properties["key1"]);
            Assert.Equal("value2", corePerson.Properties["key2"]);
            Assert.Equal(person.PersistedFaceIds, corePerson.PersistedFaceIds);
        }

        [Fact]
        public void ToCorePerson_WithNullUserData_ReturnsCorePersonWithNullProperties()
        {
            // Arrange
            var person = new Person
            {
                Id = "123",
                Name = "John Doe",
                UserData = null,
                PersistedFaceIds = new List<string> { "faceId1", "faceId2" }
            };

            // Act
            var corePerson = person.ToCorePerson();

            // Assert
            Assert.Equal(person.Id, corePerson.Id);
            Assert.Equal(person.Name, corePerson.Name);
            Assert.Null(corePerson.Properties);
            Assert.Equal(person.PersistedFaceIds, corePerson.PersistedFaceIds);
        }

        [Fact]
        public void ToCorePerson_WithEmptyUserData_ReturnsCorePersonWithNullProperties()
        {
            // Arrange
            var person = new Person
            {
                Id = "123",
                Name = "John Doe",
                UserData = string.Empty,
                PersistedFaceIds = new List<string> { "faceId1", "faceId2" }
            };

            // Act
            var corePerson = person.ToCorePerson();

            // Assert
            Assert.Equal(person.Id, corePerson.Id);
            Assert.Equal(person.Name, corePerson.Name);
            Assert.Null(corePerson.Properties);
            Assert.Equal(person.PersistedFaceIds, corePerson.PersistedFaceIds);
        }

        [Fact]
        public void ToCorePerson_WithInvalidUserData_ReturnsCorePersonWithNullProperties()
        {
            // Arrange
            var person = new Person
            {
                Id = "123",
                Name = "John Doe",
                UserData = "not valid user data",
                PersistedFaceIds = new List<string> { "faceId1", "faceId2" }
            };

            // Act
            var corePerson = person.ToCorePerson();

            // Assert
            Assert.Equal(person.Id, corePerson.Id);
            Assert.Equal(person.Name, corePerson.Name);
            Assert.Null(corePerson.Properties);
            Assert.Equal(person.PersistedFaceIds, corePerson.PersistedFaceIds);
        }
    }
}
