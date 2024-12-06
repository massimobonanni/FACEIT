using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.FaceService.Utilities
{
    /// <summary>
    /// Provides utility methods for validating various identifiers and names.
    /// </summary>
    internal static class ValidationUtility
    {
        /// <summary>
        /// Validates the specified group ID.
        /// </summary>
        /// <param name="groupId">The group ID to validate.</param>
        /// <param name="errorMessage">The error message if validation fails.</param>
        /// <returns><c>true</c> if the group ID is valid; otherwise, <c>false</c>.</returns>
        internal static bool ValidateGroupId(string groupId, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(groupId))
            {
                errorMessage = "Group ID cannot be null or empty.";
                return false;
            }

            if (groupId.Any(char.IsWhiteSpace) || groupId.Any(char.IsUpper))
            {
                errorMessage = "Group ID is not valid (cannot contain spaces or uppercase letters).";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        /// <summary>
        /// Validates the specified group name.
        /// </summary>
        /// <param name="name">The group name to validate.</param>
        /// <param name="errorMessage">The error message if validation fails.</param>
        /// <returns><c>true</c> if the group name is valid; otherwise, <c>false</c>.</returns>
        internal static bool ValidateGroupName(string name, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                errorMessage = "Name cannot be null or empty.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        /// <summary>
        /// Validates the specified person ID.
        /// </summary>
        /// <param name="personId">The person ID to validate.</param>
        /// <param name="errorMessage">The error message if validation fails.</param>
        /// <returns><c>true</c> if the person ID is valid; otherwise, <c>false</c>.</returns>
        internal static bool ValidatePersonId(string personId, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(personId))
            {
                errorMessage = "Person ID cannot be null or empty.";
                return false;
            }

            if (!Guid.TryParse(personId, out _))
            {
                errorMessage = "Person ID is not valid (must be a GUID).";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        /// <summary>
        /// Validates the specified person name.
        /// </summary>
        /// <param name="name">The person name to validate.</param>
        /// <param name="errorMessage">The error message if validation fails.</param>
        /// <returns><c>true</c> if the person name is valid; otherwise, <c>false</c>.</returns>
        internal static bool ValidatePersonName(string name, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                errorMessage = "Name cannot be null or empty.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }

        /// <summary>
        /// Validates the specified image ID.
        /// </summary>
        /// <param name="imageId">The image ID to validate.</param>
        /// <param name="errorMessage">The error message if validation fails.</param>
        /// <returns><c>true</c> if the image ID is valid; otherwise, <c>false</c>.</returns>
        internal static bool ValidateImageId(string imageId, out string errorMessage)
        {
            if (string.IsNullOrWhiteSpace(imageId))
            {
                errorMessage = "Image ID cannot be null or empty.";
                return false;
            }

            if (!Guid.TryParse(imageId, out _))
            {
                errorMessage = "Image ID is not valid (must be a GUID).";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}
