using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FACEIT.FaceService.Utilities
{
    internal static class ValidationUtility
    {

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
    }
}
