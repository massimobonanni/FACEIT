# FACEIT.Client

## Description

FACEIT.Client is a Windows Presentation Foundation (WPF) desktop application that provides a user interface for facial recognition and access control. This client application leverages Azure Cognitive Services Face API to deliver real-time facial recognition capabilities, enabling secure and contactless identity verification.

The client application features:
- **Real-time face detection and recognition** using camera input
- **Person and group management** for organizing recognized individuals
- **Azure Cognitive Services integration** for AI-powered facial recognition
- **Flexible authentication** supporting both API key and Azure identity-based authorization
- **Modern MVVM architecture** using CommunityToolkit.Mvvm for maintainable and testable code

## Prerequisites

Before running the FACEIT.Client application, ensure you have:

- Windows operating system (Windows 10 or later recommended)
- .NET SDK (version compatible with the project)
- An Azure subscription with access to Cognitive Services
- A webcam or compatible camera device for face detection

## Configuration

The application uses a configuration file to connect to Azure Cognitive Services Face API. Configuration settings are loaded from `appsettings.json` and can be overridden using `appsettings.local.json` for local development.

### Configuration File Structure

Create or update the `appsettings.local.json` file in the project root with the following structure:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "FacesManager": {
    "Key": "",
    "Endpoint": "https://your-face-service.cognitiveservices.azure.com/",
    "TenantId": "your-tenant-id",
    "ClientId": "your-client-id",
    "ClientSecret": "your-client-secret"
  }
}
```

### Configuration Parameters

- **Key**: (Optional) The API key for Azure Cognitive Services Face API. Leave empty if using identity-based authentication.
- **Endpoint**: The endpoint URL of your Azure Cognitive Services Face resource (e.g., `https://your-resource-name.cognitiveservices.azure.com/`).
- **TenantId**: Azure AD tenant ID for identity-based authentication.
- **ClientId**: Azure AD application (client) ID for identity-based authentication.
- **ClientSecret**: Azure AD application client secret for identity-based authentication.

> **Note**: The application will use identity-based authorization if `TenantId`, `ClientId`, and `ClientSecret` are all configured. Otherwise, it will fall back to using the API `Key`.

## Azure Setup

To use FACEIT.Client with Azure Cognitive Services, you need to:

1. Create an Azure Cognitive Services Face resource
2. Set up authentication (either API key or identity-based)

### Step 1: Create Azure Face API Resource

1. Sign in to the [Azure Portal](https://portal.azure.com)
2. Click **Create a resource**
3. Search for **Face** and select **Face** from Microsoft
4. Click **Create** and fill in the required details:
   - **Subscription**: Select your Azure subscription
   - **Resource group**: Create new or select existing
   - **Region**: Choose a region close to your location
   - **Name**: Enter a unique name for your resource
   - **Pricing tier**: Select an appropriate pricing tier
5. Click **Review + Create**, then **Create**
6. Once deployed, navigate to your Face resource

### Step 2: Configure Authentication

#### Option A: Using API Key (Simpler, Less Secure)

1. In your Face resource, navigate to **Keys and Endpoint**
2. Copy **KEY 1** and the **Endpoint** URL
3. Update your `appsettings.local.json`:
   ```json
   "FacesManager": {
     "Key": "your-api-key-here",
     "Endpoint": "https://your-resource-name.cognitiveservices.azure.com/",
     "TenantId": "",
     "ClientId": "",
     "ClientSecret": ""
   }
   ```

#### Option B: Using Azure AD Identity (Recommended for Production)

Identity-based authentication provides better security through Azure AD and supports managed identities.

##### 1. Register an Application in Azure AD

1. In Azure Portal, navigate to **Azure Active Directory**
2. Select **App registrations** → **New registration**
3. Enter an application name (e.g., "FACEIT-Client")
4. Select **Accounts in this organizational directory only**
5. Click **Register**
6. Note the **Application (client) ID** and **Directory (tenant) ID**

##### 2. Create a Client Secret

1. In your app registration, go to **Certificates & secrets**
2. Click **New client secret**
3. Add a description and select an expiration period
4. Click **Add**
5. **Important**: Copy the secret **Value** immediately (it won't be shown again)

##### 3. Assign the Cognitive Services Face Recognizer Role

To allow your application to access the Face API, you must assign it the appropriate role:

1. Navigate to your **Face API resource** in Azure Portal
2. Select **Access control (IAM)** from the left menu
3. Click **Add** → **Add role assignment**
4. In the **Role** tab:
   - Search for and select **Cognitive Services Face Recognizer**
   - Click **Next**
5. In the **Members** tab:
   - Select **User, group, or service principal**
   - Click **Select members**
   - Search for and select your application (e.g., "FACEIT-Client")
   - Click **Select**, then **Next**
6. In the **Review + assign** tab:
   - Review the settings
   - Click **Review + assign**

> **Available Roles for Face API**:
> - **Cognitive Services Face Recognizer**: Allows face detection, verification, and identification operations
> - **Cognitive Services User**: General access to Cognitive Services
> - **Cognitive Services Contributor**: Full access including management operations

##### 4. Update Configuration

Update your `appsettings.local.json` with the identity configuration:

```json
"FacesManager": {
  "Key": "",
  "Endpoint": "https://your-resource-name.cognitiveservices.azure.com/",
  "TenantId": "your-tenant-id",
  "ClientId": "your-application-client-id",
  "ClientSecret": "your-client-secret-value"
}
```

## Running the Application

1. Ensure your `appsettings.local.json` is properly configured
2. Build the solution in Visual Studio or using the .NET CLI:
   ```bash
   dotnet build
   ```
3. Run the application:
   ```bash
   dotnet run --project FACEIT.Client
   ```
   Or press **F5** in Visual Studio

## Security Best Practices

- **Never commit** `appsettings.local.json` to source control (it should be in `.gitignore`)
- **Use Azure Key Vault** for storing secrets in production environments
- **Rotate credentials** regularly, especially client secrets
- **Use Managed Identities** when deploying to Azure services (App Service, Azure VM, etc.)
- **Apply principle of least privilege**: Assign only the minimum required role (Face Recognizer)

## Troubleshooting

### Common Issues

**Authentication Errors**
- Verify that your `TenantId`, `ClientId`, and `ClientSecret` are correct
- Ensure the Face Recognizer role is properly assigned to your app registration
- Check that the client secret hasn't expired

**Endpoint Errors**
- Verify the endpoint URL format: `https://your-resource-name.cognitiveservices.azure.com/`
- Ensure there are no trailing slashes or typos in the URL

**Camera Not Detected**
- Verify that a camera is connected and enabled
- Check Windows privacy settings to ensure camera access is allowed for desktop apps

## Additional Resources

- [Azure Cognitive Services Face API Documentation](https://learn.microsoft.com/azure/cognitive-services/face/)
- [Azure Role-Based Access Control (RBAC)](https://learn.microsoft.com/azure/role-based-access-control/overview)
- [Best practices for Azure AD application authentication](https://learn.microsoft.com/azure/active-directory/develop/identity-platform-integration-checklist#security)

## License

Please refer to the main repository license for licensing information.
