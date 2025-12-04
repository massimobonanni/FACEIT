using FACEIT.Core.Interfaces;

namespace FACEIT.Console.Services
{
    /// <summary>
    /// Interface for creating face service instances.
    /// </summary>
    public interface IFaceServiceFactory
    {
        /// <summary>
        /// Creates an IGroupsManager instance.
        /// </summary>
        /// <param name="endpoint">The endpoint of the Azure Face Service.</param>
        /// <param name="apiKey">The API key of the Azure Face Service.</param>
        /// <returns>An IGroupsManager instance.</returns>
        IGroupsManager CreateGroupsManager(string? endpoint, string? apiKey);

        /// <summary>
        /// Creates an IPersonsManager instance.
        /// </summary>
        /// <param name="endpoint">The endpoint of the Azure Face Service.</param>
        /// <param name="apiKey">The API key of the Azure Face Service.</param>
        /// <returns>An IPersonsManager instance.</returns>
        IPersonsManager CreatePersonsManager(string? endpoint, string? apiKey);

        /// <summary>
        /// Creates an IFaceRecognizer instance.
        /// </summary>
        /// <param name="endpoint">The endpoint of the Azure Face Service.</param>
        /// <param name="apiKey">The API key of the Azure Face Service.</param>
        /// <returns>An IFaceRecognizer instance.</returns>
        IFaceRecognizer CreateFaceRecognizer(string? endpoint, string? apiKey);
    }
}
